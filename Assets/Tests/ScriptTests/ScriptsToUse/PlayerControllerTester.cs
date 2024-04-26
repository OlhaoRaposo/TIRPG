using UnityEngine;

public class CameraControllerTester : MonoBehaviour
{
    public Transform playerCamera;
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float rotationSpeed = 5f;
    public float cameraRotationSpeed = 2f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public CharacterController controller;

    private Vector3 moveDirection;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movimento do jogador
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Rotação do jogador
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(Vector3.up * mouseX);

        // Rotação da câmera
        float mouseY = Input.GetAxis("Mouse Y") * cameraRotationSpeed;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        // Aplicar gravidade ao jogador
        moveDirection.y += gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        // Pular
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
}