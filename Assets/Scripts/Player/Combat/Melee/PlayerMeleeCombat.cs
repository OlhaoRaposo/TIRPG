using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMeleeCombat : MonoBehaviour
{
    public static PlayerMeleeCombat instance;

    [Header("Variables")]
    float damage;
    float comboExecutionWindowPercentage = 0.8f;
    float[] comboWindowTime;
    int comboIndex = 0;
    float nextAttackCooldown = 0;
    bool canAttack = true;
    bool isInCombo = false;

    [Header("References")]
    [SerializeField] PlayerMeleeBase weapon;
    [SerializeField] PlayerController player;
    [SerializeField] Animator animator;
    [SerializeField] Collider attackCollider;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        comboIndex = 0;
    }

    private void OnDisable()
    {
        EndCombo();
    }

    private void Start()
    {
        SetPlayerWeapon(weapon);
    }

    private void Update()
    {
        MeleeAttack();
    }

    public void SetPlayerWeapon(PlayerMeleeBase newWeapon)
    {
        damage = newWeapon.damage;
        comboExecutionWindowPercentage = newWeapon.comboExecutionWindowPercentage;
        
        //Setar animações override
    }

    private void MeleeAttack()
    {
        if (Input.GetMouseButtonDown(0) && canAttack && player.isGrounded == true)
        {

            if (comboIndex == 0)
            {
                StartCombo();
            }

            if (Time.time >= nextAttackCooldown && (animator.GetCurrentAnimatorStateInfo(0).length * (1 - comboExecutionWindowPercentage)) <= comboWindowTime[comboIndex])
            {
                string nextAttack = "Melee_0" + comboIndex;
                
                PlayerCamera.instance.AlignRotation(PlayerCamera.instance.cameraBody.gameObject);
                animator.Play(nextAttack, 0);
                
                isInCombo = true;

                float nextAttackTime = animator.GetCurrentAnimatorStateInfo(0).length * comboExecutionWindowPercentage;
                nextAttackCooldown = Time.time + nextAttackTime;
                
                if(comboIndex >= 2)
                {
                    comboIndex = 0;
                }
                else
                {
                    comboIndex++;
                }
            }

        }

        if (isInCombo == true && Time.time >= nextAttackCooldown + (animator.GetCurrentAnimatorStateInfo(0).length * (1 - comboExecutionWindowPercentage)))
        {
            EndCombo();
        }
    }

    private void StartCombo()
    {
        isInCombo = true;
        PlayerController.instance.ToggleMove(false);
    }

    private void EndCombo()
    {
        isInCombo = false;
        comboIndex = 0;
        PlayerController.instance.ToggleMove(true);
        animator.Play("Walk Tree", 0);
    }

    public void MeleeAttackToggle(bool toggle)
    {
        canAttack = toggle;
    }

    //LÓGICA DE DANO
}
