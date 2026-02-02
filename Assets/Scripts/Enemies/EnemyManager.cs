using UnityEngine;
using System;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyT _enemyT;
    private EnemyHealth _enemyHealth;

    public void WakeUpEnemyT(Transform target)
    {
        _enemyT.SetTarget(target);
        _enemyHealth = _enemyT.GetComponent<EnemyHealth>();

        _enemyHealth.OnDead += HandleOnDead;

        _enemyT.gameObject.SetActive(true);
        //_enemyT.StartFollow();
    }

    public void ProcessDamage(int enemyId)
    {
        if(enemyId == 0)
        {
            _enemyHealth.MakeDamage(20);
        }
    }

    void Start()
    {
        _enemyT.SetId(0);
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        
    }

    private void HandleOnDead()
    {
        Debug.Log("Enemy is Dead");
        _enemyT.StopEnemy();
    }



}
