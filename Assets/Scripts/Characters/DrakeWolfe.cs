using UnityEngine;
using System.Collections;

public class DrakeWolfe : BaseCharacter
{
    [Header("Habilidad: Fuego de Supresión")]
    public float fireRateBoost = 2.0f; // Doble cadencia
    public float speedPenalty = 0.5f;  // 50% menos velocidad
    public float abilityDuration = 5.0f;

    protected override void Start()
    {
        base.Start();
        characterName = "Drake Wolfe";
        ApplyPassiveEffect(); 
    }

    private void Update()
    {
        // Tecla Q para activar habilidad
        if (Input.GetKeyDown(KeyCode.Q) && !isAbilityActive)
        {
            ActivateSpecialAbility();
        }
    }

    // Implementación de la Activa
    public override void ActivateSpecialAbility()
    {
        StartCoroutine(SuppressingFireRoutine());
    }

    private IEnumerator SuppressingFireRoutine()
    {
        isAbilityActive = true;
        
        float originalSpeed = movementSpeed;
        
        // Efecto: Más lento pero dispara más rápido (lógica visual/debug por ahora)
        movementSpeed *= speedPenalty;
        Debug.Log(">> ¡Fuego de Supresión ACTIVO! (Movimiento reducido, Cadencia aumentada)");

        yield return new WaitForSeconds(abilityDuration);

        // Revertir
        movementSpeed = originalSpeed;
        
        isAbilityActive = false;
        Debug.Log(">> Fuego de Supresión finalizado.");
    }

    protected override void ApplyPassiveEffect()
    {
        // La pasiva se calcula dinámicamente en GetReloadMultiplier
    }

    // Pasiva "Veterano"
    public float GetReloadMultiplier()
    {
        if (currentHealth <= (maxHealth * 0.3f))
        {
            return 0.5f; // Recarga rápida
        }
        return 1.0f; 
    }
}