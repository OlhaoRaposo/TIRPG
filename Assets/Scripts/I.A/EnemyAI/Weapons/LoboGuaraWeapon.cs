using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoboGuaraWeapon : MonoBehaviour
{
    public EnemyBehaviour user;
    public float damage;
    public float cadence;
    public bool isPrincipal;
    
    public void Attack()
    {
        if(isPrincipal)
            StartCoroutine(CadenceTime(user.energy));
    }
    IEnumerator CadenceTime(float energy)
    {
          if (user.isAttacking)
            {
                user.myNavMeshAgent.enemyAgent.SetDestination(user.enemyTarget.transform.position);

                int odds = Random.Range(0, 100);
                {
                    if (odds <= 50)
                        NormalAtack();
                    else if (odds > 0 && odds <= 49)
                        NormalAreaAtack();
                    else
                        EspecialAtack();
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
        if (CalculateDistance() <= 6)
        {
            Animator userAnimator = user.gameObject.TryGetComponent(out Animator animator) ? animator : null; 
            userAnimator.SetTrigger("AttackL");
        }
    }
    public void NormalAreaAtack()
    {
        user.myNavMeshAgent.enemyAgent.SetDestination(user.enemyTarget.transform.position);
        if (CalculateDistance() <= 6)
        {
            Animator userAnimator = user.gameObject.TryGetComponent(out Animator animator) ? animator : null; 
            userAnimator.SetTrigger("AttackR");
        }
    }
    public void EspecialAtack()
    {
        user.myNavMeshAgent.enemyAgent.SetDestination(user.enemyTarget.transform.position);
        if (CalculateDistance() <= 6)
        {
            Animator userAnimator = user.gameObject.TryGetComponent(out Animator animator) ? animator : null; 
            userAnimator.SetTrigger("HeavyAttack");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerHPController.instance.ChangeHP(damage, true);
        }
    }
}
