using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MenuSystem : MonoBehaviourPunCallbacks
{
    [Header("Paneles del Menú")]
    public GameObject panelPrincipal;
    public GameObject panelSeleccionModo;
    
    [Header("UI Feedback")]
    public Button botonJugar;
    public Text textoEstado; // Arrastra un texto aquí para saber qué pasa

    void Start()
    {
        PhotonNetwork.SendRate = 15; 
        PhotonNetwork.SerializationRate = 15;
        PhotonNetwork.AutomaticallySyncScene = true;
        
        string randomNick = "Agente_" + Random.Range(1000, 9999);
        PhotonNetwork.NickName = randomNick;
        Debug.Log("Tu nombre es: " + randomNick);

        panelPrincipal.SetActive(true);
        panelSeleccionModo.SetActive(false);
        
        // Bloquear botón hasta conectar
        if(botonJugar) botonJugar.interactable = false;
        ActualizarEstado("Conectando al servidor...");

        PhotonNetwork.ConnectUsingSettings();
    }

    // --- CALLBACKS DE CONEXIÓN ---

    public override void OnConnectedToMaster()
    {
        ActualizarEstado("Conectado. Uniéndose al Lobby General...");
        PhotonNetwork.JoinLobby(); // Necesario para ver listas de salas después
    }

    public override void OnJoinedLobby()
    {
        ActualizarEstado("Listo para jugar.");
        if(botonJugar) botonJugar.interactable = true;
    }

    // --- FUNCIONES DE BOTONES ---

    public void IrASeleccionModo()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            panelPrincipal.SetActive(false);
            panelSeleccionModo.SetActive(true);
        }
    }

    public void CrearIncursion() // Botón CREAR SALA
    {
        ActualizarEstado("Creando sala...");
        
        // Configuración de la sala (Máximo 3 jugadores según tu GDD)
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 3;
        options.IsVisible = true;
        options.IsOpen = true;

        // "Sala_" + números aleatorios para evitar nombres duplicados
        PhotonNetwork.JoinOrCreateRoom("sala1", options, TypedLobby.Default);
    }

    public void BuscarFrecuencia() // Botón UNIRSE
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 3;
        options.IsVisible = true;
        options.IsOpen = true;
        ActualizarEstado("Buscando señal de auxilio...");
        PhotonNetwork.JoinOrCreateRoom("sala1", options, TypedLobby.Default);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }

    public void VolverAlMenuPrincipal()
    {
        panelSeleccionModo.SetActive(false);
        panelPrincipal.SetActive(true);
    }

    // --- CALLBACKS DE SALAS (Aquí ocurre la magia) ---

    // Este se activa cuando logras CREAR o UNIRTE a una sala exitosamente
    public override void OnJoinedRoom()
    {
        Debug.Log("¡Entramos a una sala! Cargando escena del Lobby...");
        
        // IMPORTANTE: Usamos PhotonNetwork.LoadLevel para sincronizar,
        // pero como es la escena de selección de personajes, SceneManager local está bien 
        // SIEMPRE QUE ya estemos dentro de la Room.
        
        // Asegúrate que tu escena se llame EXACTAMENTE "Lobby" (o como la tengas)
        PhotonNetwork.LoadLevel("Lobby"); 
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ActualizarEstado("Error al crear: " + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        ActualizarEstado("No hay partidas. Creando una nueva...");
        CrearIncursion(); // Si no encuentra, crea una
    }

    // Función auxiliar para textos
    void ActualizarEstado(string msg)
    {
        if (textoEstado) textoEstado.text = msg;
        Debug.Log(msg);
    }
}