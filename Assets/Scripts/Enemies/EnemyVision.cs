using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] Transform eyes;
    [SerializeField] float maxSightDistance = 20f;

    [Header("Layers")]
    [SerializeField] LayerMask obstacleMask; // walls, props, etc.
    [SerializeField] LayerMask playerMask;   // player layer (optional)

    [Header("Tuning")]
    [SerializeField] float eyeHeight = 1.6f;
    [SerializeField] float playerAimHeight = 1.2f;
    [SerializeField] float sphereCastRadius = 0.15f; // helps with small gaps

    public bool CanSeePlayer(Transform player)
    {
        Vector3 origin = eyes != null ? eyes.position : transform.position + Vector3.up * eyeHeight;
        Vector3 target = player.position + Vector3.up * playerAimHeight;

        Vector3 dir = target - origin;
        float dist = dir.magnitude;
        if (dist <= 0.01f) return true;
        if (dist > maxSightDistance) return false;

        dir /= dist;

        // SphereCast is more forgiving than Raycast (corners, thin obstacles)
        if (Physics.SphereCast(origin, sphereCastRadius, dir, out RaycastHit hit, dist, obstacleMask, QueryTriggerInteraction.Ignore))
        {
            // We hit an obstacle before reaching the player
            return false;
        }

        // Optional: ensure we actually hit the player (if you want strict)
        // If you use playerMask, you can instead Raycast with (obstacleMask | playerMask) and check first hit.
        return true;
    }

    // Debug helper
    void OnDrawGizmosSelected()
    {
        if (eyes == null) return;
        Gizmos.DrawWireSphere(eyes.position, sphereCastRadius);
    }
}