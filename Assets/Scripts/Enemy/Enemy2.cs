using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float chaseRange = 12f;
    public float updateRate = 0.2f;

    UnityEngine.AI.NavMeshAgent agent;
    Transform currentTarget;

    void Start()
    {
         agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("EnemyAI: Falta NavMeshAgent");
            enabled = false;
            return;
        }

        agent.updateRotation = false;

        InvokeRepeating(nameof(UpdateTarget), 0f, updateRate);
    }

    void Update()
    {
        if (currentTarget == null)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(currentTarget.position);
    }

    // ==========================
    // BUSCAR OBJETIVO MAS CERCANO
    // ==========================
    void UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float shortestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);

            if (dist < shortestDistance && dist <= chaseRange)
            {
                shortestDistance = dist;
                closest = p.transform;
            }
        }

        currentTarget = closest;
    }

    // Debug visual
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
