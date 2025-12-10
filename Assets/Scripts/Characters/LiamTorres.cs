using UnityEngine;

public class LiamTorres : BaseCharacter
{
    [Header("Habilidad: Sobrecarga de Sistema")]
    [Tooltip("Radio de efecto del pulso electromagnético")]
    public float overloadRadius = 10f;
    
    [Tooltip("Capas de objetos que serán afectados (ej. EnemigosMecanicos, Trampas)")]
    public LayerMask affectedLayers; 

    protected override void Start()
    {
        base.Start();
        characterName = "Liam Torres";
    }

    private void Update()
    {
        // Tecla Q para activar habilidad
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSpecialAbility();
        }
    }

    // Implementación de la Activa: "Sobrecarga de Sistema"
    public override void ActivateSpecialAbility()
    {
        // Crea una esfera invisible y detecta todo lo que toque en las capas elegidas
        Collider[] hits = Physics.OverlapSphere(transform.position, overloadRadius, affectedLayers);
        
        if (hits.Length > 0)
        {
            Debug.Log($">> ¡Sobrecarga de Sistema activada! Objetos afectados: {hits.Length}");
            
            foreach (var hit in hits)
            {
                // Aquí, en el futuro, accederemos al script del enemigo para llamar su método "Stun()"
                // Por ahora, solo mostramos a quién le dimos.
                Debug.Log($"   -> Aturdiendo sistema de: {hit.name}");
            }
        }
        else
        {
            Debug.Log(">> Sobrecarga fallida: No hay objetivos mecánicos cerca.");
        }

        // Efecto visual (Debug)
        DrawDebugSphere();
    }

    // Implementación de la Pasiva: "Hacker"
    protected override void ApplyPassiveEffect()
    {
        // Esta pasiva es de utilidad, no de combate directo.
        // Se usará cuando el personaje intente abrir una puerta o consola.
    }

    // Método auxiliar que llamará tu sistema de interacción
    public float GetInteractionSpeed()
    {
        Debug.Log("Pasiva Hacker: Interactuando al 200% de velocidad.");
        return 2.0f; // Liam interactúa el doble de rápido
    }

    // Dibuja la esfera en el editor para que veas el rango
    private void DrawDebugSphere()
    {
        // Solo visible en la ventana "Scene" de Unity por un momento
        Debug.DrawRay(transform.position, Vector3.up * 5, Color.cyan, 2f);
    }
    
    // Dibuja el radio siempre en el editor (Gizmo)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, overloadRadius);
    }
}