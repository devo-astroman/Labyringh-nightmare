using KevinCastejon.FiniteStateMachine;
using UnityEngine;
using System;


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
    private int _id;
    [SerializeField] private EnemyB1 _enemyB1;
    [SerializeField] private IntNotifier _intNotifier;

    public Action<int> ReceiveDamageAction;

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
    private States _lastState;

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

        AttackState attack = AbstractState.Create<AttackState, States>(States.ATTACK_STATE, this);
        attack.Setup(ref dependencies);

        ReceiveHitState receiveHit = AbstractState.Create<ReceiveHitState, States>(States.RECEIVE_HIT_STATE, this);
        receiveHit.Setup(ref dependencies);

        DieState die = AbstractState.Create<DieState, States>(States.DIE_STATE, this);
        die.Setup(ref dependencies);

        Init(States.IDLE_STATE, idle, patrol, follow, attack, receiveHit, die);
    }

    public void SetId(int id)
    {
        _id = id;
        _enemyB1.SetId(id);
    }

    public int GetId()
    {
        return _enemyB1.GetId();
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

    public void GoLastState()
    {
        TransitionToState(_lastState);
    }

    public void SetPatrolPoints(Vector3[] patrolPoints)
    {
        _enemyB1.SetPatrolPoints(patrolPoints);
    }

    public void ReceiveDamage(int damageValue)
    {
        Debug.Log("ReceiveDamage call!");
        ReceiveDamageAction?.Invoke(damageValue);
    }

    public States GetLastState()
    {
        return _lastState;
    }

    public void SetLastState(States state)
    {
        _lastState = state;
    }

    public void NotifyDie()
    {
        _intNotifier.NotifyInt(_id);
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
           //Debug.Log("Idle");
           
           _dependencies.enemyB1.wakeUpAnimationEndsAction += HandleWakeUpAnimationEndsAction;

           _dependencies.enemyB1.gameObject.SetActive(true); //this will automatically run the idle wakeup animation
           _dependencies.enemyB1.TurnOnMinimapIndicator();
        }

        public override void OnExit()
        {
            _dependencies.fsm.SetLastState(States.IDLE_STATE);

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
           
           
           _dependencies.enemyB1.ExecutePatrol();
           _dependencies.enemyB1.AllowReceiveDamage();

           _dependencies.fsm.ReceiveDamageAction += HandleReceiveDamage;
           _dependencies.enemyB1.farDetectedHeroAction += HandleDetectedHero;
           _dependencies.enemyB1.nearDetectedHeroAction += HandleNearDetectedHero;
        }

        public override void OnExit()
        {
           _dependencies.fsm.SetLastState(States.PATROL_STATE);
           
           _dependencies.enemyB1.StopPatrol();
           _dependencies.enemyB1.BlockReceiveDamage();

           _dependencies.fsm.ReceiveDamageAction -= HandleReceiveDamage;
           _dependencies.enemyB1.farDetectedHeroAction -= HandleDetectedHero;
           _dependencies.enemyB1.nearDetectedHeroAction -= HandleNearDetectedHero;
        }

        private void HandleDetectedHero(GameObject hero)
        {
            Debug.Log("HeroDetected!!!!");
            _dependencies.heroDetected = hero;
            _dependencies.fsm.GoToFollow();
        }

        private void HandleNearDetectedHero(GameObject hero)
        {
            Debug.Log("NearHeroDetected!!!!");            
            _dependencies.fsm.GoToAttack();
        }

        private void HandleReceiveDamage(int damageValue)
        {
           Debug.Log("Patrol DAMAGE!!!!");
           _dependencies.fsm.GoToReceiveHit();
           
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
           Debug.Log("*Follow*");
           _dependencies.enemyB1.PlayPatrolAnimation(); //follow and patrol has the same animation
           _dependencies.fsm.ReceiveDamageAction += HandleReceiveDamage;
           _dependencies.enemyB1.AllowReceiveDamage();

            if (_dependencies.heroDetected)
            {
                _dependencies.enemyB1.farUndetectedHeroAction += HandleUndetectedHero;
                _dependencies.enemyB1.nearDetectedHeroAction += HandleNearDetectedHero;

                EnemyHeroTarget enemyHeroTarget = new EnemyHeroTarget();
                enemyHeroTarget.SetCurrentTransform(_dependencies.heroDetected.transform);

                _dependencies.enemyB1.StartFollow(enemyHeroTarget);

                GameObject heroFSMGO = _dependencies.heroDetected.transform.parent.gameObject;
                HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();
                heroFSM.hideStealthChangeAction += HandleHideStealthChange;

            }
            else
            {
                _dependencies.fsm.GoLastState();
            }
           
        }

        public override void OnExit()
        {
           _dependencies.fsm.SetLastState(States.FOLLOW_STATE);
           _dependencies.fsm.ReceiveDamageAction -= HandleReceiveDamage;
           _dependencies.enemyB1.BlockReceiveDamage();
           _dependencies.enemyB1.farUndetectedHeroAction -= HandleUndetectedHero;
           _dependencies.enemyB1.nearDetectedHeroAction -= HandleNearDetectedHero;
            if (_dependencies.heroDetected)
            {
                GameObject heroFSMGO = _dependencies.heroDetected.transform.parent.gameObject;
                HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();
                heroFSM.hideStealthChangeAction -= HandleHideStealthChange;
            }
           
        }

        private void HandleUndetectedHero()
        {
            _dependencies.fsm.GoToPatrol();
        }

        private void HandleReceiveDamage(int damageValue)
        {
            Debug.Log("Patrol - OnExit - HandleReceiveDamage");
            _dependencies.fsm.GoToReceiveHit();
        }

        private void HandleNearDetectedHero(GameObject heroGO)
        {
            //Should attack
            _dependencies.fsm.GoToAttack();
        }

        private void HandleHideStealthChange(bool stealthValue)
        {
            Debug.Log("FOLLOW stealthValue " + stealthValue);
            if (stealthValue)
            {
                _dependencies.fsm.GoToPatrol();
            }
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
           Debug.Log("*Attack*");
            _dependencies.enemyB1.StopNavigation();

            _dependencies.enemyB1.CheckEndOfAttackAnimation();
            _dependencies.enemyB1.AllowReceiveDamage();
            
            _dependencies.enemyB1.attackTouchedHeroAction += HandleAttackTouchedHero;
            _dependencies.enemyB1.attackDamageStartAction += HandleAttackDamageStart;
            _dependencies.enemyB1.attackDamageEndAction += HandleAttackDamageEnd;
            _dependencies.enemyB1.tailAttackAnimationEndsAction += HandleTailAttackAnimationEnds;
            _dependencies.fsm.ReceiveDamageAction += HandleReceiveDamage;

            if (_dependencies.heroDetected)
            {
                GameObject heroFSMGO = _dependencies.heroDetected.transform.parent.gameObject;
                HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();
                heroFSM.hideStealthChangeAction += HandleHideStealthChange;    
            }

            _dependencies.enemyB1.ExecuteAttack();
        }

        public override void OnExit()
        {
            Debug.Log("Attack - OnExit");
           _dependencies.fsm.SetLastState(States.ATTACK_STATE);

           _dependencies.enemyB1.IgnoreEndOfAttackAnimation();
           _dependencies.enemyB1.IgnoreAttackHitHero();
           _dependencies.enemyB1.BlockReceiveDamage();

           _dependencies.enemyB1.attackTouchedHeroAction -= HandleAttackTouchedHero;
           _dependencies.enemyB1.attackDamageStartAction -= HandleAttackDamageStart;
           _dependencies.enemyB1.attackDamageEndAction -= HandleAttackDamageEnd;
           _dependencies.enemyB1.tailAttackAnimationEndsAction -= HandleTailAttackAnimationEnds;
           _dependencies.fsm.ReceiveDamageAction -= HandleReceiveDamage;

            if (_dependencies.heroDetected)
            {
                GameObject heroFSMGO = _dependencies.heroDetected.transform.parent.gameObject;
                HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();
                heroFSM.hideStealthChangeAction -= HandleHideStealthChange;
            }
        }

        private void HandleTailAttackAnimationEnds()
        {            
            _dependencies.fsm.GoToFollow();
        }

        private void HandleAttackDamageStart()
        {
            _dependencies.enemyB1.NotifyAttackHitHero();
        }

        private void HandleAttackDamageEnd()
        {
            _dependencies.enemyB1.IgnoreAttackHitHero();
        }

        private void HandleAttackTouchedHero(GameObject heroGO)
        {
            GameObject parentGO = heroGO.transform.parent?.gameObject;
            if (parentGO)
            {
                parentGO.GetComponent<HeroFSM>().TriggerReceiveHitFromEnemy();
            }
        }

        private void HandleReceiveDamage(int damageValue)
        {
            _dependencies.fsm.GoToReceiveHit();
        }

        private void HandleHideStealthChange(bool stealthValue)
        {
            if (stealthValue)
            {
                _dependencies.fsm.GoToPatrol();
            }
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
           Debug.Log("*ReceiveHit*");
           _dependencies.enemyB1.DecreaseLife();
           _dependencies.enemyB1.BlockReceiveDamage(); //This way will not receive more damage in this state

           _dependencies.enemyB1.getHurtAnimationEndsAction += HandleGetHurtAnimationEndsAction;
           _dependencies.enemyB1.CheckEndOfReceiveHitAnimation();
           _dependencies.enemyB1.PlayReceiveHitAnimation();
        }

        public override void OnExit()
        {
           _dependencies.fsm.SetLastState(States.RECEIVE_HIT_STATE);
           _dependencies.enemyB1.IgnoreEndOfReceiveHitAnimation();
           _dependencies.enemyB1.getHurtAnimationEndsAction -= HandleGetHurtAnimationEndsAction;
        }

        private void HandleGetHurtAnimationEndsAction()
        {
            int life = _dependencies.enemyB1.GetCurrentLife();

            if(life > 0)
            {
                _dependencies.fsm.GoLastState();
            }
            else
            {
                _dependencies.fsm.GoToDie();
            }

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
            _dependencies.enemyB1.BlockReceiveDamage(); //this way will not receive more damage
           _dependencies.enemyB1.ExecuteDieEnemy();
           _dependencies.enemyB1.TurnOffMinimapIndicator();
           _dependencies.fsm.NotifyDie();
        }

        public override void OnExit()
        {
           _dependencies.fsm.SetLastState(States.DIE_STATE);
        }
    }

}
