using UnityEngine;

public class StasisGrenade : MonoBehaviour
{
    [Header("Configuración")]
    public float explosionRadius = 5f;
    // Ya no necesitamos slowFactor ni duration porque los vamos a matar

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // Buscamos la clase padre BaseEnemy
            BaseEnemy target = hit.GetComponent<BaseEnemy>();

            if (target != null)
            {
                // CAMBIO PRINCIPAL: En lugar de ralentizar, aplicamos daño masivo.
                // 9999 es suficiente para matar a cualquier cosa (Insta-Kill)
                Debug.Log($"¡BOOM! Eliminando a {hit.name}");
                target.TakeDamage(9999f); 
            }
        }

        // Efecto visual (Opcional: Si tienes partículas de explosión, instáncialas aquí)
        
        Destroy(gameObject); // Destruye la granada
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Cambié el color a rojo para indicar PELIGRO
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}