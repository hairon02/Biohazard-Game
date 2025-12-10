using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage = 15f;
    public string damageType = "Enemy";

    [Header("Knockback Settings")]
    public float knockbackForce = 8f;
    public float hitCooldown = 1f;

    private float lastHitTime;

    void Start()
    {
        // Verificar que tenemos un collider configurado como trigger
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError($"[EnemyAttack] {gameObject.name} NO TIENE COLLIDER! Agrega un collider y marca 'Is Trigger'");
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning($"[EnemyAttack] {gameObject.name} tiene collider pero NO está marcado como Trigger!");
        }
        else
        {
            Debug.Log($"[EnemyAttack] {gameObject.name} configurado correctamente con Trigger");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[EnemyAttack] Trigger detectado con: {other.gameObject.name} (Tag: {other.tag})");

        // Verificar tag del jugador
        if (!other.CompareTag("Player"))
        {
            Debug.Log($"[EnemyAttack] No es un jugador, ignorando...");
            return;
        }

        // Verificar cooldown
        if (Time.time < lastHitTime + hitCooldown)
        {
            Debug.Log($"[EnemyAttack] En cooldown, esperando...");
            return;
        }

        // Buscar componente BaseCharacter
        BaseCharacter character = other.GetComponent<BaseCharacter>();
        if (character == null)
        {
            Debug.LogError($"[EnemyAttack] El jugador {other.name} NO TIENE BaseCharacter!");
            return;
        }

        // Aplicar daño
        Debug.Log($"[EnemyAttack] ¡APLICANDO {damage} de daño a {character.characterName}! Vida antes: {character.currentHealth}");
        character.TakeDamage(damage, damageType);
        Debug.Log($"[EnemyAttack] Vida después: {character.currentHealth}");

        // Aplicar knockback si tiene CharacterController
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
            knockbackDir.y = 0;
            
            // Aplicar knockback instantáneo (sin Time.deltaTime porque OnTriggerEnter solo ocurre una vez)
            controller.Move(knockbackDir * knockbackForce);
            Debug.Log($"[EnemyAttack] Knockback aplicado: {knockbackDir * knockbackForce}");
        }
        else
        {
            Debug.LogWarning($"[EnemyAttack] El jugador no tiene CharacterController, no se puede aplicar knockback");
        }

        lastHitTime = Time.time;
    }

    // Visualizar el collider en el editor
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && col.isTrigger)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            
            if (col is SphereCollider sphere)
            {
                Gizmos.DrawSphere(transform.position + sphere.center, sphere.radius * transform.localScale.x);
            }
            else if (col is BoxCollider box)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(box.center, box.size);
            }
            else if (col is CapsuleCollider capsule)
            {
                Gizmos.DrawWireSphere(transform.position + capsule.center, capsule.radius * transform.localScale.x);
            }
        }
    }
}
