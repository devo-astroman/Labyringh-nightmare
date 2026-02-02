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

    private bool _follow = true;
    
    private SetIntervalUtility _intervalToMove;

    void Start()
    {
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
                _navMeshAgent.SetDestination(position);
            }
        }, .25f);
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        _intervalToMove.Dispose();
    }



}
