using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoitataWeapon : MonoBehaviour
{
    public EnemyBehaviour user;
    public float damage;
    public GameObject jail;
    private bool isOnAnimation;
    public GameObject beathPrefab;
    
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
            case "_crawl":
                StartCoroutine(Crawl(expression));
                break;
            case "None":
                int rnd = Random.Range(0, 100);
                    if(rnd<=50)
                        StartCoroutine(Devour(expression));
                    else {
                        StartCoroutine(Crawl(expression));
                    }
                break;
        }
    }

    private void Update() {
        if(user.target!=null && !isOnAnimation)
            transform.LookAt(user.target.transform.position);

        if (Input.GetKeyDown(KeyCode.M)) {
            Attack("_meleeBreath");
        }
    }

    IEnumerator Crawl(string expression) {
        isOnAnimation = true;
        Vector3 lookat = user.target.transform.position - user.transform.position;
        user.agent.SetDestination(lookat * 15);
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(5);
        isOnAnimation = false;
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator Devour(string expression) {
        isOnAnimation = true;
        Vector3 lookat = user.target.transform.position - user.transform.position;
        user.agent.SetDestination(lookat * 10);
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(5);
        isOnAnimation = false;
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator RangedBite(string expression) {
        user.transform.LookAt(user.target.transform.position);
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(6);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator MeleeHit(string expression) {
        user.enemyAnimator.SetTrigger(expression);
        jail.SetActive(true);
        yield return new WaitForSeconds(6);
        jail.SetActive(false);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator MeleeBreath(string expression)
    {
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 300; i++) {
          Rigidbody rb = Instantiate(beathPrefab, transform.position + (transform.forward * 2), Quaternion.identity).GetComponent<Rigidbody>();
          rb.AddForce(transform.forward * 500);
          yield return new WaitForSeconds(.000001f);
        }
        yield return new WaitForSeconds(6);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator MeleeBite(string expression) {
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(6);
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
