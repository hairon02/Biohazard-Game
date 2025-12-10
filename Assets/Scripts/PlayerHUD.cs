using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image healthFill;     // Barra Verde (Vida)
    public Image shieldFill;     // Barra Azul (Escudo) <--- NUEVO
    public Image cooldownOverlay; 

    [Header("Referencia al Jugador")]
    public BaseCharacter playerStats;

    void Start()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<BaseCharacter>();
        }
    }

    void Update()
    {
        if (playerStats == null) return;

        UpdateBars();
        UpdateCooldown();
    }

    void UpdateBars()
    {
        // 1. Actualizar Vida (Verde)
        if (healthFill != null)
        {
            healthFill.fillAmount = playerStats.currentHealth / playerStats.maxHealth;
        }

        // 2. Actualizar Escudo (Azul) - NUEVO
        if (shieldFill != null)
        {
            // Evitamos división por cero si maxShield es 0
            float shieldPercent = (playerStats.maxShield > 0) 
                                ? playerStats.currentShield / playerStats.maxShield 
                                : 0;

            shieldFill.fillAmount = shieldPercent;
        }
    }

    void UpdateCooldown()
    {
        if (cooldownOverlay == null) return;
        cooldownOverlay.fillAmount = playerStats.GetCooldownFraction();
    }
}