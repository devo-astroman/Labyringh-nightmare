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

    private int _ammo = 5;


    public Action<Vector3,Vector3,RaycastHit> onFireAction;

    // ----------------------------------------------------
    // PUBLIC API
    // ----------------------------------------------------

    public void Fire()
    {
        if (_fireOrigin == null || _aimCamera == null || _ammo == 0) return;

        Vector2 offset = UnityEngine.Random.insideUnitCircle * _spreadRadiusViewport;
        Ray cameraRay = _aimCamera.ViewportPointToRay(new Vector3(0.5f + offset.x, 0.5f + offset.y, 0f));

        Vector3 targetPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit camHit, _maxDistance, _hitMask, QueryTriggerInteraction.Ignore))
            targetPoint = camHit.point;
        else
            targetPoint = cameraRay.origin + cameraRay.direction * _maxDistance;

        Vector3 fireDir = (targetPoint - _fireOrigin.position).normalized;

        // spherecast makes close-range much more reliable
        const float bulletRadius = 0.05f;
        if (Physics.SphereCast(_fireOrigin.position, bulletRadius, fireDir, out RaycastHit hit,
                            _maxDistance, _hitMask, QueryTriggerInteraction.Ignore))
        {
            OnHit(hit);
        }

        if (_drawDebugRay)
        {
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * _maxDistance, Color.green, 0.5f);
            Debug.DrawRay(_fireOrigin.position, fireDir * _maxDistance, Color.red, 0.5f);
        }
    }

    public void IncreaseAmmo(int many)
    {
        _ammo += many;
    }

    public int GetAmmo()
    {
        return _ammo;
    }

    public void SetSpread(float radiusViewport) => _spreadRadiusViewport = Mathf.Clamp(radiusViewport, 0f, 0.25f);

    public void SetSpreedFromSpeedValue(float speedValue)
    {
        if (speedValue  == 0)
            SetSpreadToMin();

        else if(speedValue < 2.5)
            SetSpreadToMed();

        else
            SetSpreadToMax();
    }

    public void SetSpreadToMin() => _spreadRadiusViewport = Mathf.Clamp(0, 0f, 0.25f);
    public void SetSpreadToMed() => _spreadRadiusViewport = Mathf.Clamp(0.005f, 0f, 0.25f);
    public void SetSpreadToMax() => _spreadRadiusViewport = Mathf.Clamp(0.01f, 0f, 0.25f);



    // ----------------------------------------------------
    // HIT HANDLING
    // ----------------------------------------------------

    private void OnHit(RaycastHit hit)
    {
        Debug.Log("____________");
        Debug.Log("____________");
        Debug.Log("____________");
        Debug.Log("____________");
        Debug.Log($"Hit: {hit.collider.name}");
        Debug.Log("____________");
        Debug.Log("____________");
        Debug.Log("____________");
        Debug.Log("____________");

        // Example:
        // hit.collider.GetComponent<IDamageable>()?.TakeDamage(10);
        _ammo--;

        if(_ammo < 0) _ammo = 0;

        onFireAction?.Invoke(hit.point,hit.normal, hit);
    }
}
