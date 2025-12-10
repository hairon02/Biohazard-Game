using UnityEngine;

public class StasisGrenade : MonoBehaviour
{
    [Header("Configuración de Estasis")]
    public float explosionRadius = 5f;
    public float stasisDuration = 4f;
    public GameObject explosionEffect; // Opcional: Partículas

    private void OnCollisionEnter(Collision collision)
    {
        // Al tocar cualquier cosa, explota
        Explode();
    }

    void Explode()
    {
        // 1. Efecto visual (si tienes partículas)
        if (explosionEffect != null) 
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 2. Detectar enemigos en el área
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider hit in colliders)
        {
            // Buscamos si el objeto tiene el script "EnemyDummy" (que haremos abajo)
            EnemyDummy enemy = hit.GetComponent<EnemyDummy>();
            
            if (enemy != null)
            {
                enemy.ApplySlow(0.5f, stasisDuration); // Ralentiza al 50%
                Debug.Log($"Enemigo {hit.name} ralentizado por estasis.");
            }
        }

        // 3. Destruir la granada
        Destroy(gameObject);
    }
    
    // Dibujo para ver el radio en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}