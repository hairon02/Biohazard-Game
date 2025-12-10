using UnityEngine;

// Esta será la plantilla para TODOS los enemigos del juego
public class BaseEnemy : MonoBehaviour
{
    [Header("Stats Base")]
    public float health = 100f;
    public bool isMechanical = false; // Liam necesita saber esto

    // Virtual permite que el Zombi real o el Jefe cambien cómo reciben daño
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    public virtual void ApplySlow(float slowFactor, float duration)
    {
        // Lógica base (o vacía si quieres que cada uno la defina)
        Debug.Log($"{gameObject.name} ralentizado.");
    }

    public virtual void Stun(float duration)
    {
        if (isMechanical)
        {
            Debug.Log($"{gameObject.name} aturdido.");
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}