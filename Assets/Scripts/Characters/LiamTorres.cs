using UnityEngine;
using UnityEngine.InputSystem;

public class LiamTorres : BaseCharacter
{
    [Header("Habilidad: Desmantelar Sistema")]
    public float interactionRadius = 5f; // Rango corto (cuerpo a cuerpo técnico)
    public LayerMask targetLayers;       // Debería ser 'MechanicalEnemy'

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
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius, targetLayers);
        
        bool hitSomething = false;

        foreach (var hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();

            // CONDICIÓN: Solo afecta si es mecánico
            if (enemy != null && enemy.isMechanical)
            {
                Debug.Log($">> ¡OBJETIVO ELIMINADO! Desmantelando: {enemy.name}");
                
                // Daño infinito para asegurar destrucción inmediata
                enemy.TakeDamage(9999f); 
                
                // Opcional: Si quieres que desaparezca sin animación de muerte, usa:
                // Destroy(enemy.gameObject);
                
                hitSomething = true;
            }
        }

        if (!hitSomething)
        {
            Debug.Log(">> Habilidad fallida: No hay sistemas mecánicos válidos cerca.");
        }
        else
        {
             DrawDebugVisuals();
        }
    }

    protected override void ApplyPassiveEffect() { }

    private void DrawDebugVisuals()
    {
        Debug.DrawRay(transform.position, Vector3.up * 3, Color.cyan, 1f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}