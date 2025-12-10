using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public CharacterController characterController;
    public Animator animator;

    [Header("Movement")]
    public float speed = 10f;
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
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // ===============================
    // CORRECCIÓN PRINCIPAL AQUÍ
    // ===============================
    void Update()
    {
        
        // 1. Verificar si estamos tocando el suelo
        HandleGround();

        // 2. Calcular y aplicar movimiento horizontal (WASD)
        HandleMovement();
        HandleJump();     

        // 4. Aplicar gravedad y salto (eje Y)
        ApplyGravity();
    }

    // ===============================
    // RUN STATE (SIMPLE)
    // ===============================
    void HandleRunState()
    {
        // Corregido: Usamos 'sqrMagnitude' que es más eficiente que 'magnitude'
        run = moveInput.sqrMagnitude > 0.01f; 

        if (animator != null)
        {
            animator.SetBool("run", run);
        }
    }

    // ===============================
    // MOVEMENT
    // ===============================
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

        // ✅ RUN STATE
        run = input.sqrMagnitude > 0.01f;

        if (animator != null)
            animator.SetBool("run", run);

        // ✅ MOVEMENT
        Vector3 move = new Vector3(input.x, 0f, input.y);
        move = transform.TransformDirection(move);

        characterController.Move(move * speed * Time.deltaTime);
    }

    // ===============================
    // GROUND & GRAVITY
    // ===============================
    void HandleGround()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            sphereRadius,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        if (isGrounded && velocity.y < 0)
        {
            jumpPressed = false;
            if (animator != null)
                animator.SetBool("jump", false);
            velocity.y = -2f;
        }


    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    // ===============================
    // INPUT SYSTEM CALLBACKS
    // ===============================
    public void OnMove(InputAction.CallbackContext context)
{
    moveInput = context.ReadValue<Vector2>();
    // Agrega esta línea temporalmente:
    Debug.Log("Input recibido: " + moveInput); 
}

    void HandleJump()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("SPACE PRESIONADO");

            if (isGrounded)
            {
                Debug.Log("SALTANDO");
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                Debug.Log("NO ESTA EN EL SUELO");
            }
            jumpPressed = true;
            if (animator != null)
                animator.SetBool("jump", true);
        }

    }
void OnDrawGizmosSelected()
{
    if (groundCheck == null) return;
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(groundCheck.position, sphereRadius);
}

}