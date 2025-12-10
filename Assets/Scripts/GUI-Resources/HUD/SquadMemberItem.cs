using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquadMemberItem : MonoBehaviour
{
    public TMP_Text nombreJugador;
    public Slider sliderSalud;

    public void ActualizarInfo(string nombre, float vidaPorcentaje)
    {
        if(!string.IsNullOrEmpty(nombre)) nombreJugador.text = nombre;
        sliderSalud.value = vidaPorcentaje;
    }
}