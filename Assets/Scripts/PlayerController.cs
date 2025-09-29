using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;

    [SerializeField] private bool stopDashing;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isLastDirectionRight;

    private Rigidbody playerRb;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashLengh;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;

    private float dashDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        isLastDirectionRight = true;
        isDashing = false;
        stopDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //control player rotation and Z pos
        ControlRotationAndPosZ();

        //move LAND PLAYER LOOK DIRECTION   
        horizontalInput = Input.GetAxis("Horizontal");
        if (!isDashing) { transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput, Space.World); }

        //jump
        if (Input.GetKeyDown(KeyCode.Space)) { Jump(); }

        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) { StartCoroutine(Dash()); }

        //player last look direction and look direction
        if (Input.GetKeyDown(KeyCode.A))
        { 
            isLastDirectionRight = false;
            if (!isDashing) { transform.rotation = Quaternion.Euler(0, -180, 0); }
        }
        if (Input.GetKeyDown(KeyCode.D)) 
        { 
            isLastDirectionRight = true;
            if (!isDashing) { transform.rotation = Quaternion.Euler(0, 0, 0); }
        }
    }

    // if touching obstacle while dashing then stop moving
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle") && isDashing)
        {
            float xPos = transform.position.x;
            stopDashing = true;
        }
    }

    private void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        playerRb.useGravity = false;
        float yPos = transform.position.y;
        if (isLastDirectionRight) { dashDirection = 1; }
        else { dashDirection = -1; }

        float elapsed = 0f;
        while (elapsed < dashTime && !stopDashing)
        {
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
            transform.Translate(Vector3.right * dashSpeed * Time.deltaTime * dashDirection, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerRb.useGravity = true;
        isDashing = false;
        stopDashing = false;
    }

    private void ControlRotationAndPosZ()
    {
        float yRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
    }
}