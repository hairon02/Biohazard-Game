using UnityEngine;
using System.Collections;

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("Estadísticas Generales")]
    public string characterName;
    public float maxHealth = 100f;
    public float movementSpeed = 5f;

    [Header("Cooldowns")]
    public float abilityCooldown = 10f; // Tiempo de espera en segundos
    protected float nextAbilityTime = 0f; // Cuándo podré usarla de nuevo

    protected float currentHealth;
    protected bool isAbilityActive = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float amount, string damageType)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    protected virtual void Die() { Debug.Log($"{characterName} ha caído."); }

    // --- LÓGICA DE COOLDOWN ---
    // Este método verifica si podemos usar la habilidad
    public bool CanUseAbility()
    {
        if (Time.time >= nextAbilityTime && !isAbilityActive)
        {
            return true;
        }
        else
        {
            // Opcional: Avisar al jugador
            // Debug.Log($"Habilidad en enfriamiento. Espera {Mathf.Ceil(nextAbilityTime - Time.time)}s");
            return false;
        }
    }

    // Este método inicia el contador de espera
    protected void StartCooldown()
    {
        nextAbilityTime = Time.time + abilityCooldown;
    }

    public abstract void ActivateSpecialAbility();
    protected abstract void ApplyPassiveEffect();
}