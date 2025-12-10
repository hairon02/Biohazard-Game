using UnityEngine;
using System.Collections;

public abstract class BaseCharacter : MonoBehaviour
{
    [Header("Estadísticas Generales")]
    [SerializeField] protected string characterName;
    //[SerializeField] protected float maxHealth = 100f;
    //[SerializeField] protected float movementSpeed = 5f;
    public float movementSpeed = 5f;
    public float maxHealth = 100f;
    
    // Variables de estado
    protected float currentHealth;
    protected bool isAbilityActive = false;

    // Inicialización
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Inicializando operador: {characterName}");
    }

    // --- MÉTODOS CORE ---

    // Sistema de daño básico
    public virtual void TakeDamage(float amount, string damageType)
    {
        currentHealth -= amount;
        Debug.Log($"{characterName} recibió {amount} de daño. Salud restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{characterName} ha sido incapacitado.");
        // Aquí iría lógica de fin de juego o respawn
    }

    // --- MÉTODOS ABSTRACTOS ---
    
    // Obligamos a las clases hijas a definir su habilidad
    public abstract void ActivateSpecialAbility();
    
    // Obligamos a definir su pasiva
    protected abstract void ApplyPassiveEffect();
}