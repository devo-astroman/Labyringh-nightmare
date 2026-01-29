using UnityEngine;

public class CameraAimToMaskBridge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ThirdPersonOrbitCamera _cameraOrbit;
    [SerializeField] private LayerMaskAnimatorController _maskController;

    [Header("Settings")]
    [SerializeField] private float _deadZone = 0.05f; // avoid jitter
    [SerializeField] private float _maxWeight = 1f;

    private void Update()
    {
        if (_cameraOrbit == null || _maskController == null)
            return;

        float pitch = _cameraOrbit.GetPitchNormalized();

        // Dead zone to avoid micro movement
        if (Mathf.Abs(pitch) < _deadZone)
        {
            _maskController.ClearMasksSmooth();
            return;
        }

        // Looking UP
        if (pitch > 0f)
        {
            float weight = Mathf.Clamp01(pitch) * _maxWeight;
            _maskController.SetMaskASmooth(weight);   // PointUp
            _maskController.SetMaskBSmooth(0f);       // Clear Down
        }
        // Looking DOWN
        else
        {
            float weight = Mathf.Clamp01(-pitch) * _maxWeight;
            _maskController.SetMaskBSmooth(weight);   // PointDown
            _maskController.SetMaskASmooth(0f);       // Clear Up
        }
    }
}
