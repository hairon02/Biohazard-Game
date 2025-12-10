using UnityEngine;
using UnityEngine.InputSystem; // <--- NECESARIO PARA EL NUEVO SISTEMA

public class DrakeWolfe : BaseCharacter
{
    [Header("Habilidad: Fuego de Supresión")]
    public float fireRateBoost = 2.0f;
    public float speedPenalty = 0.5f;
    public float abilityDuration = 5.0f;

    protected override void Start()
    {
        base.Start();
        characterName = "Drake Wolfe";
        ApplyPassiveEffect(); 
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        // CORRECCIÓN: Usamos Keyboard.current en lugar de Input.GetKeyDown
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame && !isAbilityActive)
        {
            ActivateSpecialAbility();
            StartCooldown();
        }
    }

    public override void ActivateSpecialAbility()
    {
        StartCoroutine(SuppressingFireRoutine());
    }

    private System.Collections.IEnumerator SuppressingFireRoutine()
    {
        isAbilityActive = true;
        
        float originalSpeed = movementSpeed;
        
        // Aplicamos penalización de velocidad
        movementSpeed *= speedPenalty;
        Debug.Log(">> ¡Fuego de Supresión ACTIVO! (Movimiento reducido, Cadencia aumentada)");

        yield return new WaitForSeconds(abilityDuration);

        // Restauramos velocidad
        movementSpeed = originalSpeed;
        
        isAbilityActive = false;
        Debug.Log(">> Fuego de Supresión finalizado.");
    }

    protected override void ApplyPassiveEffect()
    {
        // Se calcula en tiempo real
    }

    public float GetReloadMultiplier()
    {
        if (currentHealth <= (maxHealth * 0.3f))
        {
            return 0.5f; 
        }
        return 1.0f; 
    }
}