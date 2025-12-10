using UnityEngine;
using UnityEngine.InputSystem;

public class LiamTorres : BaseCharacter
{
    [Header("Habilidad: Sobrecarga de Sistema")]
    public float overloadRadius = 10f;
    public float stunDuration = 5f; // Cuánto tiempo se quedan quietos
    public LayerMask affectedLayers; 

    protected override void Start()
    {
        base.Start();
        characterName = "Liam Torres";
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame && CanUseAbility())
        {
            ActivateSpecialAbility();
            StartCooldown();
        }
    }

    public override void ActivateSpecialAbility()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, overloadRadius, affectedLayers);
        
        if (hits.Length > 0)
        {
            Debug.Log($">> ¡Sobrecarga de Sistema activada! Objetos afectados: {hits.Length}");
            
            foreach (var hit in hits)
            {
                // 1. Buscamos si el objeto golpeado tiene el script de enemigo
                BaseEnemy enemy = hit.GetComponent<BaseEnemy>();

                // 2. Si existe el script, ejecutamos el Stun
                if (enemy != null)
                {
                    Debug.Log($"   -> Enviando orden de hackeo a: {hit.name}");
                    enemy.Stun(stunDuration); // <--- ¡AQUÍ ESTÁ LA MAGIA!
                }
            }
        }
        else
        {
            Debug.Log(">> Sobrecarga fallida: No hay objetivos mecánicos cerca.");
        }

        DrawDebugSphere();
    }

    protected override void ApplyPassiveEffect() { }

    public float GetInteractionSpeed() { return 2.0f; }

    private void DrawDebugSphere()
    {
        Debug.DrawRay(transform.position, Vector3.up * 5, Color.cyan, 2f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, overloadRadius);
    }
}