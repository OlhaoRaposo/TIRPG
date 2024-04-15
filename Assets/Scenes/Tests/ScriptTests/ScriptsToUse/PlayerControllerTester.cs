using UnityEngine;

public class PlayerControllerTester : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 8.0f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Vector3 velocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movimento horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
        movementDirection = transform.TransformDirection(movementDirection);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            velocity.y = jumpForce;
        }

        // Gravidade
        velocity.y += gravity * Time.deltaTime;

        // Movimento
        characterController.Move(movementDirection * moveSpeed * Time.deltaTime + velocity * Time.deltaTime);

        // Rotacao
        transform.Rotate(0.0f, Input.GetAxis("Mouse X") * Time.deltaTime * 100.0f, 0.0f);
    }
}
