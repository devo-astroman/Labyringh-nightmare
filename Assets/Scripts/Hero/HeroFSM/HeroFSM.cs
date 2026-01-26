using UnityEngine;
using KevinCastejon.FiniteStateMachine;

public struct Dependencies
{
    public int id;
    public GameObject heroPrefab;
}

public class HeroFSM : AbstractFiniteStateMachine
{
    [Header("References")]
    [SerializeField] private Animator animator;

    public Dependencies deps = new Dependencies
    {
        id=0,
        
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
        deps.id = 0;

        RunState run = AbstractState.Create<RunState, States>(States.STATE_RUN, this);
        run.Setup(ref deps);

        Init(States.STATE_RUN, run);
    }

    public void GoFreezeState()
    {   
        //Inputs of the player are disabled
        
    }

    public void GoMovementState()
    {   
        //Inputs of the player are enabled
        
    }

    public void GoAnimaticState()
    {   
        //Inputs of the player are disabled
        //animation of the character runs
        
    }


    private void OnDestroy()
    {
    }

    public class RunState : AbstractState
    {
        private Dependencies deps;

        public void Setup(ref Dependencies dependencies)
        {
            deps = dependencies;
        }

        public override void OnEnter()
        {
            Debug.Log("RUN STATE");
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
        }
    }
}
