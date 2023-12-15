using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float speed;
    [SerializeField] float runSpeed;
    [SerializeField] float stealthSpeed = 0.5f;
    [SerializeField] float dashLength = 5f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float gravity = -9f;
    [SerializeField] float stamina = 100f;
    [SerializeField] bool isRunning = false;
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isGrounded = true;
    [SerializeField] bool isDashing = true;
    [SerializeField] bool stealthMode = false;
    private float speedModifier;
    private bool startedFall = true;


    [Header("References")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    private Vector3 startRelativePoint;

    void Update()
    {
        StaminaRegen();
        Move();
        GroundCheck();
    }

    private void StaminaRegen()
    {
        if (stamina < PlayerHPController.instance.GetMaxStamina() && isRunning == false && isDashing == false && isGrounded == true)
        {
            PlayerHPController.instance.ChangeStamina(15f * Time.deltaTime, false);
            stamina += 15f * Time.deltaTime;
        }

        if (stamina <= 0)
        {
            stamina = 0;
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //Movement
        if (x != 0 || y != 0)
        {
            //Run
            if (Input.GetKey(InputController.instance.run) && (x != 0 || y != 0) && stamina >= 0.5f)
            {
                PlayerHPController.instance.ChangeStamina(0.5f, true);
                stamina -= 0.5f;

                isRunning = true;
                animator.speed = runSpeed;
            }
            else
            {
                isRunning = false;
                animator.speed = speed;
            }

            PlayerCamera.instance.AlignRotation(PlayerCamera.instance.cameraBody.gameObject);

            if (y <= 0)
            {
                animator.speed *= 0.8f;
            }

            animator.SetFloat("WalkHorizontal", x);
            animator.SetFloat("WalkVertical", y);
            x = 0;
            y = 0;
        }

        //Jump && fall
        if (Input.GetKeyDown(InputController.instance.jump) && isGrounded == true && stealthMode == false && stamina >= 15f)
        {
            PlayerHPController.instance.ChangeStamina(15f, true);
            stamina -= 15f;

            startRelativePoint = transform.position;
            isJumping = true;
        }

        if (isJumping == true)
        {
            speedModifier = ((jumpHeight - RelativeDistance()) / 2) + 0.1f;
            if (RelativeDistance() < jumpHeight)
            {
                controller.Move(Vector3.up * speedModifier * -gravity * Time.deltaTime);
            }
            else
            {
                isJumping = false;
            }
        }
        else if (isGrounded == false)
        {
            if (startedFall == true)
            {
                startRelativePoint = transform.position;
                startedFall = false;
            }
            float speedModifier = (RelativeDistance() / 2) + 0.1f;

            if (RelativeDistance() >= 2)
            {
                speedModifier = 1;
            }

            controller.Move(Vector3.up * speedModifier * gravity * Time.deltaTime);
        }
        else
        {
            startedFall = true;
        }


        //Dash
        if (Input.GetKeyDown(InputController.instance.dash) && isGrounded && stamina >= 15f)
        {
            PlayerHPController.instance.ChangeStamina(15f, true);
            stamina -= 15f;

            StartCoroutine(DashAction());
            Invoke("StopDash", 2);
        }
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.1f) == true)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private IEnumerator DashAction()
    {
        isDashing = true;
        startRelativePoint = transform.position;
        PlayerCamera.instance.ToggleMovement(false);
        while (RelativeDistance() < dashLength && isDashing == true)
        {

            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                controller.Move((dashLength - RelativeDistance() + 1) * 5 * transform.forward * Time.deltaTime);
            }
            else
            {
                controller.Move((dashLength - RelativeDistance() + 1) * 5 * ((transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"))) * Time.deltaTime);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopDash();
        yield return null;
    }

    private void StopDash()
    {
        CancelInvoke("StopDash");
        isDashing = false;
        PlayerCamera.instance.ToggleMovement(true);
    }
    private float RelativeDistance()
    {
        return Vector3.Distance(transform.position, startRelativePoint);
    }

}
