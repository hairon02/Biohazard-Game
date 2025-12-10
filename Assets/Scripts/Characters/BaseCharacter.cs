using UnityEngine;
using System.Collections; 
using Photon.Pun; // <--- NECESARIO

public abstract class BaseCharacter : MonoBehaviourPun
{
    [Header("Identificación")]
    public string characterName;

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
    
    // Variable para saber si este personaje es EL MÍO (el que controla el jugador)
    // Cuando pongas Photon, esto será: photonView.IsMine
    public bool esJugadorLocal => photonView.IsMine; // <--- NUEVO: Por defecto true para pruebas offline

    // --- Inicialización ---
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield; 

        if (!photonView.IsMine)
        {
            // Si este no es mi personaje, desactivamos este script para no procesar lógica
            // (Nota: Esto evita que TÚ ejecutes cooldowns o lógica en el clon de tu amigo)
            // Sin embargo, para habilidades visuales, necesitaremos RPCs más adelante.
            // Por ahora, esto evita conflictos.
            return;
        }
        
        string displayName = string.IsNullOrEmpty(characterName) ? gameObject.name : characterName;
        Debug.Log($"Inicializando: {displayName} | HP: {currentHealth} | Shield: {currentShield}");

        // <--- NUEVO: Si soy yo, actualizo mi HUD al iniciar la partida
        if (esJugadorLocal && HUDManager.Instance != null)
        {
            HUDManager.Instance.ActualizarSalud(currentHealth, maxHealth);
            HUDManager.Instance.ActualizarEscudo(currentShield, maxShield);
            // Actualizar munición aquí también si la tuvieras en este script
        }
    }

    // --- Sistema de Daño con Escudo ---
    public virtual void TakeDamage(float amount, string damageType)
    {
        float damageRemaining = amount;

        // 1. Lógica de Escudo
        if (currentShield > 0)
        {
            if (currentShield >= damageRemaining)
            {
                currentShield -= damageRemaining;
                damageRemaining = 0;
                Debug.Log("¡Escudo absorbió el impacto!");
            }
            else
            {
                damageRemaining -= currentShield; 
                currentShield = 0; 
                Debug.Log("¡Escudo ROTO!");
            }
        }

        // 2. Lógica de Salud
        if (damageRemaining > 0)
        {
            currentHealth -= damageRemaining;
            Debug.Log($"{characterName} recibió {damageRemaining} daño real. Vida: {currentHealth}");

            // <--- NUEVO: Feedback visual de daño (Vignette roja)
            if (esJugadorLocal && HUDManager.Instance != null)
            {
                HUDManager.Instance.EfectoRecibirDaño();
            }
        }

        // <--- NUEVO: ¡AVISAR AL HUD QUE LOS VALORES CAMBIARON! ---
        if (esJugadorLocal && HUDManager.Instance != null)
        {
            HUDManager.Instance.ActualizarSalud(currentHealth, maxHealth);
            HUDManager.Instance.ActualizarEscudo(currentShield, maxShield);
        }
        // --------------------------------------------------------

        // 3. Chequeo de muerte
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // --- MÉTODOS PARA COMPAÑEROS ---
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
        
        // <--- NUEVO: Avisar al HUD que empiece la animación de cooldown
        if (esJugadorLocal && HUDManager.Instance != null)
        {
            HUDManager.Instance.IniciarCooldownHabilidad(abilityCooldown);
        }
    }

    public float GetCooldownFraction()
    {
        if (Time.time >= nextAbilityTime) return 0f;
        float remainingTime = nextAbilityTime - Time.time;
        return Mathf.Clamp01(remainingTime / abilityCooldown);
    }

    public abstract void ActivateSpecialAbility();
    protected abstract void ApplyPassiveEffect();
}