using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbyVisualManager : MonoBehaviour
{
    [Header("Referencias UI - Scripts de Personajes")]
    public BotonPersonaje[] scriptsPersonajes; 

    [Header("Referencias UI - Control")]
    public Button btnListo;
    public Button btnIniciar; // Solo Host
    public Button btnSalir;

    [Header("Colores Botón Listo")]
    public Color listoGris = new Color(0.5f, 0.5f, 0.5f, 1f);
    public Color listoHover = Color.white;
    public Color listoActivo = new Color(0.5f, 1f, 0.5f, 1f); // Verde claro

    [Header("Estado")]
    public int personajeSeleccionadoIndex = -1;
    public bool estoyListo = false;
    
    // VARIABLE DE DEBUG: Marca esto como TRUE en el inspector para probar el botón iniciar
    public bool simularSerHost = true; 

    void Start()
    {
        // --- CONFIGURACIÓN BOTÓN LISTO (Eventos Hover Manuales) ---
        btnListo.transition = Selectable.Transition.None; 
        EventTrigger trigger = btnListo.gameObject.AddComponent<EventTrigger>();
        
        // Hover Enter
        var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) => OnListoHover(true));
        trigger.triggers.Add(entryEnter);

        // Hover Exit
        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) => OnListoHover(false));
        trigger.triggers.Add(entryExit);

        // Clicks
        btnListo.onClick.AddListener(OnBtnListoClick);
        btnIniciar.onClick.AddListener(OnBtnIniciarClick);
        btnSalir.onClick.AddListener(OnBtnSalirClick);
        
        // Estado inicial
        btnListo.image.color = listoGris;
        btnIniciar.gameObject.SetActive(false); 
    }

    // --- LÓGICA DE PERSONAJES ---

    public void SeleccionarPersonaje(int index)
    {
        // Si ya estoy listo, no puedo cambiar de personaje
        if (estoyListo) return;

        personajeSeleccionadoIndex = index;

        // Actualización Visual Local (Solo marca el mío)
        for (int i = 0; i < scriptsPersonajes.Length; i++)
        {
            scriptsPersonajes[i].ConfigurarSeleccion(i == index);
        }

        // ---------------------------------------------------------
        // TODO: PHOTON - TAREA PARA REDES
        // 1. Guardar en las "CustomProperties" del Jugador Local el index de su personaje.
        //    Ej: PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Personaje", index } });
        // 2. No hace falta validar si está ocupado, porque se permiten repetidos.
        // ---------------------------------------------------------
        Debug.Log($"[RED] Notificando selección de personaje: {index}");
    }

    // --- LÓGICA BOTÓN LISTO ---

    void OnListoHover(bool isEntering)
    {
        if (estoyListo) return;

        if (isEntering)
            // Si hay personaje seleccionado = Blanco, si no = Rojo/Gris
            btnListo.image.color = (personajeSeleccionadoIndex != -1) ? listoHover : new Color(1, 0.5f, 0.5f); 
        else
            btnListo.image.color = listoGris;
    }

    void OnBtnListoClick()
    {
        if (personajeSeleccionadoIndex == -1) return; // Obligatorio elegir personaje

        estoyListo = !estoyListo; // Alternar estado

        // Actualización Visual Local
        if (estoyListo)
        {
            btnListo.image.color = listoActivo;
        }
        else
        {
            btnListo.image.color = listoHover;
        }

        // ---------------------------------------------------------
        // TODO: PHOTON - TAREA PARA REDES
        // 1. Enviar RPC o Property: "Mi estado Ready cambió a: " + estoyListo
        // 2. Si soy MasterClient, verificar si TODOS están listos para habilitar btnIniciar.
        // ---------------------------------------------------------
        Debug.Log($"[RED] Notificando estado LISTO: {estoyListo}");

        // --- SIMULACIÓN PARA PROBAR VISUALES SIN RED ---
        if (simularSerHost && estoyListo)
        {
            btnIniciar.gameObject.SetActive(true); 
        }
        else if (simularSerHost && !estoyListo)
        {
            btnIniciar.gameObject.SetActive(false);
        }
        // -----------------------------------------------
    }

    // --- LÓGICA DE INICIAR / SALIR ---

    void OnBtnIniciarClick()
    {
        // ---------------------------------------------------------
        // TODO: PHOTON - TAREA PARA REDES
        // PhotonNetwork.LoadLevel("Nivel1"); // Carga sincronizada
        // ---------------------------------------------------------
        Debug.Log("[RED] HOST: Iniciando partida...");
    }

    void OnBtnSalirClick()
    {
        // ---------------------------------------------------------
        // TODO: PHOTON - TAREA PARA REDES
        // PhotonNetwork.LeaveRoom();
        // ---------------------------------------------------------
        Debug.Log("[RED] Desconectando...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}