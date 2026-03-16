using UnityEngine;
using System;


public class EnemyManager : MonoBehaviour
{
    //[SerializeField] private EnemyT _enemyT;
    [SerializeField] private GameObject _enemyBatPref;
    [SerializeField] private GameObject _enemySpyderPref;
    [SerializeField] private GameObject _enemyFirerPref;
    [SerializeField] private EnemyPool _enemyPool;
    //private GameObject _enemyB1FSMGO;

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

    public GameObject CreateBatEnemy(Vector3 enemyPosition, Vector3[] patrolPoints)
    {
        GameObject enemyB1FSMGO = Instantiate(_enemyBatPref, enemyPosition, Quaternion.identity);
        enemyB1FSMGO.GetComponent<EnemyB1FSM>().SetPatrolPoints(patrolPoints);

        _enemyPool.AddEnemy(enemyB1FSMGO);
        
        return enemyB1FSMGO;
    }

    public GameObject CreateBatEnemy(Vector3 enemyPosition, Vector3[] patrolPoints, Transform parent)
    {
        GameObject enemyB1FSMGO = Instantiate(_enemyBatPref, enemyPosition, Quaternion.identity, parent);
        enemyB1FSMGO.GetComponent<EnemyB1FSM>().SetPatrolPoints(patrolPoints);

        _enemyPool.AddEnemy(enemyB1FSMGO);
        
        return enemyB1FSMGO;
    }

    public GameObject CreateSpyderEnemy(Vector3 enemyPosition, EnemyHeroTarget target)
    {
        GameObject enemySpyderFSMGO = Instantiate(_enemySpyderPref, enemyPosition, Quaternion.identity);
        enemySpyderFSMGO.GetComponent<EB2FSM>().SetTarget(target);

        _enemyPool.AddEnemy(enemySpyderFSMGO);

        return enemySpyderFSMGO;
    }

    public GameObject CreateSpyderEnemy(Vector3 enemyPosition, EnemyHeroTarget target, Transform parent)
    {
        GameObject enemySpyderFSMGO = Instantiate(_enemySpyderPref, enemyPosition, Quaternion.identity,parent);
        enemySpyderFSMGO.GetComponent<EB2FSM>().SetTarget(target);

        _enemyPool.AddEnemy(enemySpyderFSMGO);

        return enemySpyderFSMGO;
    }


    public GameObject CreateFirerEnemy(Vector3 enemyPosition)
    {
        GameObject enemyFirerFSMGO = Instantiate(_enemyFirerPref, enemyPosition, Quaternion.identity);
        _enemyPool.AddEnemy(enemyFirerFSMGO);

        return enemyFirerFSMGO;
    }

    public GameObject CreateFirerEnemy(Vector3 enemyPosition, Transform parent)
    {
        GameObject enemyFirerFSMGO = Instantiate(_enemyFirerPref, enemyPosition, Quaternion.identity, parent);
        _enemyPool.AddEnemy(enemyFirerFSMGO);

        return enemyFirerFSMGO;
    }

    public void ProcessDamage(int enemyId)
    {
        /* if(enemyId == 0)
        {
            //_enemyHealth.MakeDamage(20);
            //_enemyT.MakeHit(1);
            Debug.Log("Should Make Damage");
            _enemyB1FSMGO.GetComponent<EnemyB1FSM>().ReceiveDamage(0);
        } */
    }


    private void OnEnemyDeadAction(int id)
    {
        Debug.Log("Enemy is Dead");
        /* _enemyT.StopEnemy();
        _enemyT.DeactivateEnemyCollisions(); */
        EnemyDeadAction.Invoke(id);
    }



}
