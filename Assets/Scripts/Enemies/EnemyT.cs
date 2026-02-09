using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyT : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Transform _target;
    [SerializeField] private float _scanCoolDown;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    [SerializeField] private EnemyHitManager _enemyHitManager;
    [SerializeField] private EnemyHealth _enemyHealth;

    [SerializeField] private float _enemyLife;
    [SerializeField] private float _hitCoolDown;
    private SetTimeoutUtility _timeoutHitCoolDown;

    [SerializeField] private EnemyAnimator _enemyAnimator;

    [SerializeField] private EnemyCollidersManager _enemyCollidersManager;

    [SerializeField] private EnemySoundManager _enemySoundManager;
   

    public Action<int> EnemyDeadAction;



    private bool _follow = true;
    
    private SetIntervalUtility _intervalToMove;

    void Start()
    {
        _enemySoundManager.PlayIdle();
        _timeoutHitCoolDown = new SetTimeoutUtility(this);

        _enemyHealth.SetLife(_enemyLife);
        //_enemyHealth.DeadAction += OnDeadAction;
        //_enemyHitManager.HitAction += OnHitAction;

        _enemyCollidersManager.HideColliderVisibility();

        _intervalToMove = new SetIntervalUtility(this);
        StartFollow();
    }


    public void SetId(int id)
    {
        _id = id;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void StopEnemy()
    {
        _follow = false;
    }

    public void StartFollow()
    {
        _follow = true;
        _intervalToMove.SetInterval(() => {
            if (_follow)
            {
                Vector3 position = new Vector3(_target.position.x,0,_target.position.z);

                //NavDebug.LogAgentState(_navMeshAgent,"AG-");
                _navMeshAgent.SetDestination(position);
            }
        }, .25f);
    }

    public void DeactivateEnemyCollisions()
    {        
        _enemyCollidersManager.RemoveCollisions();
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        //_enemyHealth.DeadAction -= OnDeadAction;
        //_enemyHitManager.HitAction -= OnHitAction;
        _intervalToMove.Dispose();
    }

    public void MakeHit(int amountDamage)
    {
        _enemyHealth.MakeDamage(amountDamage);
        _enemyHitManager.IgnoreHits();
        float currentEnemyLife = _enemyHealth.GetLife();
        //play hit animation

        
        _enemyAnimator.SetLife(currentEnemyLife);
        _navMeshAgent.isStopped = true;
        if(currentEnemyLife > 0)
        {
            _enemyAnimator.TriggerReceiveHit();
            _enemySoundManager.PlayReceiveHit();
            _timeoutHitCoolDown.SetTimeout(() =>
            {
                _enemyHitManager.AllowHits();
                _navMeshAgent.isStopped = false;
            },1f);

        }
        else
        {
            //play dead animation
            Debug.Log("Enemy is dead");
            _enemySoundManager.PlayDie();

        }
    }

    private void OnHitAction()
    {
        _enemyHealth.MakeDamage(1);
        _enemyHitManager.IgnoreHits();
        float currentEnemyLife = _enemyHealth.GetLife();
        //play hit animation

        
        _enemyAnimator.SetLife(currentEnemyLife);
        _navMeshAgent.isStopped = true;
        if(currentEnemyLife > 0)
        {
            _enemyAnimator.TriggerReceiveHit();    
            _timeoutHitCoolDown.SetTimeout(() =>
            {
                _enemyHitManager.AllowHits();
                _navMeshAgent.isStopped = false;
            },1f);

        }
        else
        {
            //play dead animation
            Debug.Log("Enemy is dead");
            EnemyDeadAction?.Invoke(_id);

        }


    }

    private void OnDeadAction()
    {
        //play dead animation
    }





}
