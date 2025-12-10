using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.Q))
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

    protected override void ApplyPassiveEffect()
    {
        // Pasiva aplicada en TakeDamage
    }

    // Pasiva "Inmunidad Parcial"
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