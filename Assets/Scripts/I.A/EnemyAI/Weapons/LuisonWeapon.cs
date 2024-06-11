using System.Collections;
using UnityEngine;
public class LuisonWeapon : MonoBehaviour
{
    public EnemyBehaviour user;
    [SerializeField] private float damage;

    public void Attack(string expression){
        Debug.Log("Attacking with: " + expression);
        switch (expression) {
            case "_meleeL":
                StartCoroutine(Melee(expression));
                break;
            case "_meleeR":
                StartCoroutine(Melee(expression));
                break;
            case "_heavy":
                StartCoroutine(Melee(expression));
                break;
            case "_Jump":
                StartCoroutine(Jump(expression));
                break;
            case "None":
                StartCoroutine(None());
                break;
        }
    }
    IEnumerator Melee(string expression) {
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(1.5f);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator None() {
        Vector3 offset = user.transform.position - user.target.transform.position;
        user.agent.SetDestination(user.target.transform.position + offset.normalized * 2);
        yield return new WaitForSeconds(1);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator Jump(string expression)
    {
        Vector3 dir = user.target.transform.position - user.transform.position;
        user.agent.SetDestination(user.target.transform.position + dir.normalized);
        user.enemyAnimator.SetTrigger(expression);
        yield return new WaitForSeconds(3);
        user.ChangeState(new ChaseState(user));
    }
    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Player")) {
            PlayerHPController.instance.ChangeHP(damage,true);
            Debug.Log("HIT");
        }
    }






















}
