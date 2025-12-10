using UnityEngine;
using UnityEngine.InputSystem;

public class LiamTorres : BaseCharacter
{
    [Header("Habilidad: Hackeo de Puertas")]
    public float interactionRadius = 5f; 
    public LayerMask targetLayers; // Asegúrate de incluir la capa de la puerta

    protected override void Start()
    {
        base.Start();
        characterName = "Liam Torres";
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame && CanUseAbility())
        {
            ActivateSpecialAbility();
            StartCooldown();
        }
    }

    public override void ActivateSpecialAbility()
    {
        // Buscamos objetos en el radio
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, targetLayers);
        
        bool hackedSomething = false;

        foreach (var hit in hits)
        {
            // CAMBIO: Ahora buscamos el script 'HackableObject'
            HackableObject door = hit.GetComponent<HackableObject>();

            if (door != null)
            {
                Debug.Log($">> ¡ACCESO CONCEDIDO! Hackeando: {door.name}");
                
                // Ejecutamos la función de abrir/destruir
                door.OnHack();
                
                hackedSomething = true;
            }
        }

        if (!hackedSomething)
        {
            Debug.Log(">> No hay dispositivos hackeables cerca.");
        }
        else
        {
             DrawDebugVisuals();
        }
    }

    protected override void ApplyPassiveEffect() { }

    private void DrawDebugVisuals()
    {
        Debug.DrawRay(transform.position, Vector3.up * 3, Color.green, 1f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}