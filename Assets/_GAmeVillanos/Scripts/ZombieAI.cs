using UnityEngine;
using UnityEngine.AI; // Importante: Necesario para usar la IA

public class ZombieAI : MonoBehaviour
{
    [Header("Configuración")]
    public string tagDelJugador = "Player"; // A quién busca
    
    private NavMeshAgent agente;
    private Transform objetivo;

    void Start()
    {
        // 1. Obtenemos el cerebro del zombie (el componente Agent)
        agente = GetComponent<NavMeshAgent>();

        // 2. Buscamos al jugador en la escena por su etiqueta (Tag)
        GameObject objJugador = GameObject.FindGameObjectWithTag(tagDelJugador);
        
        if (objJugador != null)
        {
            objetivo = objJugador.transform;
        }
        else
        {
            Debug.LogError("¡El Zombie no encuentra al Jugador! ¿Le pusiste el Tag 'Player'?");
        }
    }

    void Update()
    {
        // 3. Si encontramos al jugador, le decimos al agente que vaya hacia él
        if (objetivo != null)
        {
            agente.SetDestination(objetivo.position);
        }
    }
}   