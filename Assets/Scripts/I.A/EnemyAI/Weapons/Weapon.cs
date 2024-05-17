using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [Header("Stats")]
    public float damage;
    public float cadence;
    public bool isPrincipal;
    public EnemyBehaviour enemy;
    [SerializeField] string attackSelected;
    public EnemySelection selection;
    [Header("Case Shoots")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunRoot;
    public enum EnemySelection {
        Lobo,Aranha,Boitata,Robo,
    }
    public void SetUser(EnemyBehaviour en) {
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
        if (attackSelected.Contains("Shoot")) { 
            enemy.agent.SetDestination(enemy.transform.position);
            yield return new WaitForSeconds(.5f);
                Shoot(); 
                yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(cadence);
        enemy.isAtacking = false;
        enemy.ChangeState(new ChaseState(enemy));
    }

    void Shoot(){
        enemy.transform.LookAt(enemy.target.transform.position);
        Instantiate(bullet, gunRoot.transform.position, gunRoot.transform.rotation);
    }
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            PlayerHPController.instance.ChangeHP(damage, true);
        }
    }
}