using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))] // Esto asegura que Irina tenga una bocina
public class IrinaKessler : BaseCharacter
{
    [Header("Habilidad: Granada de Estasis")]
    public GameObject stasisGrenadePrefab; 
    public Transform throwPoint;
    public float throwForce = 15f; 

    [Header("Audio")]
    public AudioClip throwSound; // <--- Aquí arrastrarás tu granada.mp3

    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        characterName = "Irina Kessler";
        
        // Obtenemos la referencia al componente de audio
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame && CanUseAbility())
        {
            ActivateSpecialAbility();
            StartCooldown();
        }
    }

    public override void ActivateSpecialAbility()
    {
        if (stasisGrenadePrefab != null && throwPoint != null)
        {
            // 1. Crear la granada
            GameObject grenade = Instantiate(stasisGrenadePrefab, throwPoint.position, throwPoint.rotation);
            
            // 2. Empujarla
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
            }

            // 3. REPRODUCIR SONIDO
            if (throwSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(throwSound); // PlayOneShot es ideal para efectos
            }

            Debug.Log(">> ¡Granada lanzada!");
        }
    }

    protected override void ApplyPassiveEffect() { }

    // Mantener tu lógica de pasiva intacta
    public override void TakeDamage(float amount, string damageType)
    {
        float finalDamage = amount;
        if (damageType == "Acido" || damageType == "Gas")
        {
            finalDamage = amount * 0.7f; 
        }
        base.TakeDamage(finalDamage, damageType);
    }
}