using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject groundCheckObject;

    private PlayerInputActions inputActions;
    private GroundCheck groundCheck;
    private Rigidbody2D playerRB;

    private Vector2 moveInput;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        groundCheck = groundCheckObject.GetComponent<GroundCheck>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        Jump();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    private void PlayerMove()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        transform.Translate(move * speed * Time.deltaTime);
    }

    private void Jump()
    {
        if (inputActions.Player.Jump.triggered && groundCheck.OnGround)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
