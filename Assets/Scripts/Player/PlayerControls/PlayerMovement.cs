using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("Movement")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CharacterController playerController; //Eu odeio character controllers :)
    [SerializeField] private CapsuleCollider playerCollider;
    public GameObject playerModel;
    [SerializeField] private float walkSpeedMultiplier, runSpeedMultiplier, jumpHeight, stamina = 100f;
    [SerializeField] private bool isRunning = false, isDashing = false, isJumping = false, isGrounded = true, canMove = true;

    [Header("Combat")]
    public GameObject[] allWeapons;
    [SerializeField] private bool canSwapWeapon = true;
    public bool isRanged = true;

    private float speedModifier;
    private Vector3 startRelativePoint;
    private bool startedFall = false;

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (WorldController.worldController.isGameStarted)
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

        //SE ESTÁ INDO PRA FRENTE OLHAR PARA ONDE ESTÁ INDO, E O MESMO AO FAZER STRAFE
        if (y != 0 || x != 0)
        {
            PlayerCameraMovement.instance.AlignTargetWithCamera(PlayerCameraMovement.instance.playerObject);
        }


        //ANDAR MAIS DEVAGAR INDO PARA TRÁS, VULGO, Y < 0, REDUZINDO A VELOCIDADE EM 20%
        if (y < 0)
        {
            playerAnimator.speed = 1;
        }


        if (isGrounded == false) //PERMITE MOVIMENTAÇÃO NO AR SEM ANIMAÇÕES DE ANDAR NO CHÃO
        {
            Vector3 dir = Vector3.zero;
            if (y > 0)
            {
                dir += PlayerCameraMovement.instance.GetCameraForward();
            }
            if (y < 0)
            {
                dir -= PlayerCameraMovement.instance.GetCameraForward();
            }
            if (x > 0)
            {
                dir += PlayerCameraMovement.instance.GetCameraRight();
            }
            if (x < 0)
            {
                dir -= PlayerCameraMovement.instance.GetCameraRight();
            }
            playerController.Move(dir * runSpeedMultiplier * 2 * Time.deltaTime);
        }
        else if (isDashing == false && isJumping == false && PlayerMeleeCombat.instance.isInCombo == false) //TOCAR ANIMAÇÕES DE ANDAR SE ESTIVER NO CHÃO, SEM PULAR, SEM DASH, E NÃO ESTÁ REALIZANDO ATAQUES MELEE
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
            Physics.Raycast(transform.position, Vector3.down, 0.25f) == true
            || Physics.Raycast(transform.position + horizontalAxisPoint, Vector3.down, 0.25f) == true
            || Physics.Raycast(transform.position - horizontalAxisPoint, Vector3.down, 0.25f) == true
            || Physics.Raycast(transform.position + verticalAxisPoint, Vector3.down, 0.25f) == true
            || Physics.Raycast(transform.position - verticalAxisPoint, Vector3.down, 0.25f) == true
        )// CONTROLA AS VARIÁVEIS QUE SÃO AUTO EXPLICATIVAS, E PERMITE O ANIMATOR EXECUTAR TRANSIÇÕES DE PULO OU NÃO
        {
            isGrounded = true;
            playerAnimator.SetBool("IsGrounded", true);
        }
        else
        {
            isGrounded = false;
            playerAnimator.SetBool("IsGrounded", false);
        }
    }

    private void Jump()
    {
        //Jump && fall
        if (Input.GetKeyDown(InputController.instance.jump) && isGrounded == true && stamina >= 15f)
        {
            PlayerHPController.instance.ChangeStamina(15f, true);
            stamina -= 15f;

            startRelativePoint = transform.position;
            isJumping = true;
            playerAnimator.SetBool("IsJumping", true);
            //COMEÇOU O PULO
        }

        if (isJumping == true)
        {
            speedModifier = ((jumpHeight - RelativeDistance(Vector3.up)) / 2) + 0.25f;
            if (RelativeDistance(Vector3.up) < jumpHeight)
            {
                playerController.Move(Vector3.up * speedModifier * 9.8f * Time.deltaTime);
                //MOVIMENTAÇÃO NO ESTADO DE PULAR
            }
            else
            {
                isJumping = false;
                playerAnimator.SetBool("IsJumping", false);
                //PAROU O PULO
            }
        }
        else if (isGrounded == false)
        {
            if (startedFall == true)
            {
                startRelativePoint = transform.position;
                startedFall = false;
            }
            float speedModifier = (RelativeDistance(Vector3.up) / 2) + 0.25f;

            if (RelativeDistance(Vector3.up) >= 2)
            {
                speedModifier = 1.25f;
            }

            playerController.Move(Vector3.up * speedModifier * -9.8f * Time.deltaTime);
        }
        else
        {
            //COMECOU A CAIR
            startedFall = true;
        }
    }

    private void Dash()
    {
        //SÓ DA DASH SE ESTÁ NO CHÃO, TEM STAMINA PARA TAL E NÃO ESTÁ DANDO DASH ATUALMENTE
        if (Input.GetKeyDown(InputController.instance.dash) && isGrounded && isDashing == false && stamina >= 25f)
        {
            PlayerHPController.instance.ChangeStamina(25f, true);
            stamina -= 25;

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

    public void WeaponSwap()
    {
        //SE PUDER TROCAR DE ARMA
        if (canSwapWeapon == true)
        {
            //CASO ESTEJA FORA FORA DA ARMA RANGED E APERTE PARA TROCAR PARA ARMA RANGED
            if (isRanged == false && Input.GetKeyDown(KeyCode.Alpha1) && PlayerInventory.instance.GetRanged() != null)
            {
                //PROCURA A ARMA EQUIPADA
                foreach (GameObject weapon in allWeapons)
                {
                    if (weapon.name == PlayerGun.instance.GetGunName())
                    {
                        weapon.SetActive(true);
                    }
                    if (weapon.name == PlayerMeleeCombat.instance.GetMeleeName())
                    {
                        weapon.SetActive(false);
                    }
                }

                if (isGrounded == true)
                {
                    StartCoroutine(SwapWeaponAction(playerAnimator.GetCurrentAnimatorClipInfo(1).Length * 0.7f));
                    playerAnimator.Play($"Switch to {PlayerGun.instance.GetGunName()}");
                }

                PlayerGun.instance.enabled = true;
                PlayerMeleeCombat.instance.enabled = false;
                isRanged = true;
            }

            if (isRanged == true && Input.GetKeyDown(KeyCode.Alpha2) && PlayerInventory.instance.GetMelee() != null)
            {
                //PROCURA A ARMA EQUIPADA
                foreach (GameObject weapon in allWeapons)
                {
                    if (weapon.name == PlayerGun.instance.GetGunName())
                    {
                        weapon.SetActive(false);
                    }
                    if (weapon.name == PlayerMeleeCombat.instance.GetMeleeName())
                    {
                        weapon.SetActive(true);
                    }
                }

                if (isGrounded == true)
                {
                    StartCoroutine(SwapWeaponAction(playerAnimator.GetCurrentAnimatorClipInfo(1).Length * 0.7f));
                    playerAnimator.Play($"Switch to {PlayerMeleeCombat.instance.GetMeleeName()}");
                }

                PlayerGun.instance.enabled = false;
                PlayerMeleeCombat.instance.enabled = true;
                isRanged = false;
            }
        }
    }
    private IEnumerator SwapWeaponAction(float time)
    {
        playerAnimator.SetLayerWeight(1, 1);
        canSwapWeapon = false;

        PlayerMeleeCombat.instance.MeleeAttackToggle(false);
        PlayerGun.instance.ShootToggle(false);

        yield return new WaitForSeconds(time);

        playerAnimator.SetLayerWeight(1, 0);
        canSwapWeapon = true;

        PlayerMeleeCombat.instance.MeleeAttackToggle(true);
        PlayerGun.instance.ShootToggle(true);
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
    public void TeleportPlayer(Vector3 newPosition)
    {
        gameObject.SetActive(false);
        transform.position = newPosition;
        gameObject.SetActive(true);
    }
    private float RelativeDistance(Vector3 axis) //SERVE PARA CALCULAR PULOS DO PLAYER, CALCULA A MUDANÇA DE VELOCIDADE EM RELAÇÃO AO PONTO DE PARTIDA PARA DEIXAR A GRAVIDADE MAIS REALISTA.
    {
        Vector3 normalizedStart, normalizedEnd;
        normalizedStart = new Vector3(axis.x * startRelativePoint.x, axis.y * startRelativePoint.y, axis.z * startRelativePoint.z);
        normalizedEnd = new Vector3(axis.x * transform.position.x, axis.y * transform.position.y, axis.z * transform.position.z);
        if (isGrounded == false && isJumping == false && Vector3.Distance(normalizedStart, normalizedEnd) >= 1.5f)
        {
            //TOCA A ANIMAÇÃO DE QUEDA
            playerAnimator.Play("Falling");
        }
        return Vector3.Distance(normalizedStart, normalizedEnd);
    }
    public void ToggleMove(bool toggle)
    {
        canMove = toggle;
        playerAnimator.SetFloat("WalkHorizontal", 0);
        playerAnimator.SetFloat("WalkVertical", 0);
    }
}



