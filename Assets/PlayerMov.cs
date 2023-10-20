using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float jumpForce = 5.0f;
    public float gravity = 9.81f;
    public float lookUpLimit = 80.0f; // Limite de olhar para cima
    public float lookDownLimit = 80.0f; // Limite de olhar para baixo

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalRotation = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movimento horizontal e vertical
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput);
        inputDirection = Camera.main.transform.TransformDirection(inputDirection);
        inputDirection.y = 0;
        inputDirection.Normalize();

        // Movimento com base na entrada do jogador
        moveDirection = inputDirection * moveSpeed;

        // Rotação horizontal com base na direção do mouse
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(Vector3.up * mouseX);

        // Rotação vertical com base na direção do mouse (para cima e para baixo)
        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;
        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -lookUpLimit, lookDownLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Aplicar gravidade
        if (controller.isGrounded)
        {
            moveDirection.y = -0.5f; // Evita pequenos solavancos ao tocar o chão
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Aplicar movimento usando o Character Controller
        controller.Move(moveDirection * Time.deltaTime);
    }
}
