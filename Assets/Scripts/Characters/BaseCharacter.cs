using UnityEngine;
using System.Collections; 

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("Identificación")]
    public string characterName; // <--- ESTA FALTABA

    [Header("Estadísticas de Salud")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Estadísticas de Escudo")]
    public float maxShield = 50f; 
    public float currentShield;

    [Header("Movimiento & Cooldowns")]
    public float movementSpeed = 5f;
    public float abilityCooldown = 10f;
    protected float nextAbilityTime = 0f;
    protected bool isAbilityActive = false;
    
    // --- Inicialización ---
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield; // El escudo empieza lleno
        
        // Usamos characterName si tiene algo, si no usamos el nombre del objeto
        string displayName = string.IsNullOrEmpty(characterName) ? gameObject.name : characterName;
        Debug.Log($"Inicializando: {displayName} | HP: {currentHealth} | Shield: {currentShield}");
    }

    // --- Sistema de Daño con Escudo ---
    public virtual void TakeDamage(float amount, string damageType)
    {
        float damageRemaining = amount;

        // 1. Si tenemos escudo, que absorba el daño primero
        if (currentShield > 0)
        {
            if (currentShield >= damageRemaining)
            {
                // El escudo aguanta todo el golpe
                currentShield -= damageRemaining;
                damageRemaining = 0;
                Debug.Log("¡Escudo absorbió el impacto!");
            }
            else
            {
                // El golpe rompe el escudo y sobra daño
                damageRemaining -= currentShield; 
                currentShield = 0; // Escudo destruido
                Debug.Log("¡Escudo ROTO!");
            }
        }

        // 2. Si todavía queda daño (o no había escudo), va a la salud
        if (damageRemaining > 0)
        {
            currentHealth -= damageRemaining;
            Debug.Log($"{characterName} recibió {damageRemaining} daño real. Vida: {currentHealth}");
        }

        // 3. Chequeo de muerte
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // --- MÉTODOS PARA QUE TU COMPAÑERO LEA LOS DATOS ---
    public float GetHealthFraction()
    {
        return currentHealth / maxHealth;
    }

    public float GetShieldFraction()
    {
        if (maxShield <= 0) return 0;
        return currentShield / maxShield;
    }

    // --- MÉTODOS COMPLEMENTARIOS ---
    protected virtual void Die() 
    { 
        Debug.Log($"{characterName} ha caído.");
        Destroy(gameObject); 
    }

    public bool CanUseAbility() 
    { 
        return Time.time >= nextAbilityTime && !isAbilityActive; 
    }

    protected void StartCooldown() 
    { 
        nextAbilityTime = Time.time + abilityCooldown; 
    }

    public float GetCooldownFraction()
    {
        if (Time.time >= nextAbilityTime) return 0f;
        float remainingTime = nextAbilityTime - Time.time;
        return Mathf.Clamp01(remainingTime / abilityCooldown);
    }

    // Abstractos
    public abstract void ActivateSpecialAbility();
    protected abstract void ApplyPassiveEffect();
}