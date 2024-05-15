using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Rigidbody playerRigidBody; //Eu odeio character controllers :)
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private float walkSpeedMultiplier, runSpeedMultiplier, jumpForce, stamina = 100f;
    [SerializeField] private bool isRunning = false, isDashing = false, isJumping = false, isGrounded = true;
    [SerializeField] private bool canSwapWeapon = true;

    private void Start()
    {

    }


    private void Update()
    {
        if (true /*COLOCAR CONDIÇÃO Q O JOGO DEU PLAY*/)
        {
            StaminaRegen();
            Movement();
            GroundCheck();
            Jump();
            Dash();
            WeaponSwap();
        }
    }

    private void StaminaRegen()
    {
        //SÓ REGENERA STAMINA SE NÃO ESTIVER SE MOVENDO & TIVER STAMINA MENOR QUE O MÁXIMO
        if (stamina < PlayerHPController.instance.GetMaxStamina() && isRunning == false && isDashing == false && isGrounded == true)
        {
            PlayerHPController.instance.ChangeStamina(15f * Time.deltaTime, false);
            stamina += 15f * Time.deltaTime;
        }

        //AUTO EXPLICATIVO
        if (stamina < 0)
        {
            stamina = 0;
        }
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //CORRER SE TIVER STAMINA NECESSÁRIA E ESTIVER SE MOVENDO
        if (Input.GetKey(InputController.instance.run) && (x != 0 || y != 0) && stamina >= 0.125f)
        {
            //CONSUMO DE STAMINA AO CORRER
            PlayerHPController.instance.ChangeStamina(0.125f, true);
            stamina -= 0.125f;

            isRunning = true;
            playerAnimator.speed = runSpeedMultiplier;
        }
        else
        {
            isRunning = false;
            playerAnimator.speed = walkSpeedMultiplier;
        }


        //ANDAR MAIS DEVAGAR INDO PARA TRÁS, VULGO, Y < 0, REDUZINDO A VELOCIDADE EM 20%
        if (y < 0)
        {
            playerAnimator.speed = 1;
        }

        //TOCAR ANIMAÇÕES DE ANDAR SE ESTIVER NO CHÃO, SEM PULAR, E SEM DASH
        if (isDashing == false && isJumping == false && isGrounded == true)
        {
            playerAnimator.Play("Walk Tree");
        }

        //LITERAL MOVIMENTAÇÃO VIA ROOT MOTION
        playerAnimator.SetFloat("WalkHorizontal", x);
        playerAnimator.SetFloat("WalkVertical", y);
    }

    private void GroundCheck()
    {
        //ENVIA 4 RAYCASTS CADA UM EM UMA POSIÇÃO DIFERENTE PARA TER CERTEZA ABSOLUTA QUE O PLAYER ESTÁ NO CHÃO. NÃO CONSIGO VER ISSO AQUI RETORNANDO QUE O PLAYER ESTÁ NO CHÃO SEM ELE REALMENTE ESTAR.
        Vector3 horizontalAxisPoint = new Vector3((playerCollider.radius / 2) + 0.1f, 0, 0);
        Vector3 verticalAxisPoint = new Vector3(0, 0, (playerCollider.radius / 2) + 0.1f);

        if
        (
            Physics.Raycast(transform.position, Vector3.down, 0.1f) == true
            || Physics.Raycast(transform.position + horizontalAxisPoint, Vector3.down, 0.1f) == true
            || Physics.Raycast(transform.position - horizontalAxisPoint, Vector3.down, 0.1f) == true
            || Physics.Raycast(transform.position + verticalAxisPoint, Vector3.down, 0.1f) == true
            || Physics.Raycast(transform.position - verticalAxisPoint, Vector3.down, 0.1f) == true
        )// CONTROLA AS VARIÁVEIS QUE SÃO AUTO EXPLICATIVAS, E PERMITE O ANIMATOR EXECUTAR TRANSIÇÕES DE PULO OU NÃO
        {
            isGrounded = true;
            playerAnimator.SetBool("IsGrounded", true);
        }
        else
        {
            isGrounded = false;
            isJumping = false;

            playerAnimator.SetBool("IsJumping", false);
            playerAnimator.SetBool("IsGrounded", false);
        }
    }

    private void Jump()
    {
        //ESTÁ NO CHÃO E TEM STAMINA NESCESSÁRIA?
        if (Input.GetKeyDown(InputController.instance.jump) && isGrounded == true && stamina >= 15)
        {
            isJumping = true;
            playerAnimator.SetBool("IsJumping", true);
            playerRigidBody.AddForce(jumpForce * Vector3.up, ForceMode.Force);
        }
    }

    private void Dash()
    {
        //SÓ DA DASH SE ESTÁ NO CHÃO, TEM STAMINA PARA TAL E NÃO ESTÁ DANDO DASH ATUALMENTE
        if (Input.GetKeyDown(InputController.instance.dash) && isGrounded && isDashing == false && stamina >= 25f)
        {
            isDashing = true;
            playerAnimator.speed = runSpeedMultiplier;
            playerAnimator.SetBool("IsDashing", true);
            playerAnimator.Play("Dash Tree");
        } //EXECUTA APENAS DURANTE O DASH PARA CONFERIR SE ELE JA TERMINOU DE DAR O DASH
        else if (isDashing == true && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            isDashing = false;
            playerAnimator.speed = walkSpeedMultiplier;
            playerAnimator.SetBool("IsDashing", false);
        }
    }

    private void WeaponSwap()
    {

    }
}
