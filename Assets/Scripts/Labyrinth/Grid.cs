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

        // Get size of one grid unit (X = width, Y = depth/Z)
        Vector2 dimensions = _gridUnit.GetDimensions();
        float stepX = dimensions.x;
        float stepZ = dimensions.y;

        _grid = new GridUnit[widthGrid][];

        Vector3 origin = position;

        for (int x = 0; x < widthGrid; x++)
        {
            _grid[x] = new GridUnit[heightGrid];

            for (int y = 0; y < heightGrid; y++)
            {
                Vector3 spawnPos = origin + new Vector3(x * stepX, 0f, y * stepZ);

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

}

