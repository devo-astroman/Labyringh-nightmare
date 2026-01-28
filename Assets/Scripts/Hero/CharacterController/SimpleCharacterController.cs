using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTarget; // assign the same PlayerCameraRoot/CinemachineCameraTarget
    
    [Header("Movement - Run")]
    public float runSpeed = 5f;
    [Header("Movement - Walk")]
    //public KeyCode walkKey = KeyCode.LeftShift;    
    public float walkSpeed = 2f;
    public bool isWalking = false;

    private float _currentSpeed = 0;
    private bool _usingRunSpeed = false;
    

    [Header("Jump")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;
    private bool _shouldMakeJump = false;

    [Header("Crouch")]
    //public KeyCode crouchKey = KeyCode.LeftControl;
    private bool _isCrouch = false;
    public float crouchSpeed = 2f;
    public float standHeight = 1.5f;
    public float standCenter = 0;    
    public float crouchHeight = .7f;
    public float crouchCenter = -.36f;
    public float ceilingCheckRadius = 0.25f;
    public LayerMask ceilingMask = ~0; // everything by default

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    [SerializeField] private CharacterController _controller;
    private float _verticalVelocity;

    [SerializeField] private Transform _playerRootToRotate;

    
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
        if (_controller == null)
            Debug.LogError("CharacterController reference missing", this);

        if (_playerRootToRotate == null && _controller != null)
            _playerRootToRotate = _controller.transform; // rotate the object that is actually moving

        _currentSpeed = runSpeed;
        _usingRunSpeed = true;
    }


    void Update()
    {

        /* bool walkKeyDown = Input.GetKeyDown(walkKey);

        if (walkKeyDown)
        {
            isWalking = !isWalking;
            
        } */

/////
/*         bool crouchKeyDown = Input.GetKeyDown(crouchKey);

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
        } */

        float _x = Input.GetAxis("Horizontal");
        float _z = Input.GetAxis("Vertical");

        Vector3 _move = new Vector3(_x, 0f, _z);

        if (_move.magnitude > 1f)
            _move.Normalize();

        //float speed = (isWalking || _isCrouch) ? walkSpeed : runSpeed;
        float speed = _currentSpeed;

        // Camera forward (flattened)
        Vector3 camForward = cameraTarget != null ? cameraTarget.forward : Vector3.forward;
        Vector3 camRight   = cameraTarget != null ? cameraTarget.right   : Vector3.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Move direction relative to camera
        Vector3 moveDir = camForward * _z + camRight * _x;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        // Player always faces camera yaw (forward)
        if (cameraTarget != null)
        {
            Vector3 faceDir = camForward; // already flattened
            if (faceDir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(faceDir);
                _playerRootToRotate.rotation = Quaternion.Slerp(
                    _playerRootToRotate.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );

            }
        }

        // Move in the intended direction (not necessarily transform.forward)
        Vector3 worldMove = moveDir * speed;



        // Ground check and is not crouching
        /*         if (_controller.isGrounded && !_isCrouch)
                {
                    if (_verticalVelocity < 0)
                        _verticalVelocity = -2f; // keeps grounded

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        // Jump velocity formula
                        _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    }
                } */

        if (_shouldMakeJump)
        {
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _shouldMakeJump = false;
        }

        // Apply gravity
        _verticalVelocity += gravity * Time.deltaTime;

        // Combine horizontal + vertical
        Vector3 _velocity = worldMove;
        _velocity.y = _verticalVelocity;
        _controller.Move(_velocity * Time.deltaTime);
    }

    public bool IsCeilingBlocked()
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

    public void ApplyRun()
    {
        _currentSpeed = runSpeed;

        RemoveCrouchSetup();
        _isCrouch = false;
        _usingRunSpeed = true;
    }

    public void ApplyWalk()
    {
        _currentSpeed = walkSpeed;

        RemoveCrouchSetup();
        _isCrouch = false;
        _usingRunSpeed = false;
    }

    public bool GetUsingRunSpeed()
    {
        return _usingRunSpeed;
    }

    public void ApplyCrouch()
    {
        _currentSpeed = crouchSpeed;
        
        AddCrouchSetup();
        _isCrouch = true;
    }

    public void ApplyJump()
    {
        _shouldMakeJump = true && _controller.isGrounded;
        
    }

    private void AddCrouchSetup()
    {
        _controller.height = crouchHeight;
        float centerY = crouchCenter;
        _controller.center = new Vector3(_controller.center.x, centerY, _controller.center.z);
    }

    private void RemoveCrouchSetup()
    {
        _controller.height = standHeight;
        float centerY = standCenter;
        _controller.center = new Vector3(_controller.center.x, centerY, _controller.center.z);
    }



}
