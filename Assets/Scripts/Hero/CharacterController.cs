using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviour
{
    [Header("Movement - Run")]
    public float runSpeed = 5f;
    [Header("Movement - Walk")]
    public KeyCode walkKey = KeyCode.LeftShift;    
    public float walkSpeed = 2f;
    public bool isWalking = false;

    [Header("Jump")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Crouch")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    private bool _isCrouch = false;
    public float standHeight = 1.5f;
    public float standCenter = 0;    
    public float crouchHeight = .7f;
    public float crouchCenter = -.36f;
    public float ceilingCheckRadius = 0.25f;
    public LayerMask ceilingMask = ~0; // everything by default

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    private CharacterController _controller;
    private float _verticalVelocity;

    
    public Vector3 GetVelocity() => _controller != null ? _controller.velocity : Vector3.zero;

    public float GetHorizontalSpeed()
    {
        Vector3 v = GetVelocity();
        v.y = 0f;
        return v.magnitude;
    }

    public float GetVerticalSpeed() => GetVelocity().y;
    public bool IsGrounded() => _controller != null && _controller.isGrounded;
    public bool IsCrouching() => _isCrouch;



    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        bool walkKeyDown = Input.GetKeyDown(walkKey);

        if (walkKeyDown)
        {
            isWalking = !isWalking;
            
        }

/////
        bool crouchKeyDown = Input.GetKeyDown(crouchKey);

        if (crouchKeyDown)
        {
            _isCrouch = !_isCrouch;

            if (!_isCrouch && IsCeilingBlocked())
            {
                // try to stand up but there is a blocking ceiling
                _isCrouch = true;
            }

            _controller.height = _isCrouch ? crouchHeight : standHeight;
            float centerY = _isCrouch ? crouchCenter : standCenter;

            _controller.center = new Vector3(_controller.center.x, centerY, _controller.center.z);
        }

        float _x = Input.GetAxis("Horizontal");
        float _z = Input.GetAxis("Vertical");

        Vector3 _move = new Vector3(_x, 0f, _z);

        if (_move.magnitude > 1f)
            _move.Normalize();

        if (_move != Vector3.zero)
        {
            Quaternion _targetRotation = Quaternion.LookRotation(_move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                _targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        float speed = isWalking || _isCrouch ? walkSpeed:runSpeed;
        // Move forward in facing direction
        Vector3 _worldMove = transform.forward * _move.magnitude * speed;
        

        // Ground check and is not crouching
        if (_controller.isGrounded && !_isCrouch)
        {
            if (_verticalVelocity < 0)
                _verticalVelocity = -2f; // keeps grounded

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Jump velocity formula
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Apply gravity
        _verticalVelocity += gravity * Time.deltaTime;

        // Combine horizontal + vertical
        Vector3 _velocity = _worldMove;
        _velocity.y = _verticalVelocity;

        _controller.Move(_velocity * Time.deltaTime);
    }

    private bool IsCeilingBlocked()
    {
        // Check point just above the character's head (based on stand height)
        Vector3 _origin = transform.position + Vector3.up * (standHeight - 0.05f);

        return Physics.CheckSphere(
            _origin,
            ceilingCheckRadius,
            ceilingMask,
            QueryTriggerInteraction.Ignore
        );
    }
}
