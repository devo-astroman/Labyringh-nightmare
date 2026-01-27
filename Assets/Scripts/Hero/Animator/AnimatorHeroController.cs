using UnityEngine;

public class AnimatorHeroController : MonoBehaviour
{
    [SerializeField] private LayerHeroAnimatorController _layerHeroAnimatorController;
    [SerializeField] private Animator _animator;

    
    private string _runModeParamName = "RunMode";
    private string _walkModeParamName = "WalkMode";
    private string _crouchModeParamName = "CrouchMode";
    

    public void SetBaseMode()
    {
        SetBaseModeTrue();
        _layerHeroAnimatorController.SetBase();
    }

    public void SetRunMode()
    {
        SetModeTrue(_runModeParamName);
        _layerHeroAnimatorController.SetRun();
    }

    public void SetWalkMode()
    {
        SetModeTrue(_walkModeParamName);
        _layerHeroAnimatorController.SetWalk();
    }

    public void SetCrouchMode()
    {
        SetModeTrue(_crouchModeParamName);
        _layerHeroAnimatorController.SetCrouch();
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
    }
}
