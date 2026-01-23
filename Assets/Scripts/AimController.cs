using UnityEngine;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    public class AimController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Transform cameraTarget; // Assign PlayerCameraRoot / CinemachineCameraTarget here

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private float rotationSmoothTime = 0.05f;

        [Header("Aim Settings")]
        [SerializeField] private float aimSensitivity = 200f;

        private CharacterController _controller;
        private StarterAssetsInputs _input;

        private float _rotationVelocity;
        private float _aimYaw;

        private int _animIDSpeed;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _animIDSpeed = Animator.StringToHash("Speed");
        }

        private void OnEnable()
        {
            // Start aiming from current facing direction
            _aimYaw = transform.eulerAngles.y;

            if (animator != null)
                animator.SetBool("IsAiming", true);
        }

        private void OnDisable()
        {
            if (animator != null)
                animator.SetBool("IsAiming", false);
        }

        private void Update()
        {
            HandleRotation();
            HandleMovement();
        }

        private void HandleRotation()
        {
            // Accumulate yaw using look input (NOT camera transform)
            _aimYaw += _input.look.x * aimSensitivity * Time.deltaTime;

            float smoothYaw = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                _aimYaw,
                ref _rotationVelocity,
                rotationSmoothTime
            );

            // Rotate player
            transform.rotation = Quaternion.Euler(0f, smoothYaw, 0f);

            // Rotate camera follow target (so Cinemachine follows aim direction)
            if (cameraTarget != null)
            {
                cameraTarget.rotation = Quaternion.Euler(0f, _aimYaw, 0f);
            }
        }

        private void HandleMovement()
        {
            // Strafing movement relative to aim direction
            Vector3 inputMove = new Vector3(_input.move.x, 0f, _input.move.y);

            Vector3 moveDir = Quaternion.Euler(0f, _aimYaw, 0f) * inputMove;

            _controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            // Animator update
            if (animator != null)
            {
                float speed01 = Mathf.Clamp01(inputMove.magnitude);
                animator.SetFloat(_animIDSpeed, speed01);
            }
        }
    }
}
