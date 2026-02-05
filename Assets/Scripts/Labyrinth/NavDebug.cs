using UnityEngine;
using UnityEngine.AI;

public static class NavDebug
{
    public static void LogAgentState(NavMeshAgent agent, string prefix = "")
    {
        if (agent == null)
        {
            Debug.LogError(prefix + "Agent is null");
            return;
        }

        Debug.Log(
            $"{prefix} activeInHierarchy={agent.gameObject.activeInHierarchy}, " +
            $"enabled={agent.enabled}, isOnNavMesh={agent.isOnNavMesh}, " +
            $"pos={agent.transform.position}, agentTypeID={agent.agentTypeID}"
        );
    }
}
