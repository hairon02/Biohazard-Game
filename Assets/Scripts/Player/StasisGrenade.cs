using UnityEngine;

public class StasisGrenade : MonoBehaviour
{
    [Header("Configuración")]
    public float explosionRadius = 5f;
    public float slowFactor = 0.5f;
    public float effectDuration = 4f;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // CAMBIO AQUÍ: Ahora buscamos la clase padre 'BaseEnemy'
            // Esto funcionará con Dummies, Zombis, Jefes, etc.
            BaseEnemy target = hit.GetComponent<BaseEnemy>();

            if (target != null)
            {
                target.ApplySlow(slowFactor, effectDuration);
            }
        }

        Debug.Log(">> ¡BOOM! Granada de estasis detonada.");
        Destroy(gameObject);
    }
}