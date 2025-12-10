using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Necesario para Corrutinas

// CAMBIO 1: Heredamos de BaseEnemy para que las granadas y hackeos funcionen
public class EnemyAI : BaseEnemy
{
    [Header("IA Settings")]
    public float chaseRange = 12f;
    public float updateRate = 0.2f;

    NavMeshAgent agent;
    Transform currentTarget;
    public Animator animator;

    // Variables internas para manejar estados
    private float originalSpeed;
    private bool isStunned = false;

    // Usamos 'override' porque BaseEnemy ya tiene un Start virtual
    protected void Start()
    {
        // Inicializamos cosas de la clase padre (Vida, etc)
        health = 100f; // O la vida que quieras
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("EnemyAI: Falta NavMeshAgent");
            enabled = false;
            return;
        }

        originalSpeed = agent.speed;

        // Buscar objetivo repetidamente (optimización)
        InvokeRepeating(nameof(UpdateTarget), 0f, updateRate);
    }

    void Update()
    {
        // CAMBIO 2: Si está aturdido o muerto, no hacemos nada
        if (health <= 0 || isStunned) 
        {
            agent.isStopped = true;
            if (animator) animator.SetBool("run", false);
            return;
        }

        if (currentTarget == null)
        {
            agent.isStopped = true;
            if (animator) animator.SetBool("run", false);
            return;
        }

        // Persecución normal
        agent.isStopped = false;
        if (animator) animator.SetBool("run", true);
        agent.SetDestination(currentTarget.position);
    }

    // ====================================================
    // CONEXIÓN CON TUS HABILIDADES (La "Fusión")
    // ====================================================

    // 1. LIAM: HACKEO / STUN
    public override void Stun(float duration)
    {
        if (isMechanical) // Solo si marcaste "Is Mechanical" en el inspector
        {
            Debug.Log($"[EnemyAI] Sistema hackeado por {duration}s");
            StartCoroutine(StunRoutine(duration));
        }
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true; // Bloquea el Update
        agent.isStopped = true; // Frena el agente
        agent.velocity = Vector3.zero;
        
        // Opcional: Feedback visual o animación
        if (animator) animator.speed = 0; // Congela la animación

        yield return new WaitForSeconds(duration);

        isStunned = false;
        if (animator) animator.speed = 1; // Restaura animación
        Debug.Log("[EnemyAI] Sistema reiniciado.");
    }

    // 2. IRINA: RALENTIZAR
    public override void ApplySlow(float slowFactor, float duration)
    {
        Debug.Log($"[EnemyAI] Ralentizado al {(1-slowFactor)*100}%");
        StartCoroutine(SlowRoutine(slowFactor, duration));
    }

    private IEnumerator SlowRoutine(float factor, float duration)
    {
        agent.speed = originalSpeed * factor;
        yield return new WaitForSeconds(duration);
        agent.speed = originalSpeed;
    }

    // 3. DRAKE / GENERAL: DAÑO
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount); // Resta vida usando la lógica de BaseEnemy
        
        // Aquí podrías agregar animación de "Hit"
        // animator.SetTrigger("hit");
    }

    protected override void Die()
    {
        Debug.Log("EnemyAI: Muerte ejecutada.");
        agent.isStopped = true;
        enabled = false; // Apagamos este script
        
        // Si tienes animación de muerte:
        // animator.SetTrigger("die");
        
        base.Die(); // Destruye el objeto (o lo que tengas en BaseEnemy)
    }

    // ==========================
    // LÓGICA DE BÚSQUEDA (Original de tu compañero)
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