using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoomZone : MonoBehaviour
{
    public int jugadoresNecesarios = 3;

    private HashSet<GameObject> jugadoresDentro = new HashSet<GameObject>();
    private bool escenaCargada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadoresDentro.Add(other.gameObject);
        CheckCondition();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadoresDentro.Remove(other.gameObject);
    }

    private void CheckCondition()
    {
        if (escenaCargada) return;

        if (jugadoresDentro.Count >= jugadoresNecesarios)
        {
            escenaCargada = true;
            CargarSiguienteEscena();
        }
    }

    private void CargarSiguienteEscena()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        int totalEscenas = SceneManager.sceneCountInBuildSettings;

        int siguienteEscena = (escenaActual + 1) % totalEscenas;

        Debug.Log($"Cambiando escena: {escenaActual} → {siguienteEscena}");

        SceneManager.LoadScene(siguienteEscena);
    }
}