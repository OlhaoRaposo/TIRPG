using System.Collections;
using UnityEngine;

public class ArmadeiraWeapon : MonoBehaviour
{
    public EnemyBehaviour user;
    public float damage;
    public float cadence;
    public void Attack()
    {
        StartCoroutine(CadenceTime(user.energy));
    }
     IEnumerator CadenceTime(float energy)
     {
         if (user.isAttacking) {
             user.myNavMeshAgent.enemyAgent.SetDestination(user.enemyTarget.transform.position);
             if(energy >= 50) {
                 EspecialAtack();
                 user.energy-=100;
             }else {
                 int odds = Random.Range(0, 100);
                    if (odds <= 50) {
                        NormalAtack();
                    } else {
                        NormalAreaAtack();
                    }
             }
             yield return new WaitForSeconds(cadence);
             StartCoroutine(CadenceTime(user.energy));
         }
     }
     public float CalculateDistance()
     {
         Vector3 distance = user.enemyTarget.transform.position - user.transform.position;
         return distance.magnitude;
     }
    public void NormalAtack()
    {
        user.myNavMeshAgent.enemyAgent.SetDestination(user.enemyTarget.transform.position);
        user.transform.LookAt(user.enemyTarget.transform.position);
        Animator userAnimator = user.gameObject.TryGetComponent(out Animator animator) ? animator : null; 
        if (CalculateDistance() > 6) {
            int odds0 = Random.Range(0, 100);
            if (odds0 <= 25) {
                EspecialAtack();
            } 
        }else {
            int odds = Random.Range(0, 100);
            if (odds <= 50) {
                userAnimator.SetTrigger("NormalAttackL");
            } else {
                userAnimator.SetTrigger("NormalAttackR");
            }
        }
    }
    public void NormalAreaAtack()
    {
        user.myNavMeshAgent.enemyAgent.SetDestination(user.enemyTarget.transform.position);
        user.transform.LookAt(user.enemyTarget.transform.position);
        if (CalculateDistance() > 6) {
            int odds0 = Random.Range(0, 100);
            if (odds0 <= 25) {
                EspecialAtack();
            } 
        }else {
            Animator userAnimator = user.gameObject.TryGetComponent(out Animator animator) ? animator : null; 
            userAnimator.SetTrigger("ComboAttack");
        }
    }
    public void EspecialAtack()
    {
        user.myNavMeshAgent.enemyAgent.SetDestination(user.enemyTarget.transform.position);
        Animator userAnimator = user.gameObject.TryGetComponent(out Animator animator) ? animator : null; 
        userAnimator.SetTrigger("EspecialAttack");
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerHPController.instance.ChangeHP(damage, true);
        }
    }
}