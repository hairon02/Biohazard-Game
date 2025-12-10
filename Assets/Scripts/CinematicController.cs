using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem; // <--- AGREGADO: Librería del nuevo sistema

public class CinematicController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "MainMenu"; // Asegúrate de escribir bien el nombre de tu escena de juego

    void Start()
    {
        if (videoPlayer == null) 
            videoPlayer = GetComponent<VideoPlayer>();

        // Suscribirse al evento "cuando termine el video"
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        // CORRECCIÓN: Usamos Keyboard.current para detectar teclas con el nuevo sistema
        if (Keyboard.current != null)
        {
            // Si presiona ESPACIO o ENTER -> Saltar video
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Debug.Log("Cinemática saltada por el jugador.");
                LoadNextScene();
            }
        }
        
        // OPCIONAL: Si quieres permitir saltar con clic del mouse también:
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
             LoadNextScene();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video terminado.");
        LoadNextScene();
    }

    void LoadNextScene()
    {
        // Evitamos cargar la escena dos veces si el usuario spamea el botón
        videoPlayer.loopPointReached -= OnVideoEnd; 
        SceneManager.LoadScene(nextSceneName);
    }
}