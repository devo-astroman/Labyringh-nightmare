using System;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthCreator : MonoBehaviour
{
    [Header("Labyrinth Size - NxN - Same width and height")]
    [SerializeField] private float _nSize = 20;

    [SerializeField] private Transform _positionToPlaceTheLabyrinth;

    [Header("Room Factory")]
    [SerializeField] private RoomFactory _roomFactory;

    [Header("Grid")]
    [SerializeField] private float _cellSize = 1f; // 1 grid cell = 1 world unit by default

    [Header("Maze Settings")]
    [SerializeField] private int _seed = 1234;
    [SerializeField] private bool _addExtraLoops = false;
    [SerializeField, Range(0f, 0.3f)] private float _loopChance = 0.08f;

    // Now it's truly NxN with coords: x,y in [0..N-1]
    // Start: [0..N-1], 0
    // End:   [0..N-1], N-1
    private int _N;
    private int _W;
    private int _H;

    private System.Random _rng;

    // doors[x,y] bitmask for 1x1: N=1, E=2, S=4, W=8
    private int[,] _doors;

    private readonly List<GameObject> _spawned = new();

    /// <summary>
    /// Generates a 1x1-only labyrinth (NxN). Returns an array of two GameObjects:
    /// [0] = start room, [1] = end room
    /// </summary>
    public GameObject[] GenerateLabyrinth()
    {
        if (_roomFactory == null)
        {
            Debug.LogError("LabyrinthCreator: RoomFactory is missing.");
            return Array.Empty<GameObject>();
        }

        _N = Mathf.Max(2, Mathf.RoundToInt(_nSize)); // at least 2x2
        _W = _N;
        _H = _N;

        _rng = new System.Random(_seed);

        ClearPrevious();

        _doors = new int[_W, _H];

        // Pick random start x on bottom row (y=0), and random end x on top row (y=N-1)
        Vector2Int start = new Vector2Int(_rng.Next(0, _W), 0);
        Vector2Int end = new Vector2Int(_rng.Next(0, _W), _H - 1);

        // Build a perfect maze (DFS backtracker), guaranteeing connectivity between any two cells
        BuildPerfectMazeDFS();

        // Optionally add loops (makes it less "perfect maze" and more labyrinth)
        if (_addExtraLoops)
            AddRandomLoops();

        // Instantiate all 1x1 rooms and open entrances according to door bitmask
        GameObject startGO = null;
        GameObject endGO = null;

        for (int y = 0; y < _H; y++)
        {
            for (int x = 0; x < _W; x++)
            {
                var c = new Vector2Int(x, y);
                GameObject go = InstantiateCell(c);

                if (c == start) startGO = go;
                if (c == end) endGO = go;
            }
        }

        return new[] { startGO, endGO };
    }

    // -------------------------
    // Maze generation (perfect maze)
    // -------------------------

    private void BuildPerfectMazeDFS()
    {
        bool[,] visited = new bool[_W, _H];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        Vector2Int current = new Vector2Int(_rng.Next(0, _W), _rng.Next(0, _H));
        visited[current.x, current.y] = true;
        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Peek();
            var neighbors = GetUnvisitedNeighbors(current, visited);

            if (neighbors.Count == 0)
            {
                stack.Pop();
                continue;
            }

            var pick = neighbors[_rng.Next(neighbors.Count)];

            // Carve passage both ways
            _doors[current.x, current.y] |= pick.dirBit;
            _doors[pick.n.x, pick.n.y] |= pick.oppositeBit;

            visited[pick.n.x, pick.n.y] = true;
            stack.Push(pick.n);
        }
    }

    private List<(Vector2Int n, int dirBit, int oppositeBit)> GetUnvisitedNeighbors(Vector2Int c, bool[,] visited)
    {
        var list = new List<(Vector2Int, int, int)>(4);

        Vector2Int n;

        // N
        n = new Vector2Int(c.x, c.y + 1);
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

    // -------------------------
    // Optional loops
    // -------------------------

    private void AddRandomLoops()
    {
        for (int y = 0; y < _H; y++)
        {
            for (int x = 0; x < _W; x++)
            {
                if (_rng.NextDouble() > _loopChance) continue;

                var dirs = new List<int>(4) { 0, 1, 2, 3 };
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

                    // Carve it if not already carved
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

    // -------------------------
    // Instantiate rooms + apply entrances
    // -------------------------

    private GameObject InstantiateCell(Vector2Int cell)
    {
        Vector3 pos = GridToWorld(cell);
        GameObject go = _roomFactory.GetRoom1x1(pos);
        go.name = $"Room1x1_{cell.x}_{cell.y}";
        _spawned.Add(go);

        Room room = go.GetComponent<Room>();
        if (room != null)
        {
            room.CloseAllEntrances();

            int mask = _doors[cell.x, cell.y];

            // For 1x1 rooms, the entrance index per side is always 0
            if ((mask & DIR_N) != 0) room.OpenEntrances(RoomSides.NORTH_SIDE, new[] { 0 });
            if ((mask & DIR_E) != 0) room.OpenEntrances(RoomSides.EAST_SIDE,  new[] { 0 });
            if ((mask & DIR_S) != 0) room.OpenEntrances(RoomSides.SOUTH_SIDE, new[] { 0 });
            if ((mask & DIR_W) != 0) room.OpenEntrances(RoomSides.WEST_SIDE,  new[] { 0 });
        }

        return go;
    }

    private Vector3 GridToWorld(Vector2Int grid)
    {
        Vector3 origin = _positionToPlaceTheLabyrinth != null ? _positionToPlaceTheLabyrinth.position : Vector3.zero;
        return origin + new Vector3(grid.x * _cellSize, 0f, grid.y * _cellSize);
    }

    private bool InBounds(Vector2Int c) => c.x >= 0 && c.y >= 0 && c.x < _W && c.y < _H;

    private void ClearPrevious()
    {
        for (int i = 0; i < _spawned.Count; i++)
        {
            if (_spawned[i] == null) continue;
#if UNITY_EDITOR
            if (!Application.isPlaying) DestroyImmediate(_spawned[i]);
            else Destroy(_spawned[i]);
#else
            Destroy(_spawned[i]);
#endif
        }
        _spawned.Clear();
    }

    // -------------------------
    // Door bit constants for 1x1
    // -------------------------
    private const int DIR_N = 1 << 0;
    private const int DIR_E = 1 << 1;
    private const int DIR_S = 1 << 2;
    private const int DIR_W = 1 << 3;
}
