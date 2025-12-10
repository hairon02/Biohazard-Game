using UnityEngine;
using UnityEngine.InputSystem; // <--- AGREGADO

public class LiamTorres : BaseCharacter
{
    [Header("Habilidad: Sobrecarga de Sistema")]
    public float overloadRadius = 10f;
    public LayerMask affectedLayers; 

    protected override void Start()
    {
        base.Start();
        characterName = "Liam Torres";
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
        Collider[] hits = Physics.OverlapSphere(transform.position, overloadRadius, affectedLayers);
        
        if (hits.Length > 0)
        {
            Debug.Log($">> ¡Sobrecarga de Sistema activada! Objetos afectados: {hits.Length}");
            foreach (var hit in hits)
            {
                Debug.Log($"   -> Aturdiendo sistema de: {hit.name}");
                // Si tienes el script EnemyDummy, descomenta esto:
                // var enemy = hit.GetComponent<EnemyDummy>();
                // if(enemy) enemy.Stun(3f);
            }
        }
        else
        {
            Debug.Log(">> Sobrecarga fallida: No hay objetivos mecánicos cerca.");
        }
        DrawDebugSphere();
    }

    protected override void ApplyPassiveEffect() { }

    public float GetInteractionSpeed()
    {
        return 2.0f;
    }

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