using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Character Controller")]
    [SerializeField] CharacterController controller;
    //Velocidade do movimento de andar do jogador.
    [SerializeField] float speed = 5f;
    //Força da gravidade.
    [SerializeField] float gravity = -9f;
    //Direção do jogador.
    [SerializeField] Vector3 playerDirection;
    //Controla a direção do movimento do jogador.
    [SerializeField] Vector3 dir;

    [Header("Run Movement")]
    //Velocidade do movimento de correr do jogador.
    [SerializeField] float runVelocity = 7.5f;
    //Variável de controle se o jogador está ou não correndo.
    [SerializeField] bool isRunning = false;

    [Header("Stealth Movement")]
    //Velocidade do movimento do jogador no modo furtivo.
    [SerializeField] float stealthSpeed = 2.5f;
    //Variável de controle do modo furtivo.
    [SerializeField] bool stealthMode = false;
    //Variável de controle do tamanho original do colisor do jogador.
    [SerializeField] Vector3 standingHeight = new Vector3(1, 1, 1);
    //Variável de controle do tamanho do colisor do jogador ao estar abaixado.
    [SerializeField] Vector3 crouchedHeight = new Vector3(1, 0.5f, 1);
    //Colisor auxiliar para caso o modo furtivo seja o personagem inclinado e não agachado.
    [SerializeField] CapsuleCollider colliderTilt;
    //Centro do colisor do Character Controller ao estar agachado.
    [SerializeField] Vector3 controllerCenterCollider = new Vector3(0, -0.5f, 0);
    //Altura do colisor do Character Controller ao estar agachado.
    [SerializeField] float controllerHeightCollider = 1f;

    [Header("Jump Settings")]
    //Altura do pulo.
    [SerializeField] float jumpHeight = 1f;
    //Variável de controle se o jogador está ou não no chão.
    [SerializeField] bool inGround;

    [Header("Dash Settings")]
    //Distancia da esquiva.
    [SerializeField] float dashForce = 5f;

    [Header("Stamina Settings")]
    //Estamina do jogador.
    [SerializeField] float stamina = 100f;
    //Temporizador para recarga da estamina.
    [SerializeField] float cooldownStamina;

    void Start()
    {
        //Atribui para a variável o Character Controller do jogador.
        controller = GetComponent<CharacterController>();
        //Atribui para a variável o colisor auxiliar para inclinação do personagem.
        colliderTilt = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        //Controla o tempo do cooldown.
        cooldownStamina += Time.deltaTime;

        //Atribui para a variável de controle se o jogador está no chão o retorno do método de verificação nativo do Character Controller.
        inGround = controller.isGrounded;
        //Confere se o jogador está no chão.
        if (inGround && playerDirection.y < 0.01f)
        {
            //Se o jogador estiver no chão anula o efeito da gravidade no jogador, evitando de ficar entrando no chão.
            playerDirection = Vector3.zero;
        }

        //Nega a possibilidade de estamina negativa.
        if (stamina <= 0)
        {
            stamina = 0;
        }
        //Verifica se está utilizando e recarrega a estamina.
        if (cooldownStamina > 0.25f && stamina < 100 && !isRunning && inGround)
        {
            //Acrescenta mais estamina ao jogador.
            stamina += 1f;
            cooldownStamina = 0;
        }

        //Movimentação normal do jogador.
        if (inGround && !stealthMode && !isRunning)
        {
            dir = Vector3.zero;
            //Normaliza a direção do movimento do jogador.
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
            controller.Move(dir.normalized * Time.deltaTime * speed);
            //Controla a direção da rotação do jogador ao estar ou para de se movimentar.
            if (dir.magnitude > 0.2f)
            {
                PlayerCamera.instance.AlignRotation(PlayerCamera.instance.cameraBody.gameObject);
            }
        }

        //Modo furtivo, pressionar a tecla para ativar e desativar o modo furtivo.
        if (Input.GetKeyDown(InputController.instance.stealth) && inGround)
        {
            //Ativa e desativa o modo furtivo.
            stealthMode = !stealthMode;
            //Ativa e desativa o colisor auxiliar para a inclinação.
            colliderTilt.enabled = !colliderTilt.enabled;
        }

        if (stealthMode)
        {
            //Somente para visualização do modo, será o colisor que diminuirá durante o modo.
            transform.localScale = crouchedHeight;
            controller.height = controllerHeightCollider;
            controller.center = controllerCenterCollider;

            dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Normaliza a direção do movimento do jogador.
            controller.Move(dir.normalized * Time.deltaTime * stealthSpeed);

            //Controla a direção da rotação do jogador ao estar ou para de se movimentar.
            if (dir.magnitude > 0.2f)
            {
                transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            }
        }
        else
        {
            //Somente para visualização do modo, será o colisor que retornará ao normal ao desativar o modo.
            transform.localScale = standingHeight;
            controller.height = 2f;
            //controller.center = Vector3.zero;
        }

        //Movimento de correr ao manter a tecla pressionada.
        if (Input.GetKey(InputController.instance.run) && inGround && stamina >= 0.5f)
        {
            //Desativa o modo furtivo ao começar a correr.
            if (stealthMode)
            {
                //Desativa o modo furtivo.
                stealthMode = !stealthMode;
                //Desativa o colisor auxiliar para a inclinação.
                colliderTilt.enabled = !colliderTilt.enabled;
            }

            //Ativa a variável, para que não haja conflito com as outras movimentações.
            isRunning = true;

            //Retira estamina do jogador ao correr.
            stamina -= 0.5f;

            dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Normaliza a direção do movimento do jogador.
            controller.Move(dir.normalized * Time.deltaTime * runVelocity);

            //Controla a direção da rotação do jogador ao estar ou para de se movimentar.
            if (dir.magnitude > 0.2f)
            {
                transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            }
        }
        else
        {
            //Desativa a variável de controle de correr.
            isRunning = false;
        }

        /*Corrida e dash em mesma tecla de comando, inicia a corrida com dash e depois mantém a velocidade, 
        dash somente se estiver estamina suficiente.*/
        /*if (Input.GetKey(InputController.instance.run) && inGround && stamina >= 0.5f)
        {
            //Desativa o modo furtivo ao começar a correr.
            if (stealthMode)
            {
                //Desativa o modo furtivo.
                stealthMode = !stealthMode;
                //Desativa o colisor auxiliar para a inclinação.
                colliderTilt.enabled = !colliderTilt.enabled;
            }

            //Ativa a variável, para que não haja conflito com as outras movimentações.
            isRunning = true;

            //Retira estamina do jogador ao correr.
            stamina -= 0.5f;

            if (Input.GetKeyDown(InputController.instance.run) && stamina >= 15f)
            {
                //Remove estamina ao esquivar.
                 stamina -= 15f;

                //Fixa a direção do dash no momento exato do comando.
                playerDirection.x += Mathf.Sqrt(-3.0f * gravity) * dir.x * dashForce;
                playerDirection.z += Mathf.Sqrt(-3.0f * gravity) * dir.z * dashForce;
            }

            dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Normaliza a direção do movimento do jogador.
            controller.Move(dir.normalized * Time.deltaTime * runVelocity);

            //Controla a direção da rotação do jogador ao estar ou para de se movimentar.
            if (dir.magnitude > 0.2f)
            {
                transform.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            }
        }
        else
        {
            //Desativa a variável de controle de correr.
            isRunning = false;
        }*/

        //Pulo negando a possibilidade de pular no modo furtivo.
        if (Input.GetKeyDown(InputController.instance.jump) && inGround && !stealthMode && stamina >= 15f)
        {
            //Remove estamina ao pular.
            stamina -= 15f;

            //Fixa a direção do pulo para a mesma do momento exato do comando.
            playerDirection.x += Mathf.Sqrt(-3.0f * gravity) * dir.x;
            playerDirection.z += Mathf.Sqrt(-3.0f * gravity) * dir.z;
            playerDirection.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            Debug.Log("Pulei");
        }

        //Dash.
        if (Input.GetKeyDown(InputController.instance.dash) && inGround && stamina >= 15f)
        {
            //Remove estamina ao esquivar.
            stamina -= 15f;

            //Fixa a direção do dash no momento exato do comando.
            playerDirection.x += Mathf.Sqrt(-3.0f * gravity) * dir.x * dashForce;
            playerDirection.z += Mathf.Sqrt(-3.0f * gravity) * dir.z * dashForce;
            Debug.Log("Dash");
        }

        //Nega a possibilidade de dar dash e pular ao mesmo tempo, evitando um pulo longo e rápido.
        if (Input.GetKeyDown(InputController.instance.jump) && Input.GetKeyDown(InputController.instance.dash))
        {
            playerDirection = Vector3.zero;
        }

        //Controla a queda do pulo do jogador com a gravidade.
        playerDirection.y += gravity * Time.deltaTime;
        controller.Move(playerDirection * Time.deltaTime);
    }
}