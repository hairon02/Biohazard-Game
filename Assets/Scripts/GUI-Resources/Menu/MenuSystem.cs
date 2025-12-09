using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelPrincipal; // Arrastra aquí el Panel del Menú Principal
    public GameObject panelSeleccionModo; // Arrastra aquí el Panel de Selección (Jugar/Unir)

    // Se llama al iniciar para asegurar que solo se ve el principal
    void Start()
    {
        panelPrincipal.SetActive(true);
        panelSeleccionModo.SetActive(false);
    }

    // --- Funciones para el Menú Principal ---

    public void IrASeleccionModo() // Asigna esto al botón "JUGAR" del primer menú
    {
        panelPrincipal.SetActive(false);
        panelSeleccionModo.SetActive(true);
    }

    public void SalirDelJuego() // Asigna esto al botón "SALIR" del primer menú
    {
        Debug.Log("Cerrando aplicación...");
        Application.Quit();
    }

    // --- Funciones para el Menú de Selección (Segundo Menú) ---

    public void CrearIncursion() // Botón "CREAR INCURSIÓN"
    {
        // Aquí cargaríamos la escena del Lobby o el Nivel 1
        // Por ahora, usamos el índice +1 como tenías, o el nombre de la escena
        Debug.Log("Creando partida...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

    public void BuscarFrecuencia() // Botón "BUSCAR FRECUENCIA"
    {
        Debug.Log("Abriendo lista de servidores...");
        // Aquí iría tu lógica para abrir el Server Browser
    }

    public void VolverAlMenuPrincipal() // Asigna esto al botón "SALIR" o "VOLVER" del segundo menú
    {
        panelSeleccionModo.SetActive(false);
        panelPrincipal.SetActive(true);
    }
}