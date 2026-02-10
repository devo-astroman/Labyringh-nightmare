using UnityEngine;
using System;
using UnityEngine.AI;


public class EnemyNavigatorManager : MonoBehaviour
{
    [SerializeField] private Transform _targetToFollow;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private Vector3[] _patrolPoints;
    private int _iPatrolPoint = -1;

    private bool _follow = true;
    private SetIntervalUtility _intervalToMove;

    void Start()
    {
    }

    void Update()
    {
    }

    public void ExecutePatrol()
    {
        if(_iPatrolPoint == -1)
        {
            PatrolToNextPoint();
        }

        _navMeshAgent.isStopped = false;
    }

    public void SetPatrolPoints(Vector3[] patrolPoints)
    {
        _patrolPoints = patrolPoints;
    }

    public void PatrolToNextPoint()
    {
        Vector3 nextPoint = GetNextPatrolPoint();
        _navMeshAgent.SetDestination(nextPoint);
    }

    public void StartFollow(Transform target)
    {
        _targetToFollow = target;
        _follow = true;
        _intervalToMove.SetInterval(() => {
            if (_follow)
            {
                Vector3 position = new Vector3(_targetToFollow.position.x,0,_targetToFollow.position.z);

                //NavDebug.LogAgentState(_navMeshAgent,"AG-");
                _navMeshAgent.SetDestination(position);
            }
        }, .25f);
    }

    public void StopNavigation()
    {
        _navMeshAgent.isStopped = false;
        _follow = false;
    }

    private Vector3 GetNextPatrolPoint()
    {
        _iPatrolPoint++;
        if(_iPatrolPoint >= _patrolPoints.Length)
            _iPatrolPoint=0;

        return _patrolPoints[_iPatrolPoint];
    }

    void OnDestroy()
    {
     
    }


}
