using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeCombat : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float comboExecutionWindow = .5f;
    [SerializeField] BoxCollider attackCollider;

    bool isInCombo = false;
    bool canAttack = true;
    int comboIndex = 0;
    float nextComboAttack;

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            if (!isInCombo)
            {
                EnableCombo();
            }
            
            comboIndex = (comboIndex + 1) % 4;

            if (comboIndex == 0)
            {
                if (isInCombo)
                {
                    DisableCombo();
                }
            }
            else
            {
                animator.SetInteger("ComboIndex", comboIndex);
                animator.SetTrigger("Attack");
                StartCoroutine(ComboExecutionWindow());
            }
        }
        
        if (isInCombo)
        {
            if (Time.time > nextComboAttack)
            {
                DisableCombo();
            }
        }
    }
    void EnableCombo()
    {
        isInCombo = true;
        //canAttack = false;
        animator.SetBool("CanAttack", canAttack);
        animator.SetBool("IsInCombo", isInCombo);
    }
    void DisableCombo()
    {
        isInCombo = false;
        //canAttack = true;
        comboIndex = 0;
        animator.SetBool("CanAttack", canAttack);
        animator.SetInteger("ComboIndex", comboIndex);
        animator.SetBool("IsInCombo", isInCombo);
    }
    public void EnableCollider()
    {
        attackCollider.enabled = true;
    }
    public void DisableCollider()
    {
        attackCollider.enabled = false;
    }

    IEnumerator ComboExecutionWindow()
    {
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .75f)
            {
                nextComboAttack = Time.time + comboExecutionWindow;
                canAttack = true;
                animator.SetBool("CanAttack", canAttack);
                break;
            }
            else
            {
                canAttack = false;
                animator.SetBool("CanAttack", canAttack);
            }

            yield return null;
        }
    }
}
