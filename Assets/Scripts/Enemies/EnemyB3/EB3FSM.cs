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

    public Action<int> ReceiveDamageAction;

    public bool _testReceiveDamage = false;
    public bool _testAttack = false;

    public enum States
    {
        HIDE_STATE,
        SHOW_STATE,
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

        ShowState show = AbstractState.Create<ShowState, States>(States.SHOW_STATE, this);
        show.Setup(ref deps);

        AttackState attack = AbstractState.Create<AttackState, States>(States.ATTACK_STATE, this);
        attack.Setup(ref deps);

        ReceiveHitState receiveHit = AbstractState.Create<ReceiveHitState, States>(States.RECEIVE_HIT_STATE, this);
        receiveHit.Setup(ref deps);

        DieState die = AbstractState.Create<DieState, States>(States.DIE_STATE, this);
        die.Setup(ref deps);

        Init(States.HIDE_STATE, hide, show, preattack, attack, receiveHit, die);
    }

    private void Update()
    {//for test
        if (_testReceiveDamage)
        {
            ReceiveDamage(1);
            _testReceiveDamage = false;
        }
    }
    
    public void GoToShow()
    {
        TransitionToState(States.SHOW_STATE);
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
           _dependencies.eB3.FarHeroDetectedAction -= HandleFarHeroDetected;
        }

        private void HandleFarHeroDetected(GameObject hero)
        {
            //_dependencies.eB3.SetHeroDetected(hero);
            _dependencies.fsm.GoToShow();
        }
    }

    public class ShowState : AbstractState
    {
        private DepsEB3FSM _dependencies;

        public void Setup(ref DepsEB3FSM deps)
        {
            Debug.Log("ShowSetup");
            _dependencies = deps;
        }

        public override void OnEnter()
        {
           Debug.Log("*Show*");
           _dependencies.eB3.Show();
           _dependencies.fsm.GoToPreattack();
        }

        public override void OnExit()
        {
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
        }

        public override void OnExit()
        {
            _dependencies.eB3.AttackEndsAction -= HandleAttackEnds;
            _dependencies.fsm.ReceiveDamageAction -= HandleReceiveDamage;
        }

        private void HandleAttackEnds()
        {
            //should notify to fire the bullet fire
            Debug.Log("should fire!!-- FIRE!");
            _dependencies.fsm.GoToPreattack();
        }

        private void HandleReceiveDamage(int damageValue)
        {
            _dependencies.eB3.ReceiveDamage(1);
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
        }

        public override void OnExit()
        {
        }
    }




}


