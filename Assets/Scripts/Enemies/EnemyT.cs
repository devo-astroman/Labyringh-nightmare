using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyT : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _scanCoolDown;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    
    private SetIntervalUtility _intervalToMove;

    void Start()
    {
        _intervalToMove = new SetIntervalUtility(this);

        _intervalToMove.SetInterval(() => {
            Debug.Log("Should spawn the player");
            Vector3 position = new Vector3(_target.position.x,0,_target.position.z);
            _navMeshAgent.SetDestination(position);

        }, .25f);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        _intervalToMove.Dispose();
    }



}
