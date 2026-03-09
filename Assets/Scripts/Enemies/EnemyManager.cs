using UnityEngine;
using System;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyT _enemyT;
    [SerializeField] private GameObject _enemyB1Pref;
    [SerializeField] private GameObject _enemyB2Pref;
    [SerializeField] private GameObject _enemyB3Pref;
    [SerializeField] private EnemyPool _enemyPool;
    private GameObject _enemyB1FSMGO;

    private SetTimeoutUtility _timeout;
    public Action<int> EnemyDeadAction;

    void Start()
    {
        _timeout = new SetTimeoutUtility(this);
        /* _enemyT.SetId(0);
        _enemyT.EnemyDeadAction += OnEnemyDeadAction; */
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
       // _enemyT.EnemyDeadAction -= OnEnemyDeadAction;
    }

    public void WakeUpEnemyB1(Vector3 enemyPosition, Vector3[] patrolPoints)
    {
        GameObject enemyB1FSMGO = Instantiate(_enemyB1Pref, enemyPosition, Quaternion.identity);
        enemyB1FSMGO.GetComponent<EnemyB1FSM>().SetPatrolPoints(patrolPoints);

        _enemyPool.AddEnemy(enemyB1FSMGO);
    }

    public void WakeUpEnemyB2(Vector3 enemyPosition, Transform target)
    {
        GameObject enemyB2FSMGO = Instantiate(_enemyB2Pref, enemyPosition, Quaternion.identity);
        enemyB2FSMGO.GetComponent<EB2FSM>().SetTarget(target);

        _enemyPool.AddEnemy(enemyB2FSMGO);
    }

    public void WakeUpEnemyB3(Vector3 enemyPosition)
    {
        GameObject enemyB3FSMGO = Instantiate(_enemyB3Pref, enemyPosition, Quaternion.identity);
        _enemyPool.AddEnemy(enemyB3FSMGO);
    }

    public void WakeUpEnemyT(Vector3 enemyPosition, Transform target)
    {

        _enemyT.transform.position = enemyPosition;


        _enemyT.SetTarget(target);
        //_enemyHealth = _enemyT.GetComponent<EnemyHealth>();

        //_enemyHealth.DeadAction += HandleOnDead;

        
        //_enemyT.StartFollow();
         _timeout.SetTimeout(() => {
            _enemyT.gameObject.SetActive(true);
            //_enemyT.StartFollow();
        }, 2f); 
    }

    public void ProcessDamage(int enemyId)
    {
        if(enemyId == 0)
        {
            //_enemyHealth.MakeDamage(20);
            //_enemyT.MakeHit(1);
            Debug.Log("Should Make Damage");
            _enemyB1FSMGO.GetComponent<EnemyB1FSM>().ReceiveDamage(0);
        }
    }

    private void OnEnemyDeadAction(int id)
    {
        Debug.Log("Enemy is Dead");
        _enemyT.StopEnemy();
        _enemyT.DeactivateEnemyCollisions();
        EnemyDeadAction.Invoke(id);
    }



}
