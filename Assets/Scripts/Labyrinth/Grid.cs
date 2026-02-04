using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GridUnit _gridUnit; // prefab (recommended)

    private GridUnit[][] _grid;

    [SerializeField] private Transform originGrid;

    void Start()
    {
        //GenerateGrid(20,20,originGrid.position);
    }    

    public void GenerateGrid(int widthGrid, int heightGrid, Vector3 position)
    {
        if (_gridUnit == null)
        {
            Debug.LogError("Grid: _gridUnit prefab is missing.");
            return;
        }

        if (widthGrid <= 0 || heightGrid <= 0)
        {
            Debug.LogError("Grid: widthGrid and heightGrid must be > 0.");
            return;
        }

        _grid = new GridUnit[widthGrid][];
        Vector3 origin = position;

        // 1) Create the first tile to measure real runtime size
        _grid[0] = new GridUnit[heightGrid];
        GridUnit first = Instantiate(_gridUnit, origin, Quaternion.identity, transform);
        first.name = "GridUnit_0_0";
        _grid[0][0] = first;

        Vector2 dimensions = first.GetDimensions();   // IMPORTANT: measured from instance
        float stepX = dimensions.x;
        float stepZ = dimensions.y;

        // 2) Fill the rest
        for (int x = 0; x < widthGrid; x++)
        {
            if (_grid[x] == null)
                _grid[x] = new GridUnit[heightGrid];

            for (int y = 0; y < heightGrid; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector3 spawnPos = origin + new Vector3(-x * stepX, 0f,-y * stepZ);

                GridUnit instance = Instantiate(_gridUnit, spawnPos, Quaternion.identity, transform);
                instance.name = $"GridUnit_{x}_{y}";
                _grid[x][y] = instance;
            }
        }
    }


    public void PlaceObjectAt(GameObject objectToPlace, int x, int y)
    {
        Vector3 position = _grid[x][y].GetCenter();
        objectToPlace.transform.position = position;
    }

    public Vector3 GetPositionOfRoom(int x, int y)
    {
        return _grid[x][y].GetCenter();
    }

}

