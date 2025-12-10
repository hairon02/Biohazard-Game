using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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

    [Header("Lista de Jugadores")]
    public GameObject prefabItemJugador; // Arrastra aquí tu Prefab "Item_Jugador"
    public Transform contenedorLista;    // Arrastra el objeto "Contenedor_Lista" del Canvas

    // Lista para guardar los items que creamos y poder borrarlos luego
    private List<GameObject> listaItemsActuales = new List<GameObject>();
    
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

        // --- PRUEBA LOCAL DE LA LISTA ---
        List<DatosJugadorSimple> listaPrueba = new List<DatosJugadorSimple>();
        
        listaPrueba.Add(new DatosJugadorSimple { nombre = "Tú (Host)", isReady = false });
        listaPrueba.Add(new DatosJugadorSimple { nombre = "Tramcho", isReady = true });
        listaPrueba.Add(new DatosJugadorSimple { nombre = "Hairon", isReady = false });

        ActualizarListaJugadores(listaPrueba);
        // -------------------------------
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
        // Guardamos el índice localmente para que la siguiente escena lo lea
        // (Esto es temporal hasta que tu compañero ponga Photon, pero funciona perfecto ahora)
        PlayerPrefs.SetInt("MiPersonajeSeleccionado", personajeSeleccionadoIndex);
        PlayerPrefs.Save(); 

        // ... lógica de cargar escena ...
        // ---------------------------------------------------------
        // TODO: PHOTON - TAREA PARA REDES
        // PhotonNetwork.LoadLevel("Nivel1"); // Carga sincronizada
        // ---------------------------------------------------------
        Debug.Log("[RED] HOST: Iniciando partida...");
        Debug.Log("Guardando elección: " + personajeSeleccionadoIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene("pruebaEddy"); // O como se llame tu escena de juego
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

    // --- FUNCIÓN PARA QUE TU COMPAÑERO LLAME DESDE LA RED ---
    // Recibe una lista de objetos/clases con los datos de los jugadores
    // (Aquí uso una clase simple de ejemplo, tu compañero usará Photon.Player)
    public void ActualizarListaJugadores(List<DatosJugadorSimple> listaDatos)
    {
        // 1. Borrar la lista anterior para reconstruirla (forma bruta pero segura)
        foreach (GameObject item in listaItemsActuales)
        {
            Destroy(item);
        }
        listaItemsActuales.Clear();

        // 2. Crear un renglón nuevo por cada jugador conectado
        foreach (var datos in listaDatos)
        {
            GameObject nuevoItem = Instantiate(prefabItemJugador, contenedorLista);
            LobbyPlayerItem scriptItem = nuevoItem.GetComponent<LobbyPlayerItem>();
            
            // Pasamos los datos al renglón
            scriptItem.ConfigurarJugador(datos.nombre, datos.isReady);
            
            // Lo guardamos para poder borrarlo después
            listaItemsActuales.Add(nuevoItem);
        }
    }
    
    // Clase simple para probar (Tu compañero la reemplazará por PhotonPlayer)
    [System.Serializable]
    public class DatosJugadorSimple
    {
        public string nombre;
        public bool isReady;
    }
}