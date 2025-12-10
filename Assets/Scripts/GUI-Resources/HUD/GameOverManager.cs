using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance; // Singleton para llamarlo desde el personaje

    [Header("Referencias UI")]
    public GameObject panelGameOver; // Arrastra el "Panel_GameOver" aquí

    [Header("Configuración")]
    public string nombreEscenaMenu = "Menu"; // Asegúrate que tu menú se llame así

    void Awake()
    {
        Instance = this;
    }

    public void MostrarGameOver()
    {
        // 1. Activar el panel visual
        panelGameOver.SetActive(true);

        // 2. Detener el tiempo del juego (Efecto congelado)
        Time.timeScale = 0f;

        // 3. Liberar el cursor para poder dar clic
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Debug.Log("GAME OVER ACTIVADO");
    }

    // --- FUNCIONES PARA LOS BOTONES ---

    public void IrAlMenu()
    {
        // Importante: Reactivar el tiempo antes de cambiar de escena
        Time.timeScale = 1f;

        // ---------------------------------------------------------
        // TODO: PHOTON - TAREA PARA REDES
        // PhotonNetwork.LeaveRoom(); 
        // ---------------------------------------------------------

        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando aplicación...");
        Application.Quit(); // Esto cierra el juego construido (.exe)
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Esto para el editor de Unity
        #endif
    }
}