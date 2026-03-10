using KevinCastejon.FiniteStateMachine;
using UnityEngine;
using System;

public class DepsEB3FSM
{    
    public int id;
    public EB3 eB3;    
    public EB3FSM fsm;
    public GameObject heroDetected;
}

public class EB3FSM : AbstractFiniteStateMachine
{
    
    [SerializeField] private EB3 _eB3;
    [SerializeField] private int _life = 2;
    [SerializeField] private IntNotifier _diedNotifier;

    private int _id = 0;

    public Action<int> ReceiveDamageAction;
    public Action<int> EnemyDiedAction;

    public bool _testReceiveDamage = false;
    public bool _testAttack = false;

    public enum States
    {
        HIDE_STATE,
        SCAN_STATE,
        PREATTACK_STATE,
        ATTACK_STATE,
        RECEIVE_HIT_STATE,
        DIE_STATE,
    }
    

    public DepsEB3FSM deps = new DepsEB3FSM
    {
        id = 0,                
        eB3 = null,
        fsm = null,
        heroDetected = null,        
    };

    public bool _following = false;

    private void Awake()
    {
        _eB3.SetHealthLife(_life);
        deps.eB3 = _eB3;
        deps.fsm = this;

        PreAttackState preattack = AbstractState.Create<PreAttackState, States>(States.PREATTACK_STATE, this);
        preattack.Setup(ref deps);

        HideState hide = AbstractState.Create<HideState, States>(States.HIDE_STATE, this);
        hide.Setup(ref deps);

        ScanState scan = AbstractState.Create<ScanState, States>(States.SCAN_STATE, this);
        scan.Setup(ref deps);

        AttackState attack = AbstractState.Create<AttackState, States>(States.ATTACK_STATE, this);
        attack.Setup(ref deps);

        ReceiveHitState receiveHit = AbstractState.Create<ReceiveHitState, States>(States.RECEIVE_HIT_STATE, this);
        receiveHit.Setup(ref deps);

        DieState die = AbstractState.Create<DieState, States>(States.DIE_STATE, this);
        die.Setup(ref deps);

        Init(States.HIDE_STATE, hide, scan, preattack, attack, receiveHit, die);
    }

