using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage;
    public float cadence;
    public bool isPrincipal;
    public Enemy enemy;
    private string attackSelected;
    public void SetUser(Enemy en) {
        enemy = en;
    }
    public void Attack(string attackSelected) {
        Debug.Log("Attacking with: " + attackSelected);
       this.attackSelected = attackSelected;
        if(isPrincipal)
            if(!enemy.isAtacking)
                StartCoroutine(AttackPlayer());
    }
    IEnumerator AttackPlayer() {
        enemy.isAtacking = true;
        enemy.enemyAnimator.SetTrigger(attackSelected);
        yield return new WaitForSeconds(cadence);
        enemy.isAtacking = false;
        enemy.ChangeState(new ChaseState(enemy));
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            PlayerHPController.instance.ChangeHP(damage, true);
        }
    }
}
