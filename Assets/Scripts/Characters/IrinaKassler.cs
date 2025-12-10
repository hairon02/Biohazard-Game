using UnityEngine;
using UnityEngine.InputSystem;

public class IrinaKessler : BaseCharacter
{
    [Header("Habilidad: Granada de Estasis")]
    public GameObject stasisGrenadePrefab; 
    public Transform throwPoint;
    
    // NUEVO: Variable para controlar la fuerza del brazo
    public float throwForce = 15f; 

    protected override void Start()
    {
        base.Start();
        characterName = "Irina Kessler";
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            ActivateSpecialAbility();
            StartCooldown();
        }
    }

    public override void ActivateSpecialAbility()
    {
        if (stasisGrenadePrefab != null && throwPoint != null)
        {
            // 1. Crear la granada (guardamos la referencia en la variable 'grenade')
            GameObject grenade = Instantiate(stasisGrenadePrefab, throwPoint.position, throwPoint.rotation);
            
            // 2. Obtener su física (Rigidbody)
            Rigidbody rb = grenade.GetComponent<Rigidbody>();

            // 3. Si tiene física, ¡EMPUJARLA!
            if (rb != null)
            {
                // ForceMode.Impulse es ideal para golpes secos o lanzamientos instantáneos
                // throwPoint.forward significa "hacia donde está mirando el punto de lanzamiento"
                rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
            }

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
        }
        base.TakeDamage(finalDamage, damageType);
    }
}