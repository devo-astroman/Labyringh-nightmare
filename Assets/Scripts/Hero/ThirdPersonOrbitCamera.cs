using UnityEngine;

public class ThirdPersonOrbitCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _player; // the character root (capsule)

    [Header("Mouse Look")]
    [SerializeField] private float _sensitivity = 2f;
    [SerializeField] private float _topClamp = 70f;
    [SerializeField] private float _bottomClamp = -30f;

    private float _yaw;
    private float _pitch;

    private void Start()
    {
        if (_player != null)
            _yaw = _player.eulerAngles.y;

        Vector3 e = transform.localEulerAngles;
        _pitch = e.x;
    }

    private void LateUpdate()
    {
        // Mouse input
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        _yaw += mx * _sensitivity;
        _pitch -= my * _sensitivity;
        _pitch = Mathf.Clamp(_pitch, _bottomClamp, _topClamp);

        // Orbit target rotation (this is what Cinemachine follows)
        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        // Keep camera target anchored to player position
        if (_player != null)
            transform.position = _player.position;
    }

    public float GetYaw() => _yaw;
}
