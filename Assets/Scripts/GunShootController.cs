using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GunShootController : MonoBehaviour
{
    [Header("Animator")]
    [Tooltip("Animator of the character")]
    public Animator _animator;

    //[SerializeField] int aimLayerIndex = 1; // your AimLayer index

    [SerializeField] ThirdPersonController _thirdPersonController; // your AimLayer index
    [SerializeField] AimController _aimController; // your AimLayer index

    
    

    private bool _isAiming = false;

    void Awake()
    {
        //_animator.SetLayerWeight(aimLayerIndex, 0f);
        _animator.SetBool("IsAiming", false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isAiming = !_isAiming;

            _animator.SetBool("IsAiming", _isAiming);
            //_animator.SetLayerWeight(aimLayerIndex, _isAiming?1f:0f);

            _thirdPersonController.LockCameraPosition  = _isAiming;
            _thirdPersonController.enabled = !_isAiming;
            _aimController.enabled = _isAiming;
        }

    }

}
