using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;

    private bool m_IsPlayerInRange;
    private Collider m_PlayerCollider;
    private string m_LastHit;

    private bool IsPlayer(Collider other)
    {
        return other.transform == player
            || other.transform.IsChildOf(player);
    }

    private void OnTriggerStay(Collider other)
    {
        if (player == null || !IsPlayer(other))
            return;

        if (!m_IsPlayerInRange)
        {
            Debug.Log("Detection: Player in range", this);
        }

        m_IsPlayerInRange = true;
        m_PlayerCollider = other;
    }

    private void OnTriggerExit(Collider other)
    {
        if (player != null && IsPlayer(other))
        {
            m_IsPlayerInRange = false;
            m_LastHit = null;
        }
    }

    private void Update()
    {
        if (!m_IsPlayerInRange || m_PlayerCollider == null)
            return;

        Vector3 direction =
            m_PlayerCollider.bounds.center - transform.position;

        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(
            transform.position,
            direction.normalized,
            out RaycastHit hit,
            direction.magnitude + 0.1f,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            if (m_LastHit != hit.collider.name)
            {
                m_LastHit = hit.collider.name;
                Debug.Log("Detection ray hit: " + m_LastHit, this);
            }

            if (IsPlayer(hit.collider))
            {
                if (gameEnding == null)
                {
                    Debug.LogError("Assign Game Ending on Observer", this);
                    return;
                }

                gameEnding.CaughtPlayer();
            }
        }
    }
}