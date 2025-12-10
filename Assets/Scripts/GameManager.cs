using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    [Header("Configuración")]
    // IMPORTANTE: El orden debe coincidir con el Lobby (0=Drake, 1=Irina, 2=Liam)
    // Arrastra aquí los prefabs que están dentro de la carpeta Resources
    public GameObject[] prefabsPersonajes; 

    public Transform puntoSpawn; // Arrastra aquí un objeto vacío donde quieres nacer

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnearJugador();
        }
        else
        {
            Debug.LogWarning("Estás jugando offline. Spawneando a Drake por defecto.");
            // Lógica para pruebas offline (opcional)
        }
    }

    void SpawnearJugador()
    {
        // 1. Recuperar el índice guardado en el Lobby
        int indexElegido = 0;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PjIndex"))
        {
            indexElegido = (int)PhotonNetwork.LocalPlayer.CustomProperties["PjIndex"];
        }

        // 2. Validación de seguridad
        if (indexElegido >= prefabsPersonajes.Length || indexElegido < 0) indexElegido = 0;

        GameObject prefabAUsar = prefabsPersonajes[indexElegido];

        // 3. Instanciar a través de la red (Usando el nombre del prefab en Resources)
        GameObject miJugador = PhotonNetwork.Instantiate(prefabAUsar.name, puntoSpawn.position, Quaternion.identity);
        
        Debug.Log($"Personaje creado: {miJugador.name}");
    }
}