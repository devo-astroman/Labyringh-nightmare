using UnityEngine;


public class GroundDistanceChecker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _checkDistance = 1.5f;
    [SerializeField] private float _nearFloorThreshold = 0.35f;
    [SerializeField] private LayerMask _groundMask = ~0;

    [Header("Debug")]
    [SerializeField] private bool _drawDebugRay = true;

    [SerializeField] private CharacterController _controller;

    public bool NearFloor { get; private set; }
    public float CurrentDistance { get; private set; }

    private void Awake()
    {
    }

    private void Update()
    {
        CheckGroundDistance();
    }

    private void CheckGroundDistance()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        Ray ray = new Ray(origin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, _checkDistance, _groundMask, QueryTriggerInteraction.Ignore))
        {
            CurrentDistance = hit.distance;
            NearFloor = CurrentDistance < _nearFloorThreshold;
        }
        else
        {
            // No ground detected (falling into void)
            CurrentDistance = Mathf.Infinity;
            NearFloor = false;

        }

        if (_drawDebugRay)
        {
            Color color = NearFloor ? Color.green : Color.red;
            Debug.DrawRay(origin, Vector3.down * _checkDistance, color);
        }
    }
}
