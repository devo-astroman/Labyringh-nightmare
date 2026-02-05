using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshMazeBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _surface;

    public void Rebuild()
    {
        if (_surface == null) return;
        _surface.BuildNavMesh();
    }
}
