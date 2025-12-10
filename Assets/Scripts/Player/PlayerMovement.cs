using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun; // <--- 1. Importar

public class PlayerMovement : MonoBehaviourPun
{
    [Header("Components")]
    public CharacterController characterController;
    public Animator animator;

    // --- INTEGRACIÓN: Referencia a TU sistema de clases ---
    public BaseCharacter characterStats; 
    // -----------------------------------------------------

    [Header("Movement Settings")]
    // Esta variable ahora servirá solo de respaldo por si no hay personaje asignado
    public float defaultSpeed = 10f; 
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    Vector2 moveInput;
    bool run;                 
    bool jumpPressed;

    void Start()
    {
        if (!photonView.IsMine)
        {
            // Desactivamos el CharacterController para que no colisione raro con la sincronización
            CharacterController cc = GetComponent<CharacterController>();
            if(cc) cc.enabled = false; 
            
            this.enabled = false; // Apaga este script
            return;
        }
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // --- INTEGRACIÓN: Buscamos si este objeto tiene una clase (Drake, Liam o Irina) ---
        characterStats = GetComponent<BaseCharacter>();
        
        if (characterStats == null)
        {
            Debug.LogWarning("PlayerMovement: No se encontró script de Clase (Drake/Irina/Liam). Usando velocidad por defecto.");
        }
        else
        {
            Debug.Log($"Conectado con la clase: {characterStats.name}");
        }
        // ---------------------------------------------------------------------------------
    }

    void Update()
    {
        HandleGround();
        HandleMovement();
        HandleJump();     
        ApplyGravity();
    }

    void HandleMovement()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            input.x = (Keyboard.current.aKey.isPressed ? -1 : 0) +
                    (Keyboard.current.dKey.isPressed ?  1 : 0);

            input.y = (Keyboard.current.sKey.isPressed ? -1 : 0) +
                    (Keyboard.current.wKey.isPressed ?  1 : 0);
        }

        // Run logic
        run = input.sqrMagnitude > 0.01f;
        if (animator != null) animator.SetBool("run", run);

        // Movement Direction
        Vector3 move = new Vector3(input.x, 0f, input.y);
        move = transform.TransformDirection(move);

        // --- INTEGRACIÓN: LEER LA VELOCIDAD DE TU CLASE ---
        // Si existe characterStats, usamos SU velocidad (que puede cambiar por habilidades).
        // Si no existe, usamos defaultSpeed.
        float currentSpeed = (characterStats != null) ? characterStats.movementSpeed : defaultSpeed;
        // --------------------------------------------------

        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleGround()
    {
        if (groundCheck == null) return;

        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            sphereRadius,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        if (isGrounded && velocity.y < 0)
        {
            jumpPressed = false;
            if (animator != null) animator.SetBool("jump", false);
            velocity.y = -2f;
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = true;
            if (animator != null) animator.SetBool("jump", true);
        }
    }

    // Callbacks del Input System (si se usan en lugar del Update directo)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, sphereRadius);
    }
}