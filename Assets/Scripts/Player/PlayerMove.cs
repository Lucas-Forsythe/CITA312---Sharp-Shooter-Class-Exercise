using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // Editor Varaibles
    [SerializeField] public float MoveSpeed = 5f;
    [SerializeField] public float JumpForce = 5f;
    [SerializeField] private float Sensitivity = 50f;
    [SerializeField] private Transform CameraTransform;


    // Private Variables
    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private float xRotation = 0f;
    private Vector2 lookInput;
    private PlayerInput playerInput;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Prevent collisions from tipping or spinning the player
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Keep the player stable against walls
        rb.angularDamping = 5f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        CheckGround();
        MovePlayer();
        HandleMouseLook();
    }

    public void OnLook(InputValue inputValue)
    {
        lookInput = inputValue.Get<Vector2>();
    }

    void HandleMouseLook()
    {
        float mouseX = lookInput.x * Sensitivity * Time.fixedDeltaTime;
        float mouseY = lookInput.y * Sensitivity * Time.fixedDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        CameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
    }

    void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        Vector3 Direction = transform.right * moveInput.x + transform.forward * moveInput.y;
        Direction = Direction.normalized;
        rb.linearVelocity = new Vector3(Direction.x * MoveSpeed, rb.linearVelocity.y, Direction.z * MoveSpeed);
    }

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float range = 100f;

    void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Attack button pressed!");
            Shooting();
        }
    }

    void Shooting()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned!");
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out hit,
            range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // Check if the object has an EnemyHealth component
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                Debug.Log("Enemy hit!");
                enemy.TakeDamage(40);
            }
        }
        
        else
        {
            Debug.Log("Shot fired, but nothing was hit.");
        }

        Debug.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * range,
            Color.red,
            2f
        );
    }
}


