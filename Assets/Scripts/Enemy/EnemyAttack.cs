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
    private BaseEnemy myStats; // Referencia a su propia vida

    void Start()
    {
        // Obtenemos referencia a nuestra propia salud/IA
        myStats = GetComponent<BaseEnemy>();
        
        Collider col = GetComponent<Collider>();
        if (col == null || !col.isTrigger)
            Debug.LogWarning($"[EnemyAttack] {gameObject.name} necesita un Collider Trigger.");
    }

    private void OnTriggerEnter(Collider other)
    {
        // SEGURIDAD: Si yo (el enemigo) estoy muerto, no puedo atacar
        if (myStats != null && myStats.health <= 0) return;

        // Verificar tag del jugador
        if (!other.CompareTag("Player")) return;

        // Verificar cooldown
        if (Time.time < lastHitTime + hitCooldown) return;

        // Buscar componente BaseCharacter en el JUGADOR
        BaseCharacter character = other.GetComponent<BaseCharacter>();
        
        if (character != null)
        {
            Debug.Log($"[EnemyAttack] Golpeando a {character.characterName}");
            character.TakeDamage(damage, damageType);
            
            // Aplicar knockback
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
                knockbackDir.y = 0;
                controller.Move(knockbackDir * knockbackForce);
            }

            lastHitTime = Time.time;
        }
    }
}