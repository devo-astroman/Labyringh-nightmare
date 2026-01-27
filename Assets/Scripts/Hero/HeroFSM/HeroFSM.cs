using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public struct Dependencies
{
    public int id;
    public InputHeroController inputHeroController;
    public AnimatorHeroController animatorHeroController;    
    public Animator animator;
    public SimpleCharacterController simpleCharacterController;
}

public class HeroFSM : AbstractFiniteStateMachine{    
    [SerializeField] private InputHeroController _inputHeroController;
    [SerializeField] private AnimatorHeroController _animatorHeroController;
    [SerializeField] private SimpleCharacterController _simpleCharacterController;

    
    public Dependencies deps = new Dependencies
    {
        id=0,
        inputHeroController=null,
        animatorHeroController=null,
        animator=null,
        simpleCharacterController = null,

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
        deps.id = 0;
        deps.inputHeroController = _inputHeroController;
        deps.animatorHeroController = _animatorHeroController;
        deps.simpleCharacterController = _simpleCharacterController;
        

        RunState run = AbstractState.Create<RunState, States>(States.STATE_RUN, this);
        run.Setup(ref deps);

        WalkState walk = AbstractState.Create<WalkState, States>(States.STATE_WALK, this);
        walk.Setup(ref deps);

        CrouchState crouch = AbstractState.Create<CrouchState, States>(States.STATE_CROUCH, this);
        crouch.Setup(ref deps);

        AimState aim = AbstractState.Create<AimState, States>(States.STATE_AIM, this);
        aim.Setup(ref deps);

        Init(States.STATE_RUN, run, walk, aim, crouch);
    }

    public void GoRun()
    {   
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
        private Dependencies _dependencies;

        public void Setup(ref Dependencies deps)
        {   Debug.Log("Setup-1-RUN");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            Debug.Log("RUN STATE ");
            _dependencies.animatorHeroController.SetRunMode();
            _dependencies.simpleCharacterController.ApplyRun();

            //Put the animator in Run Layer
        }
    }

    public class WalkState : AbstractState
    {
        private Dependencies _dependencies;

        public void Setup(ref Dependencies deps)
        {
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            Debug.Log("WALK STATE");
            //Put the animator in Walk Layer
            _dependencies.animatorHeroController.SetWalkMode();
            _dependencies.simpleCharacterController.ApplyWalk();
        }
    }

    public class CrouchState : AbstractState
    {
        private Dependencies _dependencies;

        public void Setup(ref Dependencies deps)
        {
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            Debug.Log("CROUCH STATE");
            //Put the animator in Crouch Layer
            _dependencies.animatorHeroController.SetCrouchMode();
            _dependencies.simpleCharacterController.ApplyCrouch();
        }
    }

    public class AimState : AbstractState
    {
        private Dependencies deps;

        public void Setup(ref Dependencies dependencies)
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
