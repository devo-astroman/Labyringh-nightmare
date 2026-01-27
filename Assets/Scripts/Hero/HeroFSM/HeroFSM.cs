using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using System;

/* public struct Dependencies
{
    public int id;
    public InputHeroController inputHeroController;
    public AnimatorHeroController animatorHeroController;    
    public Animator animator;
    public SimpleCharacterController simpleCharacterController;
    public Action GoCrouch;
} */

public class DependenciesHeroFSM
{
    public int id;
    public InputHeroController inputHeroController;
    public AnimatorHeroController animatorHeroController;
    public Animator animator;
    public SimpleCharacterController simpleCharacterController;
    public string lastState;

    public Action GoRun;
    public Action GoWalk;
    public Action GoCrouch;
}

public class HeroFSM : AbstractFiniteStateMachine
{
    [SerializeField] private InputHeroController _inputHeroController;
    [SerializeField] private AnimatorHeroController _animatorHeroController;
    [SerializeField] private SimpleCharacterController _simpleCharacterController;


    public DependenciesHeroFSM dependencies = new DependenciesHeroFSM
    {
        id = 0,
        inputHeroController = null,
        animatorHeroController = null,
        animator = null,
        simpleCharacterController = null,
        lastState = "",
        GoRun = null,
        GoWalk = null,
        GoCrouch = null,        
    };

    public enum States
    {
        STATE_RUN,
        STATE_WALK,
        STATE_CROUCH,
        STATE_AIM
    }

    /*  public void Setup(InputHeroController inputHC, AnimatorHeroController animatorHC)
     {
         Debug.Log("Setup 0");
         deps.inputHeroController = inputHC;
         deps.animatorHeroController = animatorHC;
     } */

    private void Awake()
    {
        dependencies.id = 0;
        dependencies.inputHeroController = _inputHeroController;
        dependencies.animatorHeroController = _animatorHeroController;
        dependencies.simpleCharacterController = _simpleCharacterController;
        dependencies.lastState = "";

        dependencies.GoRun = GoRun;
        dependencies.GoWalk = GoWalk;
        dependencies.GoCrouch = GoCrouch;

        RunState run = AbstractState.Create<RunState, States>(States.STATE_RUN, this);
        run.Setup(ref dependencies);

        WalkState walk = AbstractState.Create<WalkState, States>(States.STATE_WALK, this);
        walk.Setup(ref dependencies);

        CrouchState crouch = AbstractState.Create<CrouchState, States>(States.STATE_CROUCH, this);
        crouch.Setup(ref dependencies);

        AimState aim = AbstractState.Create<AimState, States>(States.STATE_AIM, this);
        aim.Setup(ref dependencies);

        Init(States.STATE_RUN, run, walk, aim, crouch);
    }

    public void GoRun()
    {
        Debug.Log("SHOULD GO TO RUN");
        //Inputs of the player are enabled
        TransitionToState(States.STATE_RUN);
    }
    public void GoWalk()
    {
        //Inputs of the player are disabled
        TransitionToState(States.STATE_WALK);
    }

    public void GoCrouch()
    {
        //Inputs of the player are disabled
        //animation of the character runs
        TransitionToState(States.STATE_CROUCH);
    }

    public bool CanStandUp()
    {
        return _simpleCharacterController.IsCeilingBlocked();
    }


    private void OnDestroy()
    {
    }

    public class RunState : AbstractState
    {
        private DependenciesHeroFSM _dependencies;

        public void Setup(ref DependenciesHeroFSM deps)
        {
            Debug.Log("Setup-1-RUN");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            Debug.Log("RUN STATE ");


            _dependencies.animatorHeroController.SetRunMode();
            _dependencies.simpleCharacterController.ApplyRun();

            //Put the animator in Run Layer
            _dependencies.inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
            _dependencies.inputHeroController.jumpKeyPressed += HandleJumpKeyPressed;
        }

        public override void OnExit()
        {
            Debug.Log("RUN STATE ");
            _dependencies.lastState = "RUN_STATE";

            //Put the animator in Run Layer
            //unsubscribe to jump
            _dependencies.inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
            _dependencies.inputHeroController.jumpKeyPressed -= HandleJumpKeyPressed;
        }

        private void HandleCrouchKeyPressed()
        {
            //should go to crouch state
            _dependencies.GoCrouch?.Invoke();
        }

        private void HandleWalkKeyPressed()
        {
            //should go to crouch state
            Debug.Log("0-gowalk");
            _dependencies.GoWalk?.Invoke();
        }

        private void HandleJumpKeyPressed()
        {   //in the future go to jump state   
            _dependencies.simpleCharacterController.ApplyJump();
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
            Debug.Log("WALK STATE");
            //Put the animator in Walk Layer
            _dependencies.animatorHeroController.SetWalkMode();
            _dependencies.simpleCharacterController.ApplyWalk();

            _dependencies.inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
            _dependencies.inputHeroController.jumpKeyPressed += HandleJumpKeyPressed;
        }

        public override void OnExit()
        {
            Debug.Log("WALK STATE");
            //Put the animator in Walk Layer

            _dependencies.lastState = "WALK_STATE";
            _dependencies.inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
            _dependencies.inputHeroController.jumpKeyPressed -= HandleJumpKeyPressed;
        }

        private void HandleCrouchKeyPressed()
        {
            //should go to crouch state
            _dependencies.GoCrouch?.Invoke();
        }

        private void HandleWalkKeyPressed()
        {
            //should go to crouch state
            Debug.Log("1-gorun");
            _dependencies.GoRun?.Invoke();
        }

        private void HandleJumpKeyPressed()
        {   //in the future go to jump state   
            _dependencies.simpleCharacterController.ApplyJump();
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
            Debug.Log("CROUCH STATE");
            //Put the animator in Crouch Layer
            _dependencies.animatorHeroController.SetCrouchMode();
            _dependencies.simpleCharacterController.ApplyCrouch();

            _dependencies.inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
        }

        public override void OnExit()
        {
            Debug.Log("Crouch STATE");
            //Put the animator in Walk Layer
            _dependencies.lastState = "CROUCH_STATE";
            _dependencies.inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
            _dependencies.inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;

        }

        private void HandleCrouchKeyPressed()
        {
            //should go to crouch state
            if(_dependencies.lastState == "WALK_STATE")
                _dependencies.GoWalk?.Invoke();
            else if(_dependencies.lastState == "RUN_STATE")
                _dependencies.GoRun?.Invoke();
        }

        private void HandleWalkKeyPressed()
        {
            //should go to crouch state
            if(_dependencies.lastState == "WALK_STATE")
                _dependencies.GoWalk?.Invoke();
            else if(_dependencies.lastState == "RUN_STATE")
                _dependencies.GoRun?.Invoke();
        }
    }

    public class AimState : AbstractState
    {
        private DependenciesHeroFSM deps;

        public void Setup(ref DependenciesHeroFSM dependencies)
        {
            deps = dependencies;
        }

        public override void OnEnter()
        {
            Debug.Log("AIM STATE");
            //Put the animator in Aim Layer
        }
    }
}
