using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovementController : MonoBehaviour
{

	#region Fields
	[SerializeField] private InputHeroController _inputHeroController;
    [SerializeField] private SimpleCharacterController _simpleCharacterController;
    [SerializeField] private AnimatorHeroController _animatorHeroController;
    [SerializeField] private Hud _hud;
    [SerializeField] private GunFireController _gunFireController;
	#endregion

	#region Actions
    public Action CrouchPressedAction;
    public Action RunWalkSwitchPressedAction;

	public Action ChangeToCrouchAction;
    public Action ExitFromCrouchAction;
    
    public Action ChangeToAimAction;
    public Action ExitFromAimAction;
	#endregion



    #region Public Methods    
    public void AllowCrouch()
    {
        _inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
    }
    public void DisableCrouch()
    {
        _inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
    }

    public void AllowJump()
    {
        _inputHeroController.jumpKeyPressed += HandleJumpKeyPressed;
    }

    public void DisableJump()
    {
        _inputHeroController.jumpKeyPressed -= HandleJumpKeyPressed;
    }

    public void AllowFireGun()
    {
        _inputHeroController.fireKeyPressed += HandleFireKeyPressed;
    }
    public void DisableFireGun()
    {
        _inputHeroController.fireKeyPressed -= HandleFireKeyPressed;
    }

    public void AllowChangeToAim()
    {
        _inputHeroController.aimKeyPressed += HandleAimKeyPressed;
    }

    public void DisableChangeToAim()
    {
        _inputHeroController.aimKeyPressed -= HandleAimKeyPressed;
    }

    public void ListenWalkRunSwitchPressed()
    {
        _inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
    }

    public void IgnoreWalkRunSwitchPressed()
    {
        _inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
    }




    #endregion

    #region Execute ModeMovements: Run, Walk, Crouch, Jump

    public void FlipBetweenRunAndWalkSpeed()
    {
        if (_simpleCharacterController.GetUsingRunSpeed())
        {
            ApplyWalkSpeed();
        }
        else
        {
            ApplyRunSpeed();
        }
    }

    public void ApplyRunSpeed()
    {
        _simpleCharacterController.ApplyRun();
    }
    public void ExecuteModeRun()
    {
        _simpleCharacterController.ApplyRun();
        _animatorHeroController.SetRunMode();
    }

    public void ApplyWalkSpeed()
    {
        _simpleCharacterController.ApplyWalk();
    }

    public void ExecuteModeWalk()
    {
        _simpleCharacterController.ApplyWalk();
        _animatorHeroController.SetWalkMode();
    }

    public void ExecuteModeCrouch()
    {
        _animatorHeroController.SetCrouchMode();
        _simpleCharacterController.ApplyCrouch();
    }

    public void ExecuteModeAim()
    {
        _animatorHeroController.SetAimMode();
    }

    public void ExitModeAim()
    {
        _hud.HideCrosshair();
    }
    
    public void RefreshAim()
    {//Called from external script when needed to update the hud, probably from an Update
        float speed = _simpleCharacterController.GetHorizontalSpeed();

        _hud.SetCurrentSpeed(speed);
        _gunFireController.SetSpreedFromSpeedValue(speed);
    }
    #endregion

    #region Private Methods
    private void HandleCrouchKeyPressed()
    {
        Debug.Log("_simpleCharacterController.IsCrouching() " + _simpleCharacterController.IsCrouching());
        if (_simpleCharacterController.IsCrouching())
        {
            if(_simpleCharacterController.IsCeilingBlocked())
                return;

            //ExitFromCrouchAction?.Invoke();
            CrouchPressedAction?.Invoke();
        }
        else
        {
            CrouchPressedAction?.Invoke();            
        }


    }

    private void HandleJumpKeyPressed()
    {
        _simpleCharacterController.ApplyJump();
    }

    private void HandleFireKeyPressed()
    {
        _gunFireController.Fire();
    }

    private void HandleAimKeyPressed()
    {
        if (_simpleCharacterController.IsAim())
        {
            _simpleCharacterController.ExitAim();
            ExitFromAimAction?.Invoke();
        }
        else
        {
            _simpleCharacterController.ApplyAim();
            ChangeToAimAction?.Invoke();
        }
    }

    private void HandleWalkKeyPressed()
    {
        RunWalkSwitchPressedAction?.Invoke();
    }


    
	#endregion    

    



    





}