    private void Update()
    {//for test
        if (_testReceiveDamage)
        {
            ReceiveDamage(1);
            _testReceiveDamage = false;
        }
    }
    public void SetId(int id)
    {
        _id = id;
    }
    public void GoToScan()
    {
        TransitionToState(States.SCAN_STATE);
    }
    public void GoToHide()
    {
        TransitionToState(States.HIDE_STATE);
    }
    public void GoToPreattack()
    {
        TransitionToState(States.PREATTACK_STATE);
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
    public void ReceiveDamage(int damageValue)
    {
        Debug.Log("___ReceiveDamage___");
        ReceiveDamageAction?.Invoke(damageValue);
    }

    public void NotifyDie()
    {
        EnemyDiedAction?.Invoke(_id);
        _diedNotifier.NotifyInt(_id);
    }

    public class HideState : AbstractState
    {
        private DepsEB3FSM _dependencies;

        public void Setup(ref DepsEB3FSM deps)
        {
            Debug.Log("HideSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*Hide* " + _dependencies.eB3);
           _dependencies.eB3.WaitHideForHero();
           _dependencies.eB3.FarHeroDetectedAction += HandleFarHeroDetected;
        }

        public override void OnExit()
        {
           _dependencies.eB3.Show();
           _dependencies.eB3.FarHeroDetectedAction -= HandleFarHeroDetected;
        }

        private void HandleFarHeroDetected(GameObject hero)
        {
            //_dependencies.eB3.SetHeroDetected(hero);
            _dependencies.fsm.GoToScan();
        }
    }

    public class ScanState : AbstractState
    {
        private DepsEB3FSM _dependencies;

        public void Setup(ref DepsEB3FSM deps)
        {
            Debug.Log("ScanStateSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*ScanState*");
           _dependencies.eB3.FarHeroUndetectionAction += HandleFarHeroUndetection;
           _dependencies.fsm.ReceiveDamageAction += HandleReceiveDamage;

           _dependencies.eB3.EnemySawAction += HandleEnemySaw;
           _dependencies.eB3.ScanHero();
        }

        public override void OnExit()
        {
            _dependencies.eB3.FarHeroUndetectionAction -= HandleFarHeroUndetection;
            _dependencies.eB3.EnemySawAction -= HandleEnemySaw;
            _dependencies.fsm.ReceiveDamageAction -= HandleReceiveDamage;
            _dependencies.eB3.StopScanHero();
        }

        private void HandleEnemySaw(Vector3 position)
        {
            _dependencies.eB3.SetLastPlaceHeroWasSee(position);
            _dependencies.fsm.GoToPreattack();
        }

        private void HandleFarHeroUndetection()
        {
            _dependencies.fsm.GoToHide();
        }

        private void HandleReceiveDamage(int damageValue)
        {
            _dependencies.eB3.ReceiveDamage(1);
            if (_dependencies.eB3.GetCurrentLife() <= 0)
            {
                _dependencies.fsm.GoToDie();
            }
        }
    }

    

    public class PreAttackState : AbstractState
    {
        private DepsEB3FSM _dependencies;

        public void Setup(ref DepsEB3FSM deps)
        {
            Debug.Log("PreattackSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*Preattack*");
            if(_dependencies.eB3.GetCurrentLife() <= 0)
            {
                _dependencies.fsm.GoToDie();
            }
            else
            {
                if (_dependencies.eB3.IsHeroDetected())
                {
                    //go to attack
                    _dependencies.fsm.GoToAttack();
                }
                else
                {
                    //go back to hide
                    _dependencies.fsm.GoToHide();
                }
            }
        }

        public override void OnExit()
        {
        }
    }

    public class AttackState : AbstractState
    {
        private DepsEB3FSM _dependencies;

        public void Setup(ref DepsEB3FSM deps)
        {
            Debug.Log("AttackSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*Attack*");           
           _dependencies.eB3.AttackEndsAction += HandleAttackEnds;
           if(!_dependencies.eB3.Attack()){
            //the attack was not 
                _dependencies.fsm.GoToPreattack();
           }

           _dependencies.fsm.ReceiveDamageAction += HandleReceiveDamage;
           _dependencies.eB3.AttackPrimeMomentReachedAction += HandleAttackPrimeMomentReached;

        }

        public override void OnExit()
        {
            _dependencies.eB3.AttackEndsAction -= HandleAttackEnds;
            _dependencies.fsm.ReceiveDamageAction -= HandleReceiveDamage;
            _dependencies.eB3.AttackPrimeMomentReachedAction -= HandleAttackPrimeMomentReached;
        }

        private void HandleAttackPrimeMomentReached(){
            _dependencies.eB3.MakeFireAttack();
        }

        private void HandleAttackEnds()
        {
            _dependencies.fsm.GoToScan();
        }
        
        private void HandleReceiveDamage(int damageValue)
        {
            _dependencies.eB3.ReceiveDamage(1);
            if (_dependencies.eB3.GetCurrentLife() <= 0)
            {
                _dependencies.fsm.GoToDie();
            }
        }
        
    }


    public class ReceiveHitState : AbstractState
    {
        private DepsEB3FSM _dependencies;

        public void Setup(ref DepsEB3FSM deps)
        {
            Debug.Log("ReceiveHitSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*ReceiveHit*");
           _dependencies.eB3.ReceiveDamageEndsAction += HandleReceiveDamageEnds;
           _dependencies.eB3.ReceiveDamage(1);
        }

        public override void OnExit()
        {
            _dependencies.eB3.ReceiveDamageEndsAction -= HandleReceiveDamageEnds;
        }

        private void HandleReceiveDamageEnds()
        {
            float life = _dependencies.eB3.GetCurrentLife();

            if(life > 0)
            {
                _dependencies.fsm.GoToPreattack();
            }
            else
            {
                _dependencies.fsm.GoToDie();
            }

        }
    }

    public class DieState : AbstractState
    {
        private DepsEB3FSM _dependencies;

        public void Setup(ref DepsEB3FSM deps)
        {
            Debug.Log("DieSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            Debug.Log("*Die*");
            _dependencies.eB3.Die();
            _dependencies.fsm.NotifyDie();
        }

        public override void OnExit()
        {
        }
    }




}


