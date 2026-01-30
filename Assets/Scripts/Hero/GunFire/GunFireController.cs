using UnityEngine;

public class GunFireController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _fireOrigin;   // Muzzle / Empty
    [SerializeField] private Camera _aimCamera;

    [Header("Fire Settings")]
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private LayerMask _hitMask;

    [Header("Debug")]
    [SerializeField] private bool _drawDebugRay = true;

    // ----------------------------------------------------
    // PUBLIC API
    // ----------------------------------------------------

    public void Fire()
    {
        if (_fireOrigin == null || _aimCamera == null)
        {
            Debug.LogWarning("GunFireController: Missing references");
            return;
        }

        // Ray from camera center
        Ray cameraRay = _aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        // Check what camera is aiming at
        if (Physics.Raycast(cameraRay, out RaycastHit hit, _maxDistance, _hitMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            // Nothing hit → shoot forward into space
            targetPoint = cameraRay.origin + cameraRay.direction * _maxDistance;
        }

        // Direction from muzzle to target
        Vector3 fireDirection = (targetPoint - _fireOrigin.position).normalized;

        // Shoot ray from muzzle
        if (Physics.Raycast(_fireOrigin.position, fireDirection, out RaycastHit fireHit, _maxDistance, _hitMask))
        {
            OnHit(fireHit);
        }

        // Debug visualization
        if (_drawDebugRay)
        {
            Debug.DrawRay(_fireOrigin.position, fireDirection * _maxDistance, Color.red, 0.5f);
        }
    }

    // ----------------------------------------------------
    // HIT HANDLING
    // ----------------------------------------------------

    private void OnHit(RaycastHit hit)
    {
        Debug.Log($"Hit: {hit.collider.name}");

        // Example:
        // hit.collider.GetComponent<IDamageable>()?.TakeDamage(10);
    }
}
