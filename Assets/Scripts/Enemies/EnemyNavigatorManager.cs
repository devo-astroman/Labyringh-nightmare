using UnityEngine;
using System;
using UnityEngine.AI;


public class EnemyNavigatorManager : MonoBehaviour
{
    [SerializeField] private Transform _targetToFollow;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private Vector3[] _patrolPoints;
    private int _iPatrolPoint = -1;

    [SerializeField]  private bool _follow = true;
    [SerializeField]  private bool _patrolling = false;
    private SetIntervalUtility _intervalToMove;

    void Start()
    {
    }

    void Update()
    {

        if (_patrolling && HasReachedDestination(_navMeshAgent))
        {
            PatrolToNextPoint();
        }
    }

    public void ExecutePatrol()
    {
        if(_iPatrolPoint == -1)
        {
            PatrolToNextPoint();
        }

        _navMeshAgent.isStopped = false;
        _patrolling = true;
        _follow = false;
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
        _patrolling = false;
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
        _navMeshAgent.isStopped = true;
        _follow = false;
    }

    private Vector3 GetNextPatrolPoint()
    {
        _iPatrolPoint++;
        if(_iPatrolPoint >= _patrolPoints.Length)
            _iPatrolPoint=0;

        return _patrolPoints[_iPatrolPoint];
    }

    private bool HasReachedDestination(NavMeshAgent agent)
    {
        if (!agent.isOnNavMesh)
            return false;

        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        if (agent.hasPath && agent.velocity.sqrMagnitude > 0.01f)
            return false;

        return true;
    }

    void OnDestroy()
    {
     
    }


}
