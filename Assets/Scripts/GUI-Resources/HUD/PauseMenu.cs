using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <--- ¡NECESARIO PARA EL NUEVO SISTEMA!

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelPausa; 

    [Header("Configuración")]
    public string nombreEscenaMenu = "Menu"; 
    private bool estaPausado = false;

    void Update()
    {
        // --- NUEVA FORMA DE DETECTAR LA TECLA ESCAPE ---
        // Verificamos si existe un teclado y si la tecla fue presionada en este frame
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePausa();
        }
    }

    public void TogglePausa()
    {
        estaPausado = !estaPausado;
        
        // 1. Mostrar/Ocultar el panel
        panelPausa.SetActive(estaPausado);

        // 2. Control del Cursor
        if (estaPausado)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SalirAlMenuPrincipal()
    {
        Debug.Log("Saliendo de la partida...");
        
        // Time.timeScale = 1f; // Reactivar tiempo si lo hubieras detenido
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}