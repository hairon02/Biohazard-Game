using UnityEngine;
using UnityEngine.InputSystem; // <--- AGREGADO

public class IrinaKessler : BaseCharacter
{
    [Header("Habilidad: Granada de Estasis")]
    public GameObject stasisGrenadePrefab; 
    public Transform throwPoint;           

    protected override void Start()
    {
        base.Start();
        characterName = "Irina Kessler";
    }

    private void Update()
    {
        // CORRECCIÓN: Nuevo Input System
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            ActivateSpecialAbility();
        }
    }

    public override void ActivateSpecialAbility()
    {
        if (stasisGrenadePrefab != null && throwPoint != null)
        {
            Instantiate(stasisGrenadePrefab, throwPoint.position, throwPoint.rotation);
            Debug.Log(">> ¡Granada de Estasis lanzada!");
        }
        else
        {
            Debug.LogWarning("Falta asignar Prefab o ThrowPoint en Irina.");
        }
    }

    protected override void ApplyPassiveEffect() { }

    public override void TakeDamage(float amount, string damageType)
    {
        float finalDamage = amount;
        if (damageType == "Acido" || damageType == "Gas")
        {
            finalDamage = amount * 0.7f; 
            Debug.Log("Pasiva Bioingeniera: Resistencia al entorno aplicada.");
        }
        base.TakeDamage(finalDamage, damageType);
    }
}