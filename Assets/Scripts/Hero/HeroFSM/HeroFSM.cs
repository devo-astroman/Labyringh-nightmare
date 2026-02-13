using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using System;
using Unity.VisualScripting;

public class DependenciesHeroFSM
{
    public int id;
    public InputHeroController inputHeroController;
    public AnimatorHeroController animatorHeroController;
    public Animator animator;
    public SimpleCharacterController simpleCharacterController;
    public CameraAimToMaskBridge cameraAimToMaskBridge;
    public GunFireController gunFireController;
    
    public Hud hud;
    public string lastState;

    public Action GoRun;
    public Action GoWalk;
    public Action GoCrouch;
    public Action GoAim;
    public Func<bool> CanStandUp;
    public HeroFSM fsm;
}

public class HeroFSM : AbstractFiniteStateMachine
{
    [SerializeField] private InputHeroController _inputHeroController;
    [SerializeField] private AnimatorHeroController _animatorHeroController;
    [SerializeField] private SimpleCharacterController _simpleCharacterController;
    [SerializeField] private CameraAimToMaskBridge _cameraAimToMaskBridge;
    [SerializeField] private GunFireController _gunFireController;
    [SerializeField] private HeroSoundManager _heroSoundManager;
    [SerializeField] private HeroVfxsManager _heroVfxsManager;

    [SerializeField] private HeroMovementController _heroMovementController;

    [SerializeField] private Hud _hud;

    [SerializeField] private States lastState;

    public Action<Vector3,Vector3,RaycastHit> onFireAction;
    public Action receiveHitFromEnemyAction;

    


    public DependenciesHeroFSM dependencies = new DependenciesHeroFSM
    {
        id = 0,
        inputHeroController = null,
        animatorHeroController = null,
        hud = null,
        animator = null,
        simpleCharacterController = null,
        gunFireController = null,
        lastState = "",
        GoRun = null,
        GoWalk = null,
        GoCrouch = null,
        GoAim = null,
        fsm = null,
    };

    public enum States
    {
        STATE_RUN,
        STATE_WALK,
        STATE_CROUCH,
        STATE_AIM
    }

    private void Awake()
    {
        dependencies.id = 0;
        /* dependencies.inputHeroController = _inputHeroController;
        dependencies.animatorHeroController = _animatorHeroController;
        dependencies.simpleCharacterController = _simpleCharacterController;
        dependencies.cameraAimToMaskBridge = _cameraAimToMaskBridge;
        dependencies.gunFireController = _gunFireController; */

        _gunFireController.onFireAction += HandleOnFireAction;
        

        dependencies.fsm = this;
        

        RunState run = AbstractState.Create<RunState, States>(States.STATE_RUN, this);
        run.Setup(ref dependencies);

        WalkState walk = AbstractState.Create<WalkState, States>(States.STATE_WALK, this);
        walk.Setup(ref dependencies);

        CrouchState crouch = AbstractState.Create<CrouchState, States>(States.STATE_CROUCH, this);
        crouch.Setup(ref dependencies);

        AimState aim = AbstractState.Create<AimState, States>(States.STATE_AIM, this);
        aim.Setup(ref dependencies);

        Init(States.STATE_RUN, run, walk, aim, crouch);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoLastState()
    {
        TransitionToState(lastState);
    }
    public void GoRun()
    {
        TransitionToState(States.STATE_RUN);
    }
    public void GoWalk()
    {
        TransitionToState(States.STATE_WALK);
    }

    public void GoCrouch()
    {
        TransitionToState(States.STATE_CROUCH);
    }

    public void GoAim()
    {
        TransitionToState(States.STATE_AIM);
    }

    public void HandleOnFireAction(Vector3 hitPoint, Vector3 hitNormal, RaycastHit hit)
    {
        _heroSoundManager.PlayGunFire();
        _heroVfxsManager.ShowGunFireVfxs();
        onFireAction?.Invoke(hitPoint,hitNormal,hit);
    }

    public void TriggerReceiveHitFromEnemy()
    {
        Debug.Log("-TriggerReceiveHitFromEnemy-");
        receiveHitFromEnemyAction?.Invoke();
    }


    private void OnDestroy()
    {
        _gunFireController.onFireAction -= HandleOnFireAction;
    }

    public class RunState : AbstractState
    {
        private DependenciesHeroFSM _dependencies;

        public void Setup(ref DependenciesHeroFSM deps)
        {
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            _dependencies.fsm._heroMovementController.ExecuteModeRun();
            _dependencies.fsm._heroMovementController.AllowCrouch();
            _dependencies.fsm._heroMovementController.AllowChangeToAim();
            _dependencies.fsm._heroMovementController.AllowJump();
            _dependencies.fsm._heroMovementController.ListenWalkRunSwitchPressed();

            _dependencies.fsm._heroMovementController.CrouchPressedAction += HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction += HandleChangeToAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction += HandleChangeToWalk;
        }

        public override void OnExit()
        {
            _dependencies.lastState = "RUN_STATE";
            _dependencies.fsm.lastState = States.STATE_RUN;

            _dependencies.fsm._heroMovementController.CrouchPressedAction -= HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction -= HandleChangeToAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction -= HandleChangeToWalk;

            _dependencies.fsm._heroMovementController.DisableCrouch();
            _dependencies.fsm._heroMovementController.DisableChangeToAim();
            _dependencies.fsm._heroMovementController.DisableJump();
            _dependencies.fsm._heroMovementController.IgnoreWalkRunSwitchPressed();
        }

        private void HandleChangeToCrouch()
        {   
            _dependencies.fsm.GoCrouch();
        }
        private void HandleChangeToAim()
        {   
            _dependencies.fsm.GoAim();
        }
        private void HandleChangeToWalk()
        {   
            _dependencies.fsm.GoWalk();
        }
    }

    public class WalkState : AbstractState
    {
        private DependenciesHeroFSM _dependencies;

        public void Setup(ref DependenciesHeroFSM deps)
        {
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            _dependencies.fsm._heroMovementController.ExecuteModeWalk();
            _dependencies.fsm._heroMovementController.AllowCrouch();
            _dependencies.fsm._heroMovementController.AllowChangeToAim();
            _dependencies.fsm._heroMovementController.AllowJump();
            _dependencies.fsm._heroMovementController.ListenWalkRunSwitchPressed();

            _dependencies.fsm._heroMovementController.CrouchPressedAction += HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction += HandleChangeToAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction += HandleChangeToRun;
        }

        public override void OnExit()
        {
            _dependencies.fsm.lastState = States.STATE_WALK;

            _dependencies.fsm._heroMovementController.CrouchPressedAction -= HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction -= HandleChangeToAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction -= HandleChangeToRun;

            _dependencies.fsm._heroMovementController.DisableCrouch();
            _dependencies.fsm._heroMovementController.DisableChangeToAim();
            _dependencies.fsm._heroMovementController.DisableJump();
            _dependencies.fsm._heroMovementController.IgnoreWalkRunSwitchPressed();
        }

        private void HandleChangeToCrouch()
        {   
            _dependencies.fsm.GoCrouch();
        }

        private void HandleChangeToAim()
        {   
            _dependencies.fsm.GoAim();
        }

        private void HandleChangeToRun()
        {   
            _dependencies.fsm.GoRun();
        }
    }

    public class CrouchState : AbstractState
    {
        private DependenciesHeroFSM _dependencies;

        public void Setup(ref DependenciesHeroFSM deps)
        {
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            _dependencies.fsm._heroMovementController.ExecuteModeCrouch();
            _dependencies.fsm._heroMovementController.AllowCrouch();//should exit from crouch
            _dependencies.fsm._heroMovementController.AllowChangeToAim();

            _dependencies.fsm._heroMovementController.CrouchPressedAction += HandleExitFromCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction += HandleChangeToAim;
        }

        public override void OnExit()
        {
            //Put the animator in Walk Layer
            _dependencies.fsm.lastState = States.STATE_CROUCH;

            _dependencies.fsm._heroMovementController.DisableCrouch();//allow crouch exits from crouch
            _dependencies.fsm._heroMovementController.DisableChangeToAim();

            _dependencies.fsm._heroMovementController.CrouchPressedAction -= HandleExitFromCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction -= HandleChangeToAim;
        }
        private void HandleExitFromCrouch()
        {
            _dependencies.fsm.GoLastState();
        }

        private void HandleChangeToAim()
        {
            _dependencies.fsm.GoAim();
        }
    }

    public class AimState : AbstractState
    {
        private DependenciesHeroFSM _dependencies;

        public void Setup(ref DependenciesHeroFSM deps)
        {
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            Debug.Log("AIM STATE");
            //Put the animator in Aim Layer
            _dependencies.fsm._heroMovementController.ExecuteModeAim();

            _dependencies.fsm._heroMovementController.AllowFireGun();
            _dependencies.fsm._heroMovementController.AllowChangeToAim(); //Works to exit the aim
            _dependencies.fsm._heroMovementController.ListenWalkRunSwitchPressed();
            _dependencies.fsm._heroMovementController.AllowCrouch();


            _dependencies.fsm._heroMovementController.ExitFromAimAction += HandleExitFromAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction += HandleChangeMoveSpeed;
            _dependencies.fsm._heroMovementController.CrouchPressedAction += HandleChangeToCrouch;
        }


        public override void OnUpdate()
        {            
            _dependencies.fsm._heroMovementController.RefreshAim();
        }

        public override void OnExit()
        {
            _dependencies.fsm.lastState = States.STATE_AIM;
            _dependencies.fsm._heroMovementController.ExitModeAim();

            _dependencies.fsm._heroMovementController.DisableFireGun();
            _dependencies.fsm._heroMovementController.DisableChangeToAim(); //Works to exit the aim
            _dependencies.fsm._heroMovementController.IgnoreWalkRunSwitchPressed();
            _dependencies.fsm._heroMovementController.DisableCrouch();

            _dependencies.fsm._heroMovementController.ExitFromAimAction -= HandleExitFromAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction -= HandleChangeMoveSpeed;
            _dependencies.fsm._heroMovementController.CrouchPressedAction -= HandleChangeToCrouch;
           
        }
        private void HandleExitFromAim()
        {
            if(_dependencies.fsm.lastState == States.STATE_RUN || _dependencies.fsm.lastState == States.STATE_WALK)
            {
                _dependencies.fsm.GoLastState();
            }
            else
            {
                _dependencies.fsm.GoWalk();
            }
        }

        private void HandleChangeMoveSpeed()
        {
            _dependencies.fsm._heroMovementController.FlipBetweenRunAndWalkSpeed();
        }

        private void HandleChangeToCrouch()
        {
            _dependencies.fsm.GoCrouch();
        }
        
    }
}
