using UnityEngine;
using System;

public class AnimatorHeroController : MonoBehaviour
{
    [SerializeField] private LayerHeroAnimatorController _layerHeroAnimatorController;
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _pistol;
    
    private string _runModeParamName = "RunMode";
    private string _walkModeParamName = "WalkMode";
    private string _crouchModeParamName = "CrouchMode";
    private string _aimModeParamName = "AimMode";
    private string _deadModeParamName = "DeadMode";

    private bool _checkEndOfDeadAnimation = false;
    public Action DeadAnimationFinishedAction;

    void Update()
    {
        if(_checkEndOfDeadAnimation && DeadAnimationFinishedAction != null && AnimationFinished.IsAnimationFinished(_animator, "Die",8))
        {
            DeadAnimationFinishedAction.Invoke();
            _checkEndOfDeadAnimation = false;   
        }
    }
    
    public void CheckEndOfDeadAnimation()
    {
        _checkEndOfDeadAnimation = true;
    }

    public void UncheckEndOfDeadAnimation()
    {
        _checkEndOfDeadAnimation = false;
    }

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

    public void SetDeadMode()
    {
        SetModeTrue(_deadModeParamName);
        _layerHeroAnimatorController.SetDie();
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
        _animator.SetBool(_deadModeParamName,false);
    }
}
