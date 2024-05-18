using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMeleeCombat : MonoBehaviour
{
    public static PlayerMeleeCombat instance;

    [Header("Variables")]
    [SerializeField] private string weaponName;
    float damage;
    float comboExecutionWindowPercentage = 0.8f;
    float[] comboWindowTime;
    int comboIndex = 0;
    float nextAttackCooldown = 0;
    bool canAttack = true;
    bool isInCombo = false;

    [Header("References")]
    [SerializeField] PlayerMeleeBase weapon;
    //[SerializeField] PlayerController player;
    [SerializeField] Animator animator;
    [SerializeField] Collider attackCollider;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        comboIndex = 1;
    }

    private void OnDisable()
    {
        EndCombo();
    }

    private void Start()
    {
        SetNewMeleeWeapon(weapon);
        this.enabled = false;
    }

    private void Update()
    {
        MeleeAttack();
    }

    public void SetNewMeleeWeapon(PlayerMeleeBase newWeapon)
    {
        damage = newWeapon.damage;
        comboExecutionWindowPercentage = newWeapon.comboExecutionWindowPercentage;
        weaponName = newWeapon.modelName;
    }

    public string GetMeleeName()
    {
        return weaponName;
    }

    private void MeleeAttack()
    {
        if (Input.GetMouseButtonDown(0) && canAttack && PlayerMovement.instance.GetIsGrounded())
        {
            PlayerCameraMovement.instance.playerAnimator.SetLayerWeight(1, 0);
            if (comboIndex == 1)
            {
                StartCombo();
            }

            if (Time.time >= nextAttackCooldown)
            {
                string nextAttack = $"{weaponName} " + comboIndex;

                PlayerCameraMovement.instance.AlignTargetWithCamera(PlayerCameraMovement.instance.cameraBody.gameObject);
                animator.Play(nextAttack, 0);
                
                isInCombo = true;

                float nextAttackTime = animator.GetCurrentAnimatorStateInfo(0).length * comboExecutionWindowPercentage;
                nextAttackCooldown = Time.time + nextAttackTime;
                
                if(comboIndex >= 4)
                {
                    animator.Play($"{weaponName} Heavy", 0);
                    comboIndex = 1;
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
        PlayerMovement.instance?.ToggleMove(false);
    }

    private void EndCombo()
    {
        isInCombo = false;
        comboIndex = 0;
        PlayerMovement.instance?.ToggleMove(true);
        animator.Play("Walk Tree", 0);
    }

    public void MeleeAttackToggle(bool toggle)
    {
        canAttack = toggle;
    }

    //LÓGICA DE DANO
}
