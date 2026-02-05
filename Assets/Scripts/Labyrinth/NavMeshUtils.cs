using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtils
{
    public static bool PlaceAgentOnNavMesh(NavMeshAgent agent, float maxDistance = 5f)
    {
        if (agent == null) return false;

        if (NavMesh.SamplePosition(agent.transform.position, out var hit, maxDistance, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            return true;
        }

        Debug.LogError("No NavMesh found near agent position.");
        return false;
    }
}
