using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BoitataWeapon : MonoBehaviour
{
    public EnemyBehaviour user;
    public GameObject fireball;
    public float damage;
    public GameObject jail;
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
       if(Input.GetKeyDown(KeyCode.L))
           StartCoroutine(MeleeBreath("_meleeBreath")); 
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
        jail.SetActive(true);
        yield return new WaitForSeconds(8);
        jail.SetActive(false);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator MeleeBreath(string expression) {
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 300; i++) {
            for (int j = 0; j < 4; j++) {
                Quaternion randomBulletDirection =Quaternion.Euler(UnityEngine.Random.Range(0,180), UnityEngine.Random.Range(0, 180), UnityEngine.Random.Range(0, 180));
                GameObject bullet = Instantiate(fireball, transform.position,randomBulletDirection);
                bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Force);
            }
            yield return new WaitForSeconds(.00001f);
        }
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
            Debug.Log("HIT");
        }
    }
    public void TakeDamage(float damage, DamageElementManager.DamageElement damageElement) {
        user.gameObject.SendMessage("TakeDamage", damage, (SendMessageOptions)DamageElementManager.DamageElement.Physical);
    }
}