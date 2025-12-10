using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BotonPersonaje : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Configuración")]
    public Image imagenPersonaje; 
    public int indicePersonaje;   // 0=Drake, 1=Irina, 2=Liam
    
    [Header("Colores")]
    public Color colorInactivo = new Color(0.5f, 0.5f, 0.5f, 1f); // Gris
    public Color colorHover = Color.white;                        // Blanco
    public Color colorSeleccionado = new Color(0.5f, 1f, 0.5f, 1f); // Verde (Es mío)
    public Color colorOcupado = new Color(0.8f, 0.2f, 0.2f, 0.5f);  // Rojo (Ocupado por otro)

    private LobbyVisualManager manager;
    
    // Estados internos
    private bool esMio = false;
    private bool estaOcupadoPorOtro = false;

    void Start()
    {
        manager = FindFirstObjectByType<LobbyVisualManager>();
        ActualizarColor(); // Poner color inicial
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Solo hacemos hover si está libre y no lo tengo yo seleccionado
        if (!esMio && !estaOcupadoPorOtro && !manager.estoyListo)
        {
            imagenPersonaje.color = colorHover;
            transform.localScale = Vector3.one * 1.05f; // Pequeño zoom
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!esMio && !estaOcupadoPorOtro)
        {
            imagenPersonaje.color = colorInactivo;
            transform.localScale = Vector3.one;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si ya estoy listo O el personaje está ocupado por otro, no hacer nada
        if(manager.estoyListo || estaOcupadoPorOtro) return;

        // Avisamos al manager que queremos este personaje
        manager.SeleccionarPersonaje(indicePersonaje);
    }

    // --- ESTA ES LA FUNCIÓN NUEVA QUE USARÁ EL MANAGER ---
    public void ActualizarEstadoVisual(bool esMiSeleccion, bool ocupadoPorAlguienMas)
    {
        esMio = esMiSeleccion;
        estaOcupadoPorOtro = ocupadoPorAlguienMas;

        ActualizarColor();
    }

    void ActualizarColor()
    {
        if (esMio)
        {
            imagenPersonaje.color = colorSeleccionado;
            transform.localScale = Vector3.one * 1.1f; // Un poco más grande si es mío
        }
        else if (estaOcupadoPorOtro)
        {
            imagenPersonaje.color = colorOcupado;
            transform.localScale = Vector3.one; // Tamaño normal
        }
        else
        {
            // Está libre
            imagenPersonaje.color = colorInactivo;
            transform.localScale = Vector3.one;
        }
    }
}