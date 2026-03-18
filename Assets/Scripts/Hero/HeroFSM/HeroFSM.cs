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
    public HealthController healthController;

    
    
    public Hud hud;
    public string lastState;

    public Action GoRun;
    public Action GoWalk;
    public Action GoCrouch;
    public Action GoAim;
    public Action GoDie;
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
    [SerializeField] private HealthController _healthController;

    [SerializeField] private HeroMovementController _heroMovementController;

    [SerializeField] private Hud _hud;
    [SerializeField] private HeroHurtController _heroHurtController;
    [SerializeField] private HeroStealthManager _heroStealthManager;

    

    [SerializeField] private States lastState;

    [SerializeField] private bool isPlayerHidden = false;

    public Action<Vector3,Vector3,RaycastHit> onFireAction;
    public Action receiveHitFromEnemyAction;

    public Action receiveDamageFromTrapAction;

    public Action<bool> hideStealthChangeAction;

    public Action HeroDiedAction;

    private GameObject _ammoDetected;
    private GameObject _interactableDetected;

    

    


    public DependenciesHeroFSM dependencies = new DependenciesHeroFSM
    {
        id = 0,
        inputHeroController = null,
        animatorHeroController = null,
        hud = null,
        animator = null,
        simpleCharacterController = null,
        gunFireController = null,
        healthController = null,
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
        STATE_AIM,
        STATE_DIE,
    }

    private void Awake()
    {
        dependencies.id = 0;
        _gunFireController.onFireAction += HandleOnFire;        

        dependencies.fsm = this;

        RunState run = AbstractState.Create<RunState, States>(States.STATE_RUN, this);
        run.Setup(ref dependencies);

        WalkState walk = AbstractState.Create<WalkState, States>(States.STATE_WALK, this);
        walk.Setup(ref dependencies);

        CrouchState crouch = AbstractState.Create<CrouchState, States>(States.STATE_CROUCH, this);
        crouch.Setup(ref dependencies);

        AimState aim = AbstractState.Create<AimState, States>(States.STATE_AIM, this);
        aim.Setup(ref dependencies);

        DieState die = AbstractState.Create<DieState, States>(States.STATE_DIE, this);
        die.Setup(ref dependencies);

        Init(States.STATE_RUN, run, walk, aim, crouch, die);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        int currentAmmo = _gunFireController.GetAmmo();
        _hud.SetAmmo(currentAmmo);

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

    public void GoDie()
    {
        TransitionToState(States.STATE_DIE);
    }

    public void HandleOnFire(Vector3 hitPoint, Vector3 hitNormal, RaycastHit hit)
    {
        _heroSoundManager.PlayGunFire();
        _heroVfxsManager.ShowGunFireVfxs();

        int ammo = _gunFireController.GetAmmo();
        _hud.SetAmmo(ammo);
        onFireAction?.Invoke(hitPoint,hitNormal,hit);
    }

    public void TriggerReceiveHitFromEnemy()
    {
        receiveHitFromEnemyAction?.Invoke();
    }

    public bool IsHidden()
    {
        isPlayerHidden =_heroStealthManager.IsHidden;
        return _heroStealthManager.IsHidden;
    }

    public void SetHiddenStealth()
    {
        isPlayerHidden =true;
        _heroStealthManager.SetHidden(true);
        hideStealthChangeAction?.Invoke(true);
    }

    public void SetShowStealth()
    {
        isPlayerHidden =false;
        _heroStealthManager.SetHidden(false);
        hideStealthChangeAction?.Invoke(false);
    }

    public void DetectAmmoBox(GameObject ammoBox)
    {
        //Should make the ammo glow
        _ammoDetected = ammoBox;
    }
    public void UnDetectAmmoBox()
    {
        //Should make the ammo glow
        _ammoDetected = null;
    }
    public GameObject GetInteractableAmmoDetected()
    {
        return _ammoDetected;
    }

    public void DetectInteractable(GameObject interactable)
    {
        _interactableDetected = interactable;
    }
    public void UndetectInteractable()
    {
        _interactableDetected = null;
    }
    public GameObject GetInteractableDetected()
    {
        return _interactableDetected;
    }
    
    public int GetAmmoInpocket()
    {
        return _gunFireController.GetAmmo();
    }
    

    private void HandleInteractAction()
    {
        GameObject ammoGO = GetInteractableAmmoDetected();
        if (ammoGO)
        {
            //take that ammo
            int nBulletInBox = ammoGO.transform.parent.GetComponent<Bullet>().GetNBulletsByAmmoBox();
            ammoGO.transform.parent.GetComponent<Bullet>().DeactivateBullet();            
            _gunFireController.IncreaseAmmo(nBulletInBox);
            int currentAmmo = _gunFireController.GetAmmo();
            _hud.SetAmmo(currentAmmo);

        }else
        {
            GameObject interactableGO = GetInteractableDetected();            
            if (interactableGO)
            {
                //action that interactable
                Actionable actionable = interactableGO.GetComponent<Actionable>();
                actionable.MakeInteraction();
            }
            
        }

        //GameObject health = GetInteractableHealthDetected()
        //if(healthGO)// ....
    }

    public void TriggerReceiveDamageFromTrap()
    {
        receiveDamageFromTrapAction?.Invoke();
    }

    public void SetHeroPosition(Vector3 position)
    {
        Transform hero = gameObject.transform.Find("Hero");
        Debug.Log("hero " + hero);
        if (hero != null)
        {
            hero.position = position;
        }
    }


    private void OnDestroy()
    {
        _gunFireController.onFireAction -= HandleOnFire;
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
            _dependencies.fsm._heroMovementController.AllowInteract();



            _dependencies.fsm._heroMovementController.CrouchPressedAction += HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction += HandleChangeToAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction += HandleChangeToWalk;
            _dependencies.fsm._heroMovementController.InteractAction += HandleInteractAction;

            _dependencies.fsm.receiveHitFromEnemyAction += HandleReceiveHitFromEnemy;
            _dependencies.fsm.receiveDamageFromTrapAction += HandleReceiveDamageFromTrap;


        }

        public override void OnExit()
        {
            _dependencies.lastState = "RUN_STATE";
            _dependencies.fsm.lastState = States.STATE_RUN;

            _dependencies.fsm._heroMovementController.CrouchPressedAction -= HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction -= HandleChangeToAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction -= HandleChangeToWalk;
            _dependencies.fsm._heroMovementController.InteractAction -= HandleInteractAction;

            _dependencies.fsm._heroMovementController.DisableCrouch();
            _dependencies.fsm._heroMovementController.DisableChangeToAim();
            _dependencies.fsm._heroMovementController.DisableJump();
            _dependencies.fsm._heroMovementController.IgnoreWalkRunSwitchPressed();
            _dependencies.fsm._heroMovementController.DisableInteract();

            _dependencies.fsm.receiveHitFromEnemyAction -= HandleReceiveHitFromEnemy;
            _dependencies.fsm.receiveDamageFromTrapAction -= HandleReceiveDamageFromTrap;
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
           
        private void HandleReceiveHitFromEnemy()
        {   

            bool hurtMade = _dependencies.fsm._heroHurtController.MakePlayerGetHurt();

            if (hurtMade)
            {
                _dependencies.fsm._healthController.DecreaseLife(1);
                bool isDead = _dependencies.fsm._healthController.IsDead();

                if (isDead)
                {
                    _dependencies.fsm.GoDie();
                }
            }
        }

        private void HandleReceiveDamageFromTrap()
        {   
            bool hurtMade = _dependencies.fsm._heroHurtController.MakePlayerGetHurt();
            
            if (hurtMade)
            {
                _dependencies.fsm._healthController.DecreaseLife(1);
                bool isDead = _dependencies.fsm._healthController.IsDead();

                if (isDead)
                {
                    _dependencies.fsm.GoDie();                    
                }

            }
        }
        

        private void HandleInteractAction()
        {   
            _dependencies.fsm.HandleInteractAction();
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
            _dependencies.fsm._heroMovementController.AllowInteract();

            _dependencies.fsm._heroMovementController.CrouchPressedAction += HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction += HandleChangeToAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction += HandleChangeToRun;
            _dependencies.fsm._heroMovementController.InteractAction += HandleInteractAction;

            _dependencies.fsm.receiveHitFromEnemyAction += HandleReceiveHitFromEnemy;
            _dependencies.fsm.receiveDamageFromTrapAction += HandleReceiveDamageFromTrap;

        }

        public override void OnExit()
        {
            _dependencies.fsm.lastState = States.STATE_WALK;

            _dependencies.fsm._heroMovementController.CrouchPressedAction -= HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction -= HandleChangeToAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction -= HandleChangeToRun;
            _dependencies.fsm._heroMovementController.InteractAction -= HandleInteractAction;
            _dependencies.fsm._heroMovementController.DisableInteract();

            _dependencies.fsm._heroMovementController.DisableCrouch();
            _dependencies.fsm._heroMovementController.DisableChangeToAim();
            _dependencies.fsm._heroMovementController.DisableJump();
            _dependencies.fsm._heroMovementController.IgnoreWalkRunSwitchPressed();

            _dependencies.fsm.receiveHitFromEnemyAction -= HandleReceiveHitFromEnemy;
            _dependencies.fsm.receiveDamageFromTrapAction -= HandleReceiveDamageFromTrap;        
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
        
        private void HandleReceiveHitFromEnemy()
        {   

            bool hurtMade = _dependencies.fsm._heroHurtController.MakePlayerGetHurt();

            if (hurtMade)
            {
                _dependencies.fsm._healthController.DecreaseLife(1);
                bool isDead = _dependencies.fsm._healthController.IsDead();

                if (isDead)
                {
                    _dependencies.fsm.GoDie();
                }
            }
        }

        private void HandleInteractAction()
        {
            _dependencies.fsm.HandleInteractAction();
        }
        private void HandleReceiveDamageFromTrap()
        {

            bool hurtMade = _dependencies.fsm._heroHurtController.MakePlayerGetHurt();

            bool isDead = _dependencies.fsm._healthController.IsDead();

            if (hurtMade && isDead)
            {
                _dependencies.fsm.GoDie();
            }
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
            _dependencies.fsm._heroMovementController.AllowInteract();

            _dependencies.fsm._heroMovementController.CrouchPressedAction += HandleExitFromCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction += HandleChangeToAim;
            _dependencies.fsm._heroMovementController.InteractAction += HandleInteractAction;


            _dependencies.fsm.receiveHitFromEnemyAction += HandleReceiveHitFromEnemy;
            _dependencies.fsm.receiveDamageFromTrapAction += HandleReceiveDamageFromTrap;
        }

        public override void OnExit()
        {
            //Put the animator in Walk Layer
            _dependencies.fsm.lastState = States.STATE_CROUCH;

            _dependencies.fsm._heroMovementController.DisableCrouch();//allow crouch exits from crouch
            _dependencies.fsm._heroMovementController.DisableChangeToAim();
            _dependencies.fsm._heroMovementController.DisableInteract();

            _dependencies.fsm._heroMovementController.CrouchPressedAction -= HandleExitFromCrouch;
            _dependencies.fsm._heroMovementController.ChangeToAimAction -= HandleChangeToAim;
            _dependencies.fsm._heroMovementController.InteractAction -= HandleInteractAction;        

            _dependencies.fsm.receiveHitFromEnemyAction -= HandleReceiveHitFromEnemy;
            _dependencies.fsm.receiveDamageFromTrapAction -= HandleReceiveDamageFromTrap;
        }
        private void HandleExitFromCrouch()
        {
            _dependencies.fsm.GoLastState();
        }

        private void HandleChangeToAim()
        {
            _dependencies.fsm.GoAim();
        }

        private void HandleReceiveHitFromEnemy()
        {   

            bool hurtMade = _dependencies.fsm._heroHurtController.MakePlayerGetHurt();

            if (hurtMade)
            {
                _dependencies.fsm._healthController.DecreaseLife(1);
                bool isDead = _dependencies.fsm._healthController.IsDead();

                if (isDead)
                {
                    _dependencies.fsm.GoDie();
                }
            }
        }

        private void HandleInteractAction()
        {   
            _dependencies.fsm.HandleInteractAction();
        }
        private void HandleReceiveDamageFromTrap()
        {   
            bool hurtMade = _dependencies.fsm._heroHurtController.MakePlayerGetHurt();

            bool isDead = _dependencies.fsm._healthController.IsDead();

            if (hurtMade && isDead)
            {
                _dependencies.fsm.GoDie();
            }
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
            _dependencies.fsm._heroMovementController.AllowInteract();


            _dependencies.fsm._heroMovementController.ExitFromAimAction += HandleExitFromAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction += HandleChangeMoveSpeed;
            _dependencies.fsm._heroMovementController.CrouchPressedAction += HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.InteractAction += HandleInteractAction;


            _dependencies.fsm.receiveHitFromEnemyAction += HandleReceiveHitFromEnemy;
            _dependencies.fsm.receiveDamageFromTrapAction += HandleReceiveDamageFromTrap;
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
            _dependencies.fsm._heroMovementController.DisableInteract();

            _dependencies.fsm._heroMovementController.ExitFromAimAction -= HandleExitFromAim;
            _dependencies.fsm._heroMovementController.RunWalkSwitchPressedAction -= HandleChangeMoveSpeed;
            _dependencies.fsm._heroMovementController.CrouchPressedAction -= HandleChangeToCrouch;
            _dependencies.fsm._heroMovementController.InteractAction -= HandleInteractAction;        

            _dependencies.fsm.receiveHitFromEnemyAction -= HandleReceiveHitFromEnemy;
            _dependencies.fsm.receiveDamageFromTrapAction -= HandleReceiveDamageFromTrap;
           
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
        
        private void HandleReceiveHitFromEnemy()
        {   

            bool hurtMade = _dependencies.fsm._heroHurtController.MakePlayerGetHurt();

            if (hurtMade)
            {
                _dependencies.fsm._healthController.DecreaseLife(1);
                bool isDead = _dependencies.fsm._healthController.IsDead();

                if (isDead)
                {
                    _dependencies.fsm.GoDie();
                }
            }
        }
        private void HandleInteractAction()
        {   
            _dependencies.fsm.HandleInteractAction();
        }
        private void HandleReceiveDamageFromTrap()
        {   
            bool hurtMade = _dependencies.fsm._heroHurtController.MakePlayerGetHurt();

            bool isDead = _dependencies.fsm._healthController.IsDead();

            if (hurtMade && isDead)
            {
                _dependencies.fsm.GoDie();
            }
        }
    }

    public class DieState : AbstractState
    {
        private DependenciesHeroFSM _dependencies;

        public void Setup(ref DependenciesHeroFSM deps)
        {
            _dependencies = deps;
        }

        public override void OnEnter()
        {
            Debug.Log("Die State");
            _dependencies.fsm._heroMovementController.DeadAnimationFinshedAction += HandleDeadAnimationFinshed;

            _dependencies.fsm._heroMovementController.BlockMovements();
            _dependencies.fsm._heroMovementController.ExecuteModeDie();            
        }

        public override void OnExit()
        {
            _dependencies.fsm._heroMovementController.DeadAnimationFinshedAction -= HandleDeadAnimationFinshed;

            _dependencies.fsm._heroMovementController.UnblockMovements();
        }
        
        private void HandleDeadAnimationFinshed()
        {
            _dependencies.fsm.HeroDiedAction?.Invoke();
        }
    }
}
