using UnityEngine;
using System.Collections.Generic;

public class SquadHUDManager : MonoBehaviour
{
    public GameObject prefabMiembroEscuadron; // Arrastra el prefab SquadMemberUI
    public Transform contenedorLista;         // Arrastra el objeto con VerticalLayoutGroup

    // Diccionario para buscar rápido la barra de un jugador por su ID de Photon
    private Dictionary<int, SquadMemberItem> listaMiembros = new Dictionary<int, SquadMemberItem>();

    // ---------------------------------------------------------
    // TODO: PHOTON - TAREA PARA REDES
    // 1. Suscribirse a OnPlayerEnteredRoom para llamar a CrearItemParaJugador()
    // 2. Suscribirse a OnPlayerLeftRoom para llamar a EliminarItem()
    // 3. Crear función RPC que reciba (int actorID, float nuevaVida) y llame a ActualizarVidaCompañero()
    // ---------------------------------------------------------

    public void CrearItemParaJugador(int id, string nombre)
    {
        GameObject nuevoItem = Instantiate(prefabMiembroEscuadron, contenedorLista);
        SquadMemberItem script = nuevoItem.GetComponent<SquadMemberItem>();
        script.ActualizarInfo(nombre, 1.0f); 
        listaMiembros.Add(id, script);
    }

    public void ActualizarVidaCompañero(int id, float porcentajeVida)
    {
        if (listaMiembros.ContainsKey(id))
            listaMiembros[id].ActualizarInfo("", porcentajeVida);
    }

    public void EliminarItem(int id)
    {
        if (listaMiembros.ContainsKey(id))
        {
            Destroy(listaMiembros[id].gameObject);
            listaMiembros.Remove(id);
        }
    }
}