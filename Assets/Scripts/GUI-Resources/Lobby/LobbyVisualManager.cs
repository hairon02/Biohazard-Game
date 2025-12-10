using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;      // Necesario para la red
using Photon.Realtime; // Necesario para los jugadores
using ExitGames.Client.Photon; // Necesario para Hashtable

// Cambiamos a MonoBehaviourPunCallbacks para escuchar eventos de red
public class LobbyVisualManager : MonoBehaviourPunCallbacks
{
    [Header("Referencias UI - Scripts de Personajes")]
    public BotonPersonaje[] scriptsPersonajes; // Arrastra aquí tus 3 botones (Drake, Irina, Liam)

    [Header("Referencias UI - Control")]
    public Button btnListo;
    public Button btnIniciar; // Solo visible para el Host
    public Button btnSalir;
    public TMPro.TMP_Text textoEstado; // Opcional: para mensajes de estado

    [Header("Lista de Jugadores")]
    public GameObject prefabItemJugador; 
    public Transform contenedorLista;    

    // Estado local
    [HideInInspector] public bool estoyListo = false;
    private int miPersonajeIndex = -1;
    private List<GameObject> itemsLista = new List<GameObject>();

    // Claves para sincronización
    const string KEY_PJ = "PjIndex";
    const string KEY_READY = "IsReady";

    void Start()
    {
        // Configuración inicial de botones
        btnListo.onClick.AddListener(OnClickListo);
        btnIniciar.onClick.AddListener(OnClickIniciar);
        btnSalir.onClick.AddListener(OnClickSalir);

        btnListo.interactable = false; // No puedes estar listo sin personaje
        btnIniciar.gameObject.SetActive(false);

        // Limpiar mis propiedades al entrar (por si acaso)
        ResetearMisPropiedades();
        
        // Primera actualización visual
        ActualizarInterfazGeneral();
    }

    // ---------------------------------------------
    // LOGICA 1: SELECCIONAR PERSONAJE (Llamado desde BotonPersonaje)
    // ---------------------------------------------
    public void SeleccionarPersonaje(int indexDeseado)
    {
        if (estoyListo) return; // Si estoy listo, no puedo cambiar

        // Verificar si alguien más ya lo tiene (Doble seguridad)
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p != PhotonNetwork.LocalPlayer && p.CustomProperties.ContainsKey(KEY_PJ))
            {
                int indexOcupado = (int)p.CustomProperties[KEY_PJ];
                if (indexOcupado == indexDeseado)
                {
                    Debug.Log("Ese personaje ya está ocupado.");
                    return;
                }
            }
        }

        // Si está libre, me lo asigno en la red
        miPersonajeIndex = indexDeseado;
        
        Hashtable props = new Hashtable();
        props.Add(KEY_PJ, miPersonajeIndex);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    // ---------------------------------------------
    // LOGICA 2: ESTAR LISTO
    // ---------------------------------------------
    void OnClickListo()
    {
        if (miPersonajeIndex == -1) return;

        estoyListo = !estoyListo; // Alternar (True/False)

        Hashtable props = new Hashtable();
        props.Add(KEY_READY, estoyListo);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    // ---------------------------------------------
    // LOGICA 3: ACTUALIZAR TODA LA UI (Magia de Red)
    // ---------------------------------------------
    void ActualizarInterfazGeneral()
    {
        // A. Limpiar lista visual anterior
        foreach (GameObject g in itemsLista) Destroy(g);
        itemsLista.Clear();

        // Variables para conteo
        HashSet<int> personajesOcupados = new HashSet<int>();
        int jugadoresListos = 0;
        int totalJugadores = PhotonNetwork.PlayerList.Length;

        // B. Recorrer a todos los jugadores conectados
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            // Leer datos de la red
            int pjIndex = -1;
            bool isReady = false;

            if (p.CustomProperties.ContainsKey(KEY_PJ)) 
                pjIndex = (int)p.CustomProperties[KEY_PJ];
            
            if (p.CustomProperties.ContainsKey(KEY_READY)) 
                isReady = (bool)p.CustomProperties[KEY_READY];

            // Guardar qué personajes están tomados
            if (pjIndex != -1) personajesOcupados.Add(pjIndex);
            if (isReady) jugadoresListos++;

            // Crear item en la lista visual (Tu script LobbyPlayerItem)
            GameObject item = Instantiate(prefabItemJugador, contenedorLista);
            item.GetComponent<LobbyPlayerItem>().ConfigurarJugador(p.NickName, isReady);
            itemsLista.Add(item);
        }

        // C. Actualizar los 3 Botones de Personaje (Bloquear/Desbloquear)
        for (int i = 0; i < scriptsPersonajes.Length; i++)
        {
            bool esMio = (i == miPersonajeIndex);
            // Está ocupado si alguien lo tiene Y no soy yo
            bool ocupadoPorOtro = personajesOcupados.Contains(i) && !esMio;

            scriptsPersonajes[i].ActualizarEstadoVisual(esMio, ocupadoPorOtro);
        }

        // D. Control del Botón LISTO
        btnListo.interactable = (miPersonajeIndex != -1);
        ColorBlock colors = btnListo.colors;
        // Visual simple: Verde si estoy listo, Blanco si no
        btnListo.image.color = estoyListo ? Color.green : Color.white; 

        // E. Control del Botón INICIAR (Solo HOST)
        if (PhotonNetwork.IsMasterClient)
        {
            // REGLA: Exactamente 3 Jugadores Y Todos Listos
            // (Para pruebas, puedes cambiar el 3 por 1)
            bool condicionesCumplidas = (totalJugadores == 3 && jugadoresListos == totalJugadores);
            
            btnIniciar.gameObject.SetActive(condicionesCumplidas);
            
            if (textoEstado)
            {
                if (condicionesCumplidas) textoEstado.text = "¡LISTOS PARA LA INCURSIÓN!";
                else textoEstado.text = $"Esperando: {jugadoresListos}/{totalJugadores} Listos (Se requieren 3)";
            }
        }
        else
        {
            btnIniciar.gameObject.SetActive(false);
            if (textoEstado) textoEstado.text = "Esperando al Líder...";
        }
    }

    // ---------------------------------------------
    // LOGICA 4: INICIAR PARTIDA
    // ---------------------------------------------
    void OnClickIniciar()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false; // Nadie más entra
        PhotonNetwork.LoadLevel("Level1"); // ¡Asegúrate del nombre exacto!
    }

    void OnClickSalir()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    // Al salir, volvemos al menú
    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu_Principal");
    }

    // ---------------------------------------------
    // EVENTOS AUTOMATICOS DE PHOTON
    // ---------------------------------------------
    
    // Alguien cambió una propiedad (Eligió personaje o dio listo)
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        ActualizarInterfazGeneral();
    }

    // Alguien entró
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ActualizarInterfazGeneral();
    }

    // Alguien salió
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ActualizarInterfazGeneral();
    }

    // Ayudante para resetear datos
    void ResetearMisPropiedades()
    {
        Hashtable props = new Hashtable();
        props.Add(KEY_PJ, -1);
        props.Add(KEY_READY, false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
}