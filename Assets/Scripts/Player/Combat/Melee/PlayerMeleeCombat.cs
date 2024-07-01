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
    [SerializeField] private LayerMask collisionLayer = new LayerMask();
    [HideInInspector] public bool isInCombo = false;

    [Header("References")]
    [SerializeField] PlayerMeleeBase weapon;
    [SerializeField] GameObject target;
    [SerializeField] AudioBoard localBoard;
    [SerializeField] Animator animator;

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

        if (Input.GetMouseButtonDown(0) && canAttack && PlayerMovement.instance.GetIsGrounded() && UIManager.instance.GetIsInMenus() == false)
        {
            if (Time.time >= nextAttackCooldown)
            {
                localBoard.PlayAudio("Melee_Swing");
                PlayerCameraMovement.instance.playerAnimator.SetLayerWeight(1, 0);
                if (comboIndex == 1)
                {
                    StartCombo();
                }

                string nextAttack = $"{weaponName} " + comboIndex;

                PlayerCameraMovement.instance.AlignTargetWithCamera(PlayerCameraMovement.instance.playerObject);
                animator.Play(nextAttack, 0);
                StartCoroutine(InflictDamage());

                float nextAttackTime = animator.GetCurrentAnimatorStateInfo(0).length * comboExecutionWindowPercentage;
                nextAttackCooldown = Time.time + nextAttackTime;

                if (comboIndex >= 4)
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

    public IEnumerator InflictDamage()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * comboExecutionWindowPercentage);
        Collider[] hitEnemies = Physics.OverlapSphere(target.transform.position, weapon.reach);
        if (hitEnemies != null)
        {
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.tag == "Enemy")
                {
                    if(comboIndex >= 4)
                    {
                        enemy.transform.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(weapon.damage * 1.25f * PlayerStats.instance.GetMeleeDamageMultiplier(), weapon.damageElement);
                    }
                    else
                    {
                        enemy.transform.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(weapon.damage * PlayerStats.instance.GetMeleeDamageMultiplier(), weapon.damageElement);
                    }
                    Hitmark.instance.ToggleHitmark();
                }
            }
        }
    }

    private void StartCombo()
    {
        isInCombo = true;
    }

    private void EndCombo()
    {
        isInCombo = false;
        comboIndex = 0;
        animator.Play("Walk Tree", 0);
    }

    public void MeleeAttackToggle(bool toggle)
    {
        canAttack = toggle;
    }
}
