using UnityEngine;
using System.Collections;

public class EnemyDummy : MonoBehaviour
{
    [Header("Estado del Enemigo")]
    public float health = 100f;
    public bool isMechanical = false; // Marcar TRUE para probar a LIAM

    // Método para recibir daño (Drake)
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} daño. Vida: {health}");
        if (health <= 0) Destroy(gameObject);
    }

    // Método para ser aturdido (Liam)
    public void Stun(float duration)
    {
        if (isMechanical)
        {
            Debug.Log($"<color=cyan>¡{gameObject.name} (Mecánico) ha sido ATURDIDO por {duration}s!</color>");
            // Aquí iría la lógica de detener su IA
            StartCoroutine(RecoverFromStun(duration));
        }
    }

    // Método para ser ralentizado (Irina)
    public void ApplySlow(float slowFactor, float duration)
    {
        Debug.Log($"<color=green>{gameObject.name} ralentizado un {(1-slowFactor)*100}% por Estasis.</color>");
        // Aquí reducirías su NavMeshAgent speed
    }

    private IEnumerator RecoverFromStun(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log($"{gameObject.name} se ha recuperado del aturdimiento.");
    }
}