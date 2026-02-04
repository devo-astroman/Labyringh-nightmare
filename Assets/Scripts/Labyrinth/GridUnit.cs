using UnityEngine;

public class GridUnit : MonoBehaviour
{
    [SerializeField] private GameObject  _floor;
    
    public Vector2 GetDimensions()
    {
        if (_floor == null)
        {
            Debug.LogError("GridUnit: Floor reference missing");
            return Vector2.zero;
        }

        Renderer r = _floor.GetComponent<Renderer>();

        if (r == null)
        {
            Debug.LogError("GridUnit: No Renderer found on floor object");
            return Vector2.zero;
        }

        Bounds b = r.bounds;

        // X = width, Z = depth (Unity ground plane convention)
        return new Vector2(b.size.x, b.size.z);
    }


}
