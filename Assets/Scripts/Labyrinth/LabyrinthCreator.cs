using System;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthCreator : MonoBehaviour
{
    [SerializeField] private int _gridW = 20;
    [SerializeField] private int _gridH = 20;

    [SerializeField] private Transform _gridPosition;

    [SerializeField] private Grid _grid;

    [SerializeField] private GameObject _roomGOPrefab;

    [SerializeField] private int _seed = 1234;
    [SerializeField] private bool _addExtraLoops = false;
    [SerializeField, Range(0f, 0.3f)] private float _loopChance = 0.08f;

    // doors[x,y] bitmask: N=1, E=2, S=4, W=8
    private int[,] _doors;
    private System.Random _rng;

    private const int DIR_N = 1 << 0;
    private const int DIR_E = 1 << 1;
    private const int DIR_S = 1 << 2;
    private const int DIR_W = 1 << 3;

    public void GenerateLabyrinth()
    {
        if (_grid == null)
        {
            Debug.LogError("LabyrinthCreator: Grid reference missing.");
            return;
        }

        if (_roomGOPrefab == null)
        {
            Debug.LogError("LabyrinthCreator: Room prefab missing.");
            return;
        }

        if (_gridW <= 0 || _gridH <= 0)
        {
            Debug.LogError("LabyrinthCreator: Invalid grid size.");
            return;
        }

        if (_gridPosition == null)
        {
            Debug.LogError("LabyrinthCreator: Grid position missing.");
            return;
        }

        _rng = new System.Random(_seed);

        _grid.GenerateGrid(_gridW, _gridH, _gridPosition.position);

        CreateLabyrinth();
        //CreateTestLabyrinth();
    }

    public Vector3[] GetStartAndEndPositions()
    {
        
        Vector3 startRoomPosition = _grid.GetPositionOfRoom(0,0);
        Vector3 endRoomPosition = _grid.GetPositionOfRoom(_gridW-1,_gridH-1);

        return new Vector3[]{startRoomPosition,endRoomPosition};

    }

    private void CreateTestLabyrinth()
    {
        if (_roomGOPrefab == null)
        {
            Debug.LogError("LabyrinthCreator: Room prefab missing.");
            return;
        }

        // Helper to place a 1x1 room at (x,y) and open the specified sides (index 0)
        void PlaceTestRoom(int x, int y, bool openN, bool openE, bool openS, bool openW)
        {
            GameObject roomGO = Instantiate(_roomGOPrefab);
            roomGO.name = $"TestRoom_{x}_{y}";

            Room room = roomGO.GetComponent<Room>();
            if (room != null)
            {
                room.CloseAllEntrances();

                if (openN) room.OpenRoom1x1EntranceNorth();
                if (openE) room.OpenRoom1x1EntranceEast();
                if (openS) room.OpenRoom1x1EntranceSouth();
                if (openW) room.OpenRoom1x1EntranceWest();
            }

            _grid.PlaceObjectAt(roomGO, x, y);
        }

        // We build a "snake" path that fills the grid:
        // Row 0: (0,0) -> (W-1,0)
        // Up to row 1, then (W-1,1) -> (0,1)
        // Up to row 2, then (0,2) -> (W-1,2)
        // ... until last cell in the last row.
        //
        // This matches your description and is perfect to validate OpenEntrances correctness.
        for (int y = 0; y < _gridH; y++)
        {
            bool leftToRight = (y % 2 == 0);

            for (int i = 0; i < _gridW; i++)
            {
                int x = leftToRight ? i : (_gridW - 1 - i);

                bool openN = false;
                bool openE = false;
                bool openS = false;
                bool openW = false;

                // Horizontal connections inside the row
                if (leftToRight)
                {
                    if (x > 0) openW = true;                 // connected to previous (x-1)
                    if (x < _gridW - 1) openE = true;        // connected to next (x+1)
                }
                else
                {
                    if (x < _gridW - 1) openE = true;        // connected to previous (x+1) in the snake direction
                    if (x > 0) openW = true;                 // connected to next (x-1) in the snake direction
                }

                // Vertical connection at the end of each row to go up to the next row
                // Row 0 ends at x=W-1, Row 1 ends at x=0, Row 2 ends at x=W-1, etc.
                bool isRowEndCell = leftToRight ? (x == _gridW - 1) : (x == 0);

                if (isRowEndCell && y < _gridH - 1)
                {
                    openN = true; // this cell goes up
                }

                // And the cell above must have its South openOUTH open; we handle it here too
                // because we are placing every cell anyway.
                if (y > 0)
                {
                    // If the cell below (same x, y-1) ended the previous row, then this cell should open South.
                    bool prevRowLeftToRight = ((y - 1) % 2 == 0);
                    bool belowWasEnd = prevRowLeftToRight ? (x == _gridW - 1) : (x == 0);

                    if (belowWasEnd)
                        openS = true;
                }

                PlaceTestRoom(x, y, openN, openE, openS, openW);
            }
        }

        Debug.Log("CreateTestLabyrinth: Snake path generated for entrance validation.");
    }


    private void CreateLabyrinth()
    {
        // 1) Build a perfect maze using DFS backtracker
        _doors = new int[_gridW, _gridH];
        bool[,] visited = new bool[_gridW, _gridH];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        Vector2Int start = new Vector2Int(_rng.Next(0, _gridW), _rng.Next(0, _gridH));
        visited[start.x, start.y] = true;
        stack.Push(start);

        while (stack.Count > 0)
        {
            Vector2Int cur = stack.Peek();

            var neighbors = GetUnvisitedNeighbors(cur, visited);
            if (neighbors.Count == 0)
            {
                stack.Pop();
                continue;
            }

            var pick = neighbors[_rng.Next(neighbors.Count)];

            // carve both ways
            _doors[cur.x, cur.y] |= pick.dirBit;
            _doors[pick.n.x, pick.n.y] |= pick.oppBit;

            visited[pick.n.x, pick.n.y] = true;
            stack.Push(pick.n);
        }

        // 2) Optional loops (less "perfect", more labyrinth)
        if (_addExtraLoops)
            AddRandomLoops();

        // 3) Place a room prefab at every grid cell and open entrances based on door mask
        for (int y = 0; y < _gridH; y++)
        {
            for (int x = 0; x < _gridW; x++)
            {
                GameObject roomGO = Instantiate(_roomGOPrefab);
                roomGO.name = $"Room_{x}_{y}";

                Room room = roomGO.GetComponent<Room>();
                if (room != null)
                {
                    room.CloseAllEntrances();

                    int mask = _doors[x, y];

                    // For 1x1 rooms, entrance index per side is always 0
                    if ((mask & DIR_N) != 0) room.OpenEntrances(RoomSides.NORTH_SIDE, new[] { 0 });
                    if ((mask & DIR_E) != 0) room.OpenEntrances(RoomSides.EAST_SIDE,  new[] { 0 });
                    if ((mask & DIR_S) != 0) room.OpenEntrances(RoomSides.SOUTH_SIDE, new[] { 0 });
                    if ((mask & DIR_W) != 0) room.OpenEntrances(RoomSides.WEST_SIDE,  new[] { 0 });
                }

                // Place into your grid cell (assumes Grid has this method)
                _grid.PlaceObjectAt(roomGO, x, y);
            }
        }
    }

    // -------------------------
    // Helpers
    // -------------------------

    private List<(Vector2Int n, int dirBit, int oppBit)> GetUnvisitedNeighbors(Vector2Int c, bool[,] visited)
    {
        var list = new List<(Vector2Int, int, int)>(4);

        // N
        var n = new Vector2Int(c.x, c.y + 1);
        if (InBounds(n) && !visited[n.x, n.y]) list.Add((n, DIR_N, DIR_S));

        // E
        n = new Vector2Int(c.x + 1, c.y);
        if (InBounds(n) && !visited[n.x, n.y]) list.Add((n, DIR_E, DIR_W));

        // S
        n = new Vector2Int(c.x, c.y - 1);
        if (InBounds(n) && !visited[n.x, n.y]) list.Add((n, DIR_S, DIR_N));

        // W
        n = new Vector2Int(c.x - 1, c.y);
        if (InBounds(n) && !visited[n.x, n.y]) list.Add((n, DIR_W, DIR_E));

        return list;
    }

    private bool InBounds(Vector2Int c) => c.x >= 0 && c.y >= 0 && c.x < _gridW && c.y < _gridH;

    private void AddRandomLoops()
    {
        for (int y = 0; y < _gridH; y++)
        {
            for (int x = 0; x < _gridW; x++)
            {
                if (_rng.NextDouble() > _loopChance) continue;

                // Try to add one extra connection from (x,y) to a random neighbor
                var dirs = new List<int> { 0, 1, 2, 3 };
                Shuffle(dirs);

                foreach (int d in dirs)
                {
                    Vector2Int n;
                    int bit, opp;

                    switch (d)
                    {
                        case 0: n = new Vector2Int(x, y + 1); bit = DIR_N; opp = DIR_S; break;
                        case 1: n = new Vector2Int(x + 1, y); bit = DIR_E; opp = DIR_W; break;
                        case 2: n = new Vector2Int(x, y - 1); bit = DIR_S; opp = DIR_N; break;
                        default: n = new Vector2Int(x - 1, y); bit = DIR_W; opp = DIR_E; break;
                    }

                    if (!InBounds(n)) continue;

                    if ((_doors[x, y] & bit) == 0)
                    {
                        _doors[x, y] |= bit;
                        _doors[n.x, n.y] |= opp;
                        break;
                    }
                }
            }
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = _rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
