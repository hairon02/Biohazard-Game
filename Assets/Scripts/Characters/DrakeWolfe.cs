using UnityEngine;
using UnityEngine.InputSystem;

public class DrakeWolfe : BaseCharacter
{
    [Header("Habilidad: Granada Flashbang")]
    public float stunRadius = 8f;     // Radio de explosión
    public float stunDuration = 4f;   // Tiempo que se quedan quietos
    public LayerMask affectedLayers;  // Capas afectadas (Enemigos)
    
    // Opcional: Efecto visual
    public GameObject flashEffectPrefab; 

    protected override void Start()
    {
        base.Start();
        characterName = "Drake Wolfe";
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
        // 1. Detectar enemigos en radio
        Collider[] hits = Physics.OverlapSphere(transform.position, stunRadius, affectedLayers);
        
        if (hits.Length > 0)
        {
            Debug.Log($">> ¡Flashbang detonada! Enemigos afectados: {hits.Length}");
            
            foreach (var hit in hits)
            {
                BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    // TRUCO: Usamos ApplySlow con factor 0 para detenerlos totalmente.
                    // Esto funciona tanto en Zombis como en Robots.
                    enemy.ApplySlow(0f, stunDuration); 
                    Debug.Log($"   -> Aturdiendo a: {hit.name}");
                }
            }
        }
        else
        {
            Debug.Log(">> Flashbang fallida: No hay enemigos cerca.");
        }

        DrawDebugSphere();
    }

    protected override void ApplyPassiveEffect() { } // La pasiva de recarga sigue en el arma

    private void DrawDebugSphere()
    {
        // Visualización rápida en Scene
        Debug.DrawRay(transform.position, Vector3.up * 5, Color.white, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
    }
}