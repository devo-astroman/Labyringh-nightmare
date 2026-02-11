using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using System;

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

    [SerializeField] private Hud _hud;

    public Action<Vector3,Vector3,RaycastHit> onFireAction;


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
        dependencies.inputHeroController = _inputHeroController;
        dependencies.animatorHeroController = _animatorHeroController;
        dependencies.simpleCharacterController = _simpleCharacterController;
        dependencies.cameraAimToMaskBridge = _cameraAimToMaskBridge;
        dependencies.gunFireController = _gunFireController;

        _gunFireController.onFireAction += HandleOnFireAction;

        
        dependencies.hud = _hud;

        dependencies.lastState = "";

        dependencies.GoRun = GoRun;
        dependencies.GoWalk = GoWalk;
        dependencies.GoCrouch = GoCrouch;
        dependencies.GoAim = GoAim;
        

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
            _dependencies.animatorHeroController.SetRunMode();
            _dependencies.simpleCharacterController.ApplyRun();

            //Put the animator in Run Layer
            _dependencies.inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
            _dependencies.inputHeroController.jumpKeyPressed += HandleJumpKeyPressed;
            _dependencies.inputHeroController.aimKeyPressed += HandleAimKeyPressed;
        }

        public override void OnExit()
        {
            _dependencies.lastState = "RUN_STATE";

            _dependencies.inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
            _dependencies.inputHeroController.jumpKeyPressed -= HandleJumpKeyPressed;
            _dependencies.inputHeroController.aimKeyPressed -= HandleAimKeyPressed;
        }

        private void HandleCrouchKeyPressed()
        {
            _dependencies.GoCrouch?.Invoke();
        }

        private void HandleWalkKeyPressed()
        {
            _dependencies.GoWalk?.Invoke();
        }

        private void HandleJumpKeyPressed()
        {   
            _dependencies.simpleCharacterController.ApplyJump();
        }

        private void HandleAimKeyPressed()
        {   Debug.Log(" -HandleAimKeyPressed- ");
            _dependencies.GoAim?.Invoke();
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
            //Put the animator in Walk Layer
            _dependencies.animatorHeroController.SetWalkMode();
            _dependencies.simpleCharacterController.ApplyWalk();            

            _dependencies.inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
            _dependencies.inputHeroController.jumpKeyPressed += HandleJumpKeyPressed;
            _dependencies.inputHeroController.aimKeyPressed += HandleAimKeyPressed;
        }

        public override void OnExit()
        {
            //Put the animator in Walk Layer
            _dependencies.lastState = "WALK_STATE";
            _dependencies.inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
            _dependencies.inputHeroController.jumpKeyPressed -= HandleJumpKeyPressed;
            _dependencies.inputHeroController.aimKeyPressed -= HandleAimKeyPressed;
        }

        private void HandleCrouchKeyPressed()
        {
            //should go to crouch state
            _dependencies.GoCrouch?.Invoke();
        }

        private void HandleWalkKeyPressed()
        {
            //should go to crouch state
            _dependencies.GoRun?.Invoke();
        }

        private void HandleJumpKeyPressed()
        {   //in the future go to jump state   
            _dependencies.simpleCharacterController.ApplyJump();
        }

        private void HandleAimKeyPressed()
        {   //in the future go to jump state   
            _dependencies.GoAim?.Invoke();
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
            //Put the animator in Crouch Layer
            _dependencies.animatorHeroController.SetCrouchMode();
            _dependencies.simpleCharacterController.ApplyCrouch();

            _dependencies.inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
            _dependencies.inputHeroController.aimKeyPressed += HandleAimKeyPressed;
        }

        public override void OnExit()
        {
            //Put the animator in Walk Layer
            _dependencies.lastState = "CROUCH_STATE";
            _dependencies.inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
            _dependencies.inputHeroController.aimKeyPressed -= HandleAimKeyPressed;
        }

        private void HandleCrouchKeyPressed()
        {
            Debug.Log("IS CEILING BLOCKED? " +  _dependencies.simpleCharacterController.IsCeilingBlocked());
            if(_dependencies.simpleCharacterController.IsCeilingBlocked())
                return;

            //should go to crouch state
            if(_dependencies.lastState == "WALK_STATE")
                _dependencies.GoWalk?.Invoke();
            else if(_dependencies.lastState == "RUN_STATE")
                _dependencies.GoRun?.Invoke();
            else if(_dependencies.lastState == "AIM_STATE")
                _dependencies.GoAim?.Invoke();
        }

        private void HandleWalkKeyPressed()
        {
            if(_dependencies.simpleCharacterController.IsCeilingBlocked())
                return;

            //should go to crouch state
            if(_dependencies.lastState == "WALK_STATE")
                _dependencies.GoWalk?.Invoke();
            else if(_dependencies.lastState == "RUN_STATE")
                _dependencies.GoRun?.Invoke();
            else if(_dependencies.lastState == "AIM_STATE")
                _dependencies.GoAim?.Invoke();
        }

        private void HandleAimKeyPressed()
        {
            if(_dependencies.simpleCharacterController.IsCeilingBlocked())
                return;

            //should go to aim
            _dependencies.GoAim?.Invoke();
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
            _dependencies.animatorHeroController.SetAimMode();

            _dependencies.inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
            _dependencies.inputHeroController.aimKeyPressed += HandleAimKeyPressed;
            _dependencies.inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
            _dependencies.inputHeroController.fireKeyPressed += HandleFireKeyPressed;

            _dependencies.cameraAimToMaskBridge.enabled = true;

            /* 
                small: 0
                med: 0.005
            
             */


            /* Vector3 velocity = _dependencies.simpleCharacterController.GetVelocity();

            if (velocity.Equals(Vector3.zero))
            {
                _dependencies.hud.SetSmallCrosshair();
            }
            else
            {
                if (_dependencies.simpleCharacterController.GetUsingRunSpeed())
                {
                    _dependencies.hud.SetBigCrosshair();
                }
                else
                {
                    _dependencies.hud.SetMedCrosshair();
                }


            } */


        }


        public override void OnUpdate()
        {
            float speed = _dependencies.simpleCharacterController.GetHorizontalSpeed();
            _dependencies.hud.SetCurrentSpeed(speed);

            if (speed  == 0)
                _dependencies.gunFireController.SetSpread(0);

            else if(speed < 2.5)
                _dependencies.gunFireController.SetSpread(0.005f);

            else
                _dependencies.gunFireController.SetSpread(0.01f);


        }

        public override void OnExit()
        {
            _dependencies.lastState = "AIM_STATE";
            _dependencies.inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
            _dependencies.inputHeroController.aimKeyPressed -= HandleAimKeyPressed;
            _dependencies.inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
            _dependencies.inputHeroController.fireKeyPressed -= HandleFireKeyPressed;

            _dependencies.cameraAimToMaskBridge.enabled = false;

            _dependencies.hud.HideCrosshair();
        }

        private void HandleWalkKeyPressed()
        {
            if (_dependencies.simpleCharacterController.GetUsingRunSpeed())
            {
                _dependencies.simpleCharacterController.ApplyWalk();
            }
            else
            {
                _dependencies.simpleCharacterController.ApplyRun();
            }
           
        }
        private void HandleAimKeyPressed()
        {
            if(_dependencies.lastState == "WALK_STATE" || _dependencies.lastState == "CROUCH_STATE")
                _dependencies.GoWalk?.Invoke();
            else if(_dependencies.lastState == "RUN_STATE")
                _dependencies.GoRun?.Invoke();            
        }

        private void HandleCrouchKeyPressed()
        {
            _dependencies.GoCrouch?.Invoke();
        }

        private void HandleFireKeyPressed()
        {            
            _dependencies.gunFireController.Fire();

        }
        
    }
}
