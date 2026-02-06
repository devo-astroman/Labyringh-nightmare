using UnityEngine;
using System;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyT _enemyT;
    //private EnemyHealth _enemyHealth;

    private SetTimeoutUtility _timeout;
    public Action<int> EnemyDeadAction;

    void Start()
    {
        _timeout = new SetTimeoutUtility(this);
        _enemyT.SetId(0);
        _enemyT.EnemyDeadAction += OnEnemyDeadAction;
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        _enemyT.EnemyDeadAction -= OnEnemyDeadAction;
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
            _enemyT.MakeHit(1);
            
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
