using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [System.Serializable]
    public struct PerfilPersonaje {
        public string nombre;
        public Sprite fotoRetrato;
        public Sprite iconoHabilidad;
    }

    [Header("Base de Datos")]
    public PerfilPersonaje[] baseDeDatosPersonajes; // Configura Drake(0), Irina(1), Liam(2) aquí

    [Header("Estado (Izq)")]
    public Image retratoPersonaje;
    public Slider sliderSalud;
    public Image rellenoSalud;
    public Slider sliderEscudo;
    public Gradient gradienteSalud; 

    [Header("Combate (Der)")]
    public Image iconoHabilidad;
    public Image overlayCooldown; 
    public CanvasGroup vignetteDaño; 

    void Awake() { Instance = this; }

    void Start()
    {
        // ---------------------------------------------------------
        // TODO: PHOTON - TAREA PARA REDES
        // Leer CustomProperties: int index = (int)PhotonNetwork.LocalPlayer.CustomProperties["PersonajeID"];
        // ---------------------------------------------------------
        
        // Lectura Local (Temporal)
        int miIndice = PlayerPrefs.GetInt("MiPersonajeSeleccionado", 0);
        ConfigurarHUDInicial(miIndice);

        vignetteDaño.alpha = 0;
    }

    void ConfigurarHUDInicial(int index)
    {
        if (index < 0 || index >= baseDeDatosPersonajes.Length) index = 0;
        PerfilPersonaje datos = baseDeDatosPersonajes[index];
        retratoPersonaje.sprite = datos.fotoRetrato;
        iconoHabilidad.sprite = datos.iconoHabilidad;
    }

    // --- LLAMAR DESDE TU PLAYER CONTROLLER ---

    public void ActualizarSalud(float actual, float maximo)
    {
        float porcentaje = actual / maximo;
        sliderSalud.value = porcentaje;
        rellenoSalud.color = gradienteSalud.Evaluate(porcentaje);
        
        if (porcentaje < 0.3f) vignetteDaño.alpha = 0.3f; // Pantalla roja fija si mueres
        else vignetteDaño.alpha = 0f;
    }

    public void ActualizarEscudo(float actual, float maximo)
    {
        // Evitamos división por cero si el escudo máximo es 0
        if (maximo <= 0) 
        {
            sliderEscudo.value = 0;
            return;
        }

        sliderEscudo.value = actual / maximo;
    }

    public void EfectoRecibirDaño()
    {
        StopCoroutine("FlashDaño");
        StartCoroutine("FlashDaño");
    }

    IEnumerator FlashDaño()
    {
        vignetteDaño.alpha = 0.8f;
        yield return new WaitForSeconds(0.1f);
        while (vignetteDaño.alpha > 0)
        {
            vignetteDaño.alpha -= Time.deltaTime * 2;
            yield return null;
        }
    }
    
    // --- MÉTODOS DE COOLDOWN ---

    public void IniciarCooldownHabilidad(float duracion)
    {
        // Detenemos la corrutina anterior por si acaso se llama dos veces seguidas
        StopCoroutine("RutinaCooldown");
        StartCoroutine(RutinaCooldown(duracion));
    }

    // --- CORRUTINA PARA ANIMAR EL COOLDOWN ---
    // Esta función hace que la imagen oscura vaya desapareciendo como un reloj
    IEnumerator RutinaCooldown(float duracion)
    {
        Debug.Log("RELOJ: Iniciando animación de " + duracion + " segundos.");

        float tiempoFin = Time.time + duracion; 
        if (overlayCooldown != null) overlayCooldown.fillAmount = 1;

        while (Time.time < tiempoFin)
        {
            float tiempoRestante = tiempoFin - Time.time;
            if (overlayCooldown != null)
                overlayCooldown.fillAmount = tiempoRestante / duracion;
            
            yield return null; 
        }
        if (overlayCooldown != null) overlayCooldown.fillAmount = 0;
    }
}