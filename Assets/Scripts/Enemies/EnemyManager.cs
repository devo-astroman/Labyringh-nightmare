using UnityEngine;
using System;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyT _enemyT;

    public void WakeUpEnemyT(Transform target)
    {
        _enemyT.SetTarget(target);
        _enemyT.gameObject.SetActive(true);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        
    }



}
