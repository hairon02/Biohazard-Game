using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BotonPersonaje : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Configuración")]
    public Image imagenPersonaje; // Arrastra la imagen del personaje aquí
    public int indicePersonaje;   // 0=Drake, 1=Irina, 2=Liam
    
    [Header("Colores")]
    public Color colorNormal = new Color(0.5f, 0.5f, 0.5f, 1f); // Gris (Inactivo)
    public Color colorOriginal = Color.white; // Blanco (Activo/Hover)

    private LobbyVisualManager manager;
    private bool estaSeleccionado = false;

    void Start()
    {
        manager = FindFirstObjectByType<LobbyVisualManager>();
        
        // Estado inicial: Apagado (Gris)
        imagenPersonaje.color = colorNormal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Solo hacemos efecto hover si NO está seleccionado y NO hemos dado a "Listo"
        if (!estaSeleccionado && !manager.estoyListo)
        {
            imagenPersonaje.color = colorOriginal;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Si no está seleccionado, vuelve a gris al salir el mouse
        if (!estaSeleccionado)
        {
            imagenPersonaje.color = colorNormal;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si ya estamos listos, no hacemos nada (bloqueado)
        if(manager.estoyListo) return;

        // ---------------------------------------------------------
        // TODO: PHOTON (OPCIONAL PARA TU COMPAÑERO)
        // Aquí se podría chequear primero si el personaje está libre en la red.
        // Por ahora, lo seleccionamos localmente y avisamos al manager.
        // ---------------------------------------------------------

        manager.SeleccionarPersonaje(indicePersonaje);
    }

    // --- Métodos controlados por el Manager ---

    public void ConfigurarSeleccion(bool seleccionado)
    {
        estaSeleccionado = seleccionado;
        
        // Lógica visual: Seleccionado = Color Real / No seleccionado = Gris
        imagenPersonaje.color = estaSeleccionado ? colorOriginal : colorNormal;
        
        // Efecto visual extra: Pequeño cambio de tamaño
        transform.localScale = seleccionado ? Vector3.one * 1.1f : Vector3.one;
    }
}