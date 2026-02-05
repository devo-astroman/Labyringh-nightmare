using UnityEngine;
using System;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyT _enemyT;
    private EnemyHealth _enemyHealth;

    private SetTimeoutUtility _timeout;

    void Start()
    {
        _timeout = new SetTimeoutUtility(this);
        _enemyT.SetId(0);
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        
    }

    public void WakeUpEnemyT(Vector3 enemyPosition, Transform target)
    {

        _enemyT.transform.position = enemyPosition;


        _enemyT.SetTarget(target);
        _enemyHealth = _enemyT.GetComponent<EnemyHealth>();

        _enemyHealth.OnDead += HandleOnDead;

        
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
            _enemyHealth.MakeDamage(20);
        }
    }

    private void HandleOnDead()
    {
        Debug.Log("Enemy is Dead");
        _enemyT.StopEnemy();
    }



}
