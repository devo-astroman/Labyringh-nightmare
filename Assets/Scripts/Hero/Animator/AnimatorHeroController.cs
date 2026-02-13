using UnityEngine;

public class AnimatorHeroController : MonoBehaviour
{
    [SerializeField] private LayerHeroAnimatorController _layerHeroAnimatorController;
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _pistol;
    
    private string _runModeParamName = "RunMode";
    private string _walkModeParamName = "WalkMode";
    private string _crouchModeParamName = "CrouchMode";
    private string _aimModeParamName = "AimMode";
    

    public void SetBaseMode()
    {
        SetBaseModeTrue();
        _layerHeroAnimatorController.SetBase();
    }

    public void SetRunMode()
    {
        _pistol.SetActive(false);
        SetModeTrue(_runModeParamName);
        _layerHeroAnimatorController.SetRun();
    }

    public void SetWalkMode()
    {
        _pistol.SetActive(false);
        SetModeTrue(_walkModeParamName);
        _layerHeroAnimatorController.SetWalk();
    }

    public void SetCrouchMode()
    {
        _pistol.SetActive(false);
        SetModeTrue(_crouchModeParamName);
        _layerHeroAnimatorController.SetCrouch();
    }

    public void SetAimMode()
    {
        _pistol.SetActive(true);
        SetModeTrue(_aimModeParamName);
        _layerHeroAnimatorController.SetAim();
    }

    private void SetModeTrue(string paramName)
    {
        SetBaseModeTrue();
        _animator.SetBool(paramName,true);
    }
    private void SetBaseModeTrue()
    {
        _animator.SetBool(_runModeParamName,false);
        _animator.SetBool(_walkModeParamName,false);
        _animator.SetBool(_crouchModeParamName,false);
        _animator.SetBool(_aimModeParamName,false);
    }
}
