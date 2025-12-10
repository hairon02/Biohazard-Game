using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPlayerItem : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_Text textoNombre;    // Arrastra aquí el Texto del nombre
    public Image imagenEstado;  // Arrastra aquí la Imagen del icono (✔/❌)

    [Header("Recursos Visuales")]
    public Sprite iconoListo;   // Asigna aquí tu sprite de ✔ (Check verde)
    public Sprite iconoNoListo; // Asigna aquí tu sprite de ❌ (Cruz roja/gris)
    public Color colorListo = Color.green;
    public Color colorNoListo = Color.red; // O gris

    // Función para configurar los datos (El Manager llamará a esto)
    public void ConfigurarJugador(string nombre, bool estaListo)
    {
        textoNombre.text = nombre;
        ActualizarEstado(estaListo);
    }

    public void ActualizarEstado(bool isReady)
    {
        if (isReady)
        {
            imagenEstado.sprite = iconoListo;
            imagenEstado.color = colorListo;
        }
        else
        {
            imagenEstado.sprite = iconoNoListo;
            imagenEstado.color = colorNoListo;
        }
    }
}