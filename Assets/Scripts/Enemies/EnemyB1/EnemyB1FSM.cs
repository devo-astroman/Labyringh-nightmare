using KevinCastejon.FiniteStateMachine;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;
using UnityEngine.Video;


public class DependenciesEnemyB1FSM
{
    public int id;
    public EnemyB1 enemyB1;
    public SetTimeoutUtility idleTimeout;
    public EnemyB1FSM fsm;
    public GameObject heroDetected;
}

public class EnemyB1FSM : AbstractFiniteStateMachine
{
    [SerializeField] private EnemyB1 _enemyB1;

    public enum States
    {
        IDLE_STATE,
        PATROL_STATE,
        FOLLOW_STATE,
        ATTACK_STATE,
        RECEIVE_HIT_STATE,
        DIE_STATE,
    }
    

    public DependenciesEnemyB1FSM dependencies = new DependenciesEnemyB1FSM
    {
        id = 0,
        idleTimeout = null,
        enemyB1 = null,
        fsm = null,
        heroDetected = null,
        
    };


    private void Awake()
    {        
        dependencies.idleTimeout = new SetTimeoutUtility(this);
        dependencies.enemyB1 = _enemyB1;
        dependencies.fsm = this;

        IdleState idle = AbstractState.Create<IdleState, States>(States.IDLE_STATE, this);
        idle.Setup(ref dependencies);

        PatrolState patrol = AbstractState.Create<PatrolState, States>(States.PATROL_STATE, this);
        patrol.Setup(ref dependencies);

        FollowState follow = AbstractState.Create<FollowState, States>(States.FOLLOW_STATE, this);
        follow.Setup(ref dependencies);

        FollowState attack = AbstractState.Create<FollowState, States>(States.FOLLOW_STATE, this);
        attack.Setup(ref dependencies);

        ReceiveHitState receiveHit = AbstractState.Create<ReceiveHitState, States>(States.RECEIVE_HIT_STATE, this);
        receiveHit.Setup(ref dependencies);

        DieState die = AbstractState.Create<DieState, States>(States.DIE_STATE, this);
        die.Setup(ref dependencies);

        Init(States.IDLE_STATE, idle, patrol, follow, attack, receiveHit, die);


    }

    

    public void GoToIdle()
    {
        TransitionToState(States.IDLE_STATE);
    }

    public void GoToPatrol()
    {
        TransitionToState(States.PATROL_STATE);
    }
    public void GoToFollow()
    {
        TransitionToState(States.FOLLOW_STATE);
    }

    public void GoToAttack()
    {
        TransitionToState(States.ATTACK_STATE);
    }

    public void GoToReceiveHit()
    {
        TransitionToState(States.RECEIVE_HIT_STATE);
    }

    public void GoToDie()
    {
        TransitionToState(States.DIE_STATE);
    }

    public void SetPatrolPoints(Vector3[] patrolPoints)
    {
        _enemyB1.SetPatrolPoints(patrolPoints);
    }


    public class IdleState : AbstractState
    {
        private DependenciesEnemyB1FSM _dependencies;

        public void Setup(ref DependenciesEnemyB1FSM deps)
        {
            Debug.Log("IdleSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("Idle");
           _dependencies.enemyB1.wakeUpAnimationEndsAction += HandleWakeUpAnimationEndsAction;

           _dependencies.enemyB1.gameObject.SetActive(true); //this will automatically run the idle wakeup animation
        }

        public override void OnExit()
        {
            if(_dependencies.idleTimeout != null)
                _dependencies.idleTimeout.Dispose();

           _dependencies.enemyB1.wakeUpAnimationEndsAction -= HandleWakeUpAnimationEndsAction;
        }

        private void HandleWakeUpAnimationEndsAction()
        {
            _dependencies.fsm.GoToPatrol();
        }
    }

    public class PatrolState : AbstractState
    {
        private DependenciesEnemyB1FSM _dependencies;

        public void Setup(ref DependenciesEnemyB1FSM deps)
        {
            Debug.Log("PatrolSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("Patrol");
           _dependencies.enemyB1.PlayPatrolAnimation();
           _dependencies.enemyB1.detectedHeroAction += HandleDetectedHeroAction;
           _dependencies.enemyB1.ExecutePatrol();
        }

        public override void OnExit()
        {
           _dependencies.enemyB1.detectedHeroAction -= HandleDetectedHeroAction;
           _dependencies.enemyB1.StopPatrol();
        }

        private void HandleDetectedHeroAction(GameObject hero)
        {
            _dependencies.heroDetected = hero;
            _dependencies.fsm.GoToFollow();
        }
    }

    public class FollowState : AbstractState
    {
        private DependenciesEnemyB1FSM _dependencies;

        public void Setup(ref DependenciesEnemyB1FSM deps)
        {
            Debug.Log("FollowSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("Follow");

           _dependencies.enemyB1.PlayPatrolAnimation(); //follow and patrol has the same animation
           
        }

        public override void OnExit()
        {
           
        }
    }

    public class AttackState : AbstractState
    {
        private DependenciesEnemyB1FSM _dependencies;

        public void Setup(ref DependenciesEnemyB1FSM deps)
        {
            Debug.Log("AttackSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("Attack");           
        }

        public override void OnExit()
        {
           
        }
    }

    public class ReceiveHitState : AbstractState
    {
        private DependenciesEnemyB1FSM _dependencies;

        public void Setup(ref DependenciesEnemyB1FSM deps)
        {
            Debug.Log("ReceiveHitSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("ReceiveHit");           
        }

        public override void OnExit()
        {
           
        }
    }

    public class DieState : AbstractState
    {
        private DependenciesEnemyB1FSM _dependencies;

        public void Setup(ref DependenciesEnemyB1FSM deps)
        {
            Debug.Log("DieSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("Die");           
        }

        public override void OnExit()
        {
           
        }
    }

}
