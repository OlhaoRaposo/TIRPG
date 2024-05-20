using System;
using System.Collections;
using UnityEngine;

public class BoitataWeapon : MonoBehaviour
{
    public EnemyBehaviour user;
    public float damage;
    public bool isHitting;
    public void Attack(string expression){
        Debug.Log("Attacking with: " + expression);
        
        switch (expression) {
            case "_meleeBite":
                StartCoroutine( MeleeBite(expression));
                break;
            case "_meleeBreath":
                StartCoroutine(MeleeBreath(expression));
                break;
            case "_meleeLethal":
                StartCoroutine(MeleeHit(expression));
                break;
            case "_rangedBite":
                StartCoroutine( RangedBite(expression));
                break;
            case "_devour":
                StartCoroutine(Devour(expression));
                break;
        }
    }
    private void Update() {
    }

    IEnumerator Devour(string expression) {
        Vector3 lookat = user.target.transform.position - user.transform.position;
        user.agent.SetDestination(lookat * 10);
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(5);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator RangedBite(string expression) {
        user.transform.LookAt(user.target.transform.position);
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(8);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator MeleeHit(string expression) {
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(8);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator MeleeBreath(string expression)
    {
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(8);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator MeleeBite(string expression) {
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(8);
        user.ChangeState(new ChaseState(user));
    }
    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Player")) {
            PlayerHPController.instance.ChangeHP(damage,true);
            isHitting = true;
            Debug.Log("HIT");
        }else
            isHitting = false;
    }
   
    public void TakeDamage(float damage, DamageElementManager.DamageElement damageElement) {
        user.gameObject.SendMessage("TakeDamage", damage, (SendMessageOptions)DamageElementManager.DamageElement.Physical);
    }
}
