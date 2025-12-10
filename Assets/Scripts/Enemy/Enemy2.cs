using UnityEngine;
using UnityEngine.AI;
using System.Collections; 

public class Enemy2 : BaseEnemy // <--- CAMBIO 1: Herencia
{
    [Header("Enemy 2 Settings")]
    public float chaseRange = 12f;
    public float updateRate = 0.2f;

    NavMeshAgent agent;
    Transform currentTarget;

    // Variables para controlar estados alterados
    private float originalSpeed;
    private bool isStunned = false;

    // Usamos 'new void Start' o simplemente Start, pero aseguramos inicializar cosas
    void Start()
    {
        // Inicializar stats base si es necesario
        if (health <= 0) health = 100f; 

        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Enemy2: Falta NavMeshAgent");
            enabled = false;
            return;
        }

        // Guardamos la velocidad original para poder restaurarla después de la granada de Irina
        originalSpeed = agent.speed;

        // Lógica original de tu compañero
        agent.updateRotation = false; 

        InvokeRepeating(nameof(UpdateTarget), 0f, updateRate);
    }

    void Update()
    {
        // CAMBIO 2: Válvula de seguridad
        // Si ya se murió o está hackeado, NO se mueve.
        if (health <= 0 || isStunned)
        {
            agent.isStopped = true;
            return;
        }

        if (currentTarget == null)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(currentTarget.position);
    }

    // ====================================================
    // CONEXIÓN CON TUS HABILIDADES (Overrides)
    // ====================================================

    // 1. LIAM: HACKEO
    public override void Stun(float duration)
    {
        // Solo afecta si marcas "Is Mechanical" en el inspector de este enemigo
        if (isMechanical) 
        {
            Debug.Log($"[Enemy2] Unidad Especial Hackeada ({duration}s)");
            StartCoroutine(StunRoutine(duration));
        }
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        yield return new WaitForSeconds(duration);

        isStunned = false;
        Debug.Log("[Enemy2] Sistemas reiniciados.");
    }

    // 2. IRINA: RALENTIZAR
    public override void ApplySlow(float slowFactor, float duration)
    {
        Debug.Log($"[Enemy2] Ralentizado.");
        StartCoroutine(SlowRoutine(slowFactor, duration));
    }

    private IEnumerator SlowRoutine(float factor, float duration)
    {
        agent.speed = originalSpeed * factor;
        yield return new WaitForSeconds(duration);
        agent.speed = originalSpeed;
    }

    // 3. MUERTE (Granada Insta-Kill)
    // No necesitamos escribir TakeDamage aquí porque usa el de BaseEnemy.
    // Pero si quieres que haga algo especial al morir, puedes usar override Die().

    // ==========================
    // LÓGICA DE BÚSQUEDA (Original)
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}