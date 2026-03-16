using KevinCastejon.FiniteStateMachine;
using UnityEngine;
using System;

public class DepsEB2FSM
{    
    public int id;
    public EB2 eB2;    
    public EB2FSM fsm;
    public GameObject heroDetected;
}

public class EB2FSM : AbstractFiniteStateMachine
{
    
    [SerializeField] private EB2 _eB2;
    [SerializeField] private int _life = 2;
    [SerializeField] IntNotifier _diedNotifier;

    private int _id = 0;

    public Action<int> ReceiveDamageAction;
    public Action<int> EnemyDiedAction;

    public bool _testReceiveDamage = false;

    public enum States
    {
        IDLE_STATE,
        FOLLOW_STATE,
        RECEIVE_HIT_STATE,
        DIE_STATE,
    }
    

    public DepsEB2FSM deps = new DepsEB2FSM
    {
        id = 0,                
        eB2 = null,
        fsm = null,
        heroDetected = null,        
    };

    public SetTimeoutUtility _timeout;
    public bool _following = false;

    private void Awake()
    {   
        Debug.Log("Awake");           
    }

    private void OnEnable()
    {//for test
        _timeout = new SetTimeoutUtility(this);
        _eB2.SetHealthLife(_life);
        deps.eB2 = _eB2;
        deps.fsm = this;

        IdleState idle = AbstractState.Create<IdleState, States>(States.IDLE_STATE, this);
        idle.Setup(ref deps);

        FollowState follow = AbstractState.Create<FollowState, States>(States.FOLLOW_STATE, this);
        follow.Setup(ref deps);

        ReceiveHitState receiveHit = AbstractState.Create<ReceiveHitState, States>(States.RECEIVE_HIT_STATE, this);
        receiveHit.Setup(ref deps);

        DieState die = AbstractState.Create<DieState, States>(States.DIE_STATE, this);
        die.Setup(ref deps);

        Init(States.IDLE_STATE, idle, follow, receiveHit, die);
    }

    private void Update()
    {//for test
        if (_testReceiveDamage)
        {
            ReceiveDamage(1);
            _testReceiveDamage = false;
        }
    }

    private void OnDisable()
    {
        if(_timeout != null)
            _timeout.Dispose();
    }

    public void SetId(int id)
    {
        _id = id;
    }
    public void SetTarget(EnemyHeroTarget target)
    {
        _eB2.SetTargetToFollow(target);
    }

    public void GoToIdle()
    {
        TransitionToState(States.IDLE_STATE);
    }
    public void GoToReceiveHit()
    {
        TransitionToState(States.RECEIVE_HIT_STATE);
    }

    public void GoToFollow()
    {
        TransitionToState(States.FOLLOW_STATE);
    }

    public void GoToDie()
    {
        TransitionToState(States.DIE_STATE);
    }
    public void ReceiveDamage(int damageValue)
    {
        ReceiveDamageAction?.Invoke(damageValue);
    }

    public void NotifyDie()
    {
        EnemyDiedAction?.Invoke(_id);
        _diedNotifier.NotifyInt(_id);
    }

    public class IdleState : AbstractState
    {
        private DepsEB2FSM _dependencies;

        public void Setup(ref DepsEB2FSM deps)
        {
            Debug.Log("IdleSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*Idle*");

            _dependencies.fsm._timeout.SetTimeout(() => {
                _dependencies.fsm.GoToFollow();

            }, 2f); 
            
            _dependencies.eB2.TurnOnMinimapIndicator();
        }

        public override void OnExit()
        {
            _dependencies.fsm._timeout.Dispose();
        }
        
    }


    public class FollowState : AbstractState
    {
        private DepsEB2FSM _dependencies;

        public void Setup(ref DepsEB2FSM deps)
        {
            Debug.Log("FollowSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*Follow*");
            if (!_dependencies.fsm._following)
            {
                _dependencies.eB2.FollowTarget();
            }
            else
            {
                _dependencies.eB2.ResumeFollowTarget();
            }


           _dependencies.fsm.ReceiveDamageAction += HandleReceiveDamage;
           _dependencies.eB2.NearHeroDetectedAction += HandleNearHeroDetected;
        }

        public override void OnExit()
        {
           _dependencies.fsm.ReceiveDamageAction -= HandleReceiveDamage;
           _dependencies.eB2.NearHeroDetectedAction -= HandleNearHeroDetected;
           _dependencies.fsm._following = true;
        }

        private void HandleReceiveDamage(int damageValue)
        {
            Debug.Log("Patrol - OnExit - HandleReceiveDamage");
            _dependencies.fsm.GoToReceiveHit();
        }

        private void HandleNearHeroDetected(GameObject hero)
        {
            _dependencies.eB2.DamageTarget();
            
        }

        
        
    }


    public class ReceiveHitState : AbstractState
    {
        private DepsEB2FSM _dependencies;

        public void Setup(ref DepsEB2FSM deps)
        {
            Debug.Log("ReceiveHitSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*ReceiveHit*");
            _dependencies.eB2.ReceiveDamageEndsAction += HandleReceiveDamageEnds;
           _dependencies.eB2.ReceiveDamage(1);
        }

        public override void OnExit()
        {
            _dependencies.eB2.ReceiveDamageEndsAction -= HandleReceiveDamageEnds;
        }

        private void HandleReceiveDamageEnds()
        {
            float life = _dependencies.eB2.GetCurrentLife();

            if(life > 0)
            {
                _dependencies.fsm.GoToFollow();
            }
            else
            {
                _dependencies.fsm.GoToDie();
            }

        }
    }

    public class DieState : AbstractState
    {
        private DepsEB2FSM _dependencies;

        public void Setup(ref DepsEB2FSM deps)
        {
            Debug.Log("DieSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            _dependencies.eB2.Die();
            _dependencies.fsm.NotifyDie();
        }

        public override void OnExit()
        {
        }
    }




}


