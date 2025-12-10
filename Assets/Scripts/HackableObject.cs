using UnityEngine;

public class HackableObject : MonoBehaviour
{
    // Opcional: Si quieres que tarde un poco en desaparecer o suene algo
    public void OnHack()
    {
        Debug.Log($">> Hackeo exitoso: Abriendo {gameObject.name}");
        
        // Aquí podrías poner una animación de abrir puerta:
        // GetComponent<Animator>().SetTrigger("Open");
        
        // Pero como pediste que desaparezca:
        Destroy(gameObject); 
    }
}