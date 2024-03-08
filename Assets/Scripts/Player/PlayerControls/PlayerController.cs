using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

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
    private bool startedFall = true, isRanged = true;


    [Header("References")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject rangedWeapon, meleeWeapon;
    [SerializeField] Animator animator;
    private Vector3 startRelativePoint;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        StaminaRegen();
        GroundCheck();

        Move();
        //ToggleFightStyle();
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

    private void GroundCheck()
    {
        Vector3 horizontalAxisPoint = new Vector3((controller.radius / 2) + 0.1f, 0, 0);
        Vector3 verticalAxisPoint = new Vector3(0, 0, (controller.radius / 2) + 0.1f);

        if
        (
            Physics.Raycast(transform.position, Vector3.down, 0.1f) == true
            || Physics.Raycast(transform.position + horizontalAxisPoint, Vector3.down, 0.1f) == true
            || Physics.Raycast(transform.position - horizontalAxisPoint, Vector3.down, 0.1f) == true
            || Physics.Raycast(transform.position + verticalAxisPoint, Vector3.down, 0.1f) == true
            || Physics.Raycast(transform.position - verticalAxisPoint, Vector3.down, 0.1f) == true
        )
        {
            isGrounded = true;
            CancelInvoke("FallCheck");
            animator.SetBool("IsGrounded", true);
            return;
        }
        else
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
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
            if (Input.GetKey(InputController.instance.run) && (x != 0 || y != 0) && stamina >= 0.125f)
            {
                PlayerHPController.instance.ChangeStamina(0.125f, true);
                stamina -= 0.125f;

                isRunning = true;
                animator.speed = runSpeed;
                if (isDashing == false && isJumping == false && isGrounded == true)
                {
                    animator.Play("Walk Tree");
                }
            }
            else
            {
                isRunning = false;
                animator.speed = speed;
                if (isDashing == false && isJumping == false && isGrounded == true)
                {
                    animator.Play("Walk Tree");
                }
            }

            if (isDashing == false)
            {
                PlayerCamera.instance.AlignRotation(PlayerCamera.instance.cameraBody.gameObject);
            }

            if (y <= 0)
            {
                animator.speed *= 0.8f;
            }

            animator.SetFloat("WalkHorizontal", x);
            animator.SetFloat("WalkVertical", y);

            if (startedFall == false && isJumping == false && isGrounded == false)
            {
                Vector3 dir = Vector3.zero;
                if (Input.GetAxis("Vertical") > 0)
                {
                    dir += PlayerCamera.instance.GetCameraForward();
                }
                if (Input.GetAxis("Vertical") < 0)
                {
                    dir -= PlayerCamera.instance.GetCameraForward();
                }
                if (Input.GetAxis("Horizontal") > 0)
                {
                    dir += PlayerCamera.instance.GetCameraRight();
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    dir -= PlayerCamera.instance.GetCameraRight();
                }

                controller.Move(speed * dir * Time.fixedDeltaTime);
            }
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
            animator.SetBool("IsJumping", true);
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
                animator.SetBool("IsJumping", false);
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
        if (Input.GetKeyDown(InputController.instance.dash) && isGrounded && stamina >= 25f && isDashing == false)
        {
            PlayerHPController.instance.ChangeStamina(25f, true);
            stamina -= 25f;

            StartCoroutine(DashAction());
            Invoke("StopDash", 2);
        }
    }

    private void ToggleFightStyle()
    {
        if (isRanged == false && Input.GetKeyDown(KeyCode.Alpha1))
        {
            rangedWeapon.SetActive(true);
            meleeWeapon.SetActive(false);
            if (isGrounded == true)
            {
                animator.Play("RangedToMelee");
            }

            isRanged = true;
        }

        if (isRanged == true && Input.GetKeyDown(KeyCode.Alpha2))
        {
            rangedWeapon.SetActive(false);
            meleeWeapon.SetActive(true);
            if (isGrounded == true)
            {
                animator.Play("MeleeToRanged");
            }

            isRanged = false;
        }
    }

    private IEnumerator DashAction()
    {
        Vector3 movingDir = Vector3.zero;

        isDashing = true;
        startRelativePoint = transform.position;
        PlayerCamera.instance.ToggleMovement(false);
        animator.SetBool("IsDashing", true);

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            movingDir = transform.forward;
        }
        else
        {
            movingDir = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        }

        while (RelativeDistance() < dashLength && isDashing == true)
        {
            controller.Move((dashLength - RelativeDistance() + 1) * 7.5f * movingDir.normalized * Time.deltaTime);
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
        animator.SetBool("IsDashing", false);
    }

    private float RelativeDistance()
    {
        return Vector3.Distance(transform.position, startRelativePoint);
    }

    public void Respawn()
    {
        Debug.Log("Desabilitei");
        UIManager.instance.DisableAllPanels();
        UIManager.instance.ReactivatesCursor();
        PlayerHPController.instance.OnLevelWasLoaded();
        Teste();
    }

    public void Teste()
    {
        transform.position = new Vector3 (0,0,0);

    }
}
