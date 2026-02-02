using UnityEngine;
using System;

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

    [SerializeField, Range(0f, 0.25f)]
    private float _spreadRadiusViewport = 0.01f;


    public Action<Vector3,Vector3,RaycastHit> onFireAction;

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

        // Ray from a random point inside a circle around screen center
        // Spread is expressed in VIEWPORT units:
        // 0.0 = exact center, 0.05 = 5% of screen width/height radius
        Vector2 offset = UnityEngine.Random.insideUnitCircle * _spreadRadiusViewport; // NEW serialized float
        Vector3 viewportPoint = new Vector3(0.5f + offset.x, 0.5f + offset.y, 0f);

        Ray cameraRay = _aimCamera.ViewportPointToRay(viewportPoint);

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

    public void SetSpread(float radiusViewport) => _spreadRadiusViewport = Mathf.Clamp(radiusViewport, 0f, 0.25f);



    // ----------------------------------------------------
    // HIT HANDLING
    // ----------------------------------------------------

    private void OnHit(RaycastHit hit)
    {
        Debug.Log($"Hit: {hit.collider.name}");

        // Example:
        // hit.collider.GetComponent<IDamageable>()?.TakeDamage(10);

        onFireAction?.Invoke(hit.point,hit.normal, hit);
    }
}
