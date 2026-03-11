using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthCreator : MonoBehaviour
{
    [SerializeField] private int _gridW = 20;
    [SerializeField] private int _gridH = 20;

    [SerializeField] private Transform _gridPosition;

    [SerializeField] private Grid _grid;

    [SerializeField] private GameObject _roomGOPrefab;
    [SerializeField] private GameObject _exitPrefab;

    [SerializeField] private int _seed = 1234;
    [SerializeField] private bool _addExtraLoops = false;
    [SerializeField, Range(0f, 0.3f)] private float _loopChance = 0.08f;

    [SerializeField] private GameObject _roomsParentGO;

    [SerializeField] private NavMeshMazeBaker _navMeshMazeBaker;

    [SerializeField] private LabyrinthDebugger _labyrinthDebugger;
    [SerializeField] private LabyrinthObjectsManager _labyrinthObjectsManager;

    private ExitFence _exitFence;

    
    public Action<int> PuzzleSolved;
    

    // doors[x,y] bitmask: N=1, E=2, S=4, W=8
    private int[,] _doors;
    private System.Random _rng;

    private const int DIR_N = 1 << 0;
    private const int DIR_E = 1 << 1;
    private const int DIR_S = 1 << 2;
    private const int DIR_W = 1 << 3;

    private Coroutine _generateRoutine;

    void Start()
    {
        _labyrinthObjectsManager.PuzzleSolvedAction += HandlePuzzleSolved;
    }

    public void GenerateLabyrinth()
    {
        if (_generateRoutine != null)
            StopCoroutine(_generateRoutine);

        _generateRoutine = StartCoroutine(GenerateLabyrinthRoutine());
    }

    void OnDestroy()
    {
        _labyrinthObjectsManager.PuzzleSolvedAction -= HandlePuzzleSolved;
    }

    private IEnumerator GenerateLabyrinthRoutine()
    {
        if (_grid == null)
        {
            Debug.LogError("LabyrinthCreator: Grid reference missing.");
            yield break;
        }

        if (_roomGOPrefab == null)
        {
            Debug.LogError("LabyrinthCreator: Room prefab missing.");
            yield break;
        }

        if (_gridW <= 0 || _gridH <= 0)
        {
            Debug.LogError("LabyrinthCreator: Invalid grid size.");
            yield break;
        }

        if (_gridPosition == null)
        {
            Debug.LogError("LabyrinthCreator: Grid position missing.");
            yield break;
        }

        if (_roomsParentGO == null)
        {
            Debug.LogError("LabyrinthCreator: RoomsParentGO missing.");
            yield break;
        }

        // Optional: clear old rooms under parent
        for (int i = _roomsParentGO.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_roomsParentGO.transform.GetChild(i).gameObject);
        }

        _rng = new System.Random(_seed);

        _grid.GenerateGrid(_gridW, _gridH, _gridPosition.position);

        // Wait a frame so transforms/colliders settle before placing rooms/navmesh
        yield return null;

        CreateLabyrinth();

        // Wait a frame so newly instantiated rooms/doors are fully registered
        yield return null;

        if (_navMeshMazeBaker != null)
            _navMeshMazeBaker.Rebuild();

        _generateRoutine = null;
    }

    public void GenerateLabyrinthTest()
    {
        if (_generateRoutine != null)
            StopCoroutine(_generateRoutine);

        _generateRoutine = StartCoroutine(GenerateLabyrinthRoutineTest());
    }

    public void ActivatePuzzle(int idPuzzle)
    {
        _labyrinthObjectsManager.ActivatePuzzle(idPuzzle);
    }

    public void ShowDebug(Vector3[] positions)
    {
        _labyrinthDebugger.ShowRoomMarksAt(positions);
    }

    private IEnumerator GenerateLabyrinthRoutineTest()
    {



        // Wait a frame so newly instantiated rooms/doors are fully registered
        yield return null;

        if (_navMeshMazeBaker != null)
            _navMeshMazeBaker.Rebuild();

        _generateRoutine = null;
    }

    public Vector3[] GetStartAndEndPositions()
    {
        Vector3 startRoomPosition = _grid.GetPositionOfRoom(0, 0);
        Vector3 endRoomPosition = _grid.GetPositionOfRoom(_gridW - 1, _gridH - 1);
        return new Vector3[] { startRoomPosition, endRoomPosition };
    }

    public Vector3 GetRoomPosition(int x, int y)
    {
        return _grid.GetPositionOfRoom(x, y);
    }

    public void OpenExitFence()
    {
        _exitFence.OpenFence();
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

        // 2) Optional loops
        if (_addExtraLoops)
            AddRandomLoops();

        // 3) Place a room prefab at every grid cell and open entrances based on door mask
        Transform parent = _roomsParentGO.transform;

        for (int y = 0; y < _gridH; y++)
        {
            for (int x = 0; x < _gridW; x++)
            {
                if(x == 9 && y == 4)
                {
                    GameObject exitGO = Instantiate(_exitPrefab, parent);
                    exitGO.name = $"Exit_{x}_{y}";
                    _grid.PlaceObjectAt(exitGO, x, y);

                    _exitFence = exitGO.GetComponent<ExitFence>();
                }
                else
                {
                    GameObject roomGO = Instantiate(_roomGOPrefab, parent);
                    roomGO.name = $"Room_{x}_{y}";

                    Room room = roomGO.GetComponent<Room>();
                    int mask = 0;
                    if (room != null)
                    {
                        room.CloseAllEntrances();

                        mask = _doors[x, y];

                        if ((mask & DIR_N) != 0) room.OpenEntrances(RoomSides.NORTH_SIDE, new[] { 0 });
                        if ((mask & DIR_E) != 0) room.OpenEntrances(RoomSides.EAST_SIDE, new[] { 0 });
                        if ((mask & DIR_S) != 0) room.OpenEntrances(RoomSides.SOUTH_SIDE, new[] { 0 });
                        if ((mask & DIR_W) != 0) room.OpenEntrances(RoomSides.WEST_SIDE, new[] { 0 });
                    }

                    _grid.PlaceObjectAt(roomGO, x, y);
                    ApplyFunction(roomGO, x, y, mask);
                }


                
            }
        }
    }

    private void ApplyFunction(GameObject roomGO, int x, int y, int mask)
    {

        _labyrinthObjectsManager.ProcessRoom(roomGO,x,y,mask);

        // Example: store coords on a component
/*         var room = roomGO.GetComponent<Room>();
        if (room != null)
        {
            room.GridX = x;
            room.GridY = y;
            room.DoorMask = mask;
        }

        // Example: spawn something depending on mask, distance, etc.
         */

        _labyrinthObjectsManager.ProcessRoom(roomGO, x, y, mask);


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

    private void HandlePuzzleSolved(int id)
    {
        PuzzleSolved?.Invoke(id);
    }
}
