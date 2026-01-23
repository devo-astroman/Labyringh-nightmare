using UnityEngine;

[RequireComponent(typeof(SimpleCharacterController))]
public class CharacterAnimatorController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Animator _animator;

    private SimpleCharacterController _simpleCharacterController;

    [Header("Animator Params")]
    [SerializeField] private string _speedParam = "Speed";
    [SerializeField] private string _isMovingParam = "IsMoving";
    [SerializeField] private string _isRunningParam = "IsRunning";
    [SerializeField] private string _isJumpingParam = "Jump";       // recommend Trigger
    [SerializeField] private string _isCrouchingParam = "Crouch";   // bool
    [SerializeField] private string _isFallingParam = "FreeFall";   // bool

    [Header("Tuning")]
    [SerializeField] private float _idleThreshold = 0.05f;
    [SerializeField] private float _runThreshold = 4.0f;
    [SerializeField] private float _speedDamp = 8f;

    [Tooltip("Vertical speed below which we consider 'falling' (negative = going down)")]
    [SerializeField] private float _fallThreshold = -0.2f;

    private float _smoothedSpeed;

    // State tracking to detect jump start
    private bool _wasGrounded;

    void Start()
    {
        _simpleCharacterController = GetComponent<SimpleCharacterController>();

        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();

        _wasGrounded = _simpleCharacterController.IsGrounded();
    }

    void Update()
    {
        if (_animator == null || _simpleCharacterController == null) return;

        // --- Speed / locomotion ---
        float speed = _simpleCharacterController.GetHorizontalSpeed();
        _smoothedSpeed = Mathf.Lerp(_smoothedSpeed, speed, Time.deltaTime * _speedDamp);

        _animator.SetFloat(_speedParam, _smoothedSpeed);

        bool isMoving = _smoothedSpeed > _idleThreshold;
        bool isRunning = _smoothedSpeed >= _runThreshold;

        if (!string.IsNullOrEmpty(_isMovingParam))
            _animator.SetBool(_isMovingParam, isMoving);

        if (!string.IsNullOrEmpty(_isRunningParam))
            _animator.SetBool(_isRunningParam, isRunning);

        // --- Crouch ---
        bool isCrouching = _simpleCharacterController.IsCrouching();
        if (!string.IsNullOrEmpty(_isCrouchingParam))
            _animator.SetBool(_isCrouchingParam, isCrouching);

        // --- Jump / Fall ---
        bool grounded = _simpleCharacterController.IsGrounded();
        float vY = _simpleCharacterController.GetVerticalSpeed();

        // Jump start: was grounded last frame, now not grounded, and moving upward
        bool jumpStarted = _wasGrounded && !grounded && vY > 0.1f;

        // Falling: airborne and going downward
        bool isFalling = !grounded && vY < _fallThreshold;

        // Jump parameter: recommend Trigger
        if (jumpStarted && !string.IsNullOrEmpty(_isJumpingParam))
        {
            _animator.SetTrigger(_isJumpingParam);
        }

        if (!string.IsNullOrEmpty(_isFallingParam))
            _animator.SetBool(_isFallingParam, isFalling);

        _wasGrounded = grounded;
    }
}
