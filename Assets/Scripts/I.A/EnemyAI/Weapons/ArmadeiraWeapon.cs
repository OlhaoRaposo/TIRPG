using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArmadeiraWeapon : MonoBehaviour
{
    [SerializeField] private List<Transform> gunRoots;
     public GameObject normalBullet;
     public GameObject especialBullet;
     public GameObject target;
     public EnemyBehaviour user;
     private bool isShooting;

     public void Attack(string expression){
         Debug.Log("Attacking with: " + expression);
        
        switch (expression) {
           case "_melee1":
               StartCoroutine(MeleeHit(expression));
               break;
           case "_melee2":
               StartCoroutine(MeleeHit(expression));
               break;
           case "_melee3":
               StartCoroutine(MeleeHit(expression));
               break;
           case "_ranged1":
               StartCoroutine(NormalShoot(4,expression));
               break;
           case "_ranged2":
               StartCoroutine(NormalShoot(12,expression));
               break;
           case "_esp1":
               StartCoroutine(EspecialShoot(4,expression));
               break;
           case "_esp2":
               StartCoroutine(EspecialShoot(12,expression));
               break;
           case "_Jump":
               StartCoroutine(Jump(expression));
               break;
         }
     }

     private void Update()
     {
         if (isShooting) {
             Transform userTransform = user.transform;
             Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - userTransform.position);
             userTransform.rotation = Quaternion.Lerp(userTransform.rotation, targetRotation, .15f * Time.deltaTime);
         }
     }

     IEnumerator MeleeHit(string expression) {
         user.enemyAnimator.SetTrigger(expression);
         yield return new WaitForSeconds(1);
         user.ChangeState(new ChaseState(user));
     }
     IEnumerator NormalShoot(int shoots,string expression) {
         isShooting = true;
         user.agent.SetDestination(user.transform.position);
            for (int i = 0; i < shoots; i++){
                user.enemyAnimator.SetTrigger(expression);
                user.transform.LookAt(user.target.transform.position);
                int rndRoot = Random.Range(0, gunRoots.Count);
                GameObject bullet = Instantiate(normalBullet, gunRoots[rndRoot].position, gunRoots[rndRoot].rotation);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1);
            user.ChangeState(new ChaseState(user));
            isShooting = false;
     }
     IEnumerator EspecialShoot(int shoots,string expression){
         isShooting = true;
         user.agent.SetDestination(user.transform.position);
         yield return new WaitForSeconds(1);
         user.enemyAnimator.SetTrigger(expression);
         user.enemyAnimator.SetBool("bastionActive", true);
         yield return new WaitForSeconds(2);
         List<GameObject> targets = null;
         Vector3[] points = new Vector3[shoots];
         
         for (int i = 0; i < shoots; i++) {
             points[i] = user.target.transform.position + Random.insideUnitSphere * 6;
             GameObject tr = Instantiate(target, new Vector3(points[i].x, 0, points[i].z), Quaternion.identity);
             yield return new WaitForSeconds(.1f);
         }
         yield return new WaitForSeconds(.5f);
         for (int i = 0; i < shoots; i++) {
            Instantiate(especialBullet, points[i] + new Vector3(0, 10,0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
         }
         
         yield return new WaitForSeconds(.5f);
         user.enemyAnimator.SetBool("bastionActive", false);
         yield return new WaitForSeconds(.5f);
         isShooting = false;
         user.ChangeState(new ChaseState(user));
     }
     IEnumerator Jump(string expression) {
         user.transform.LookAt(user.transform.forward);
         user.enemyAnimator.SetTrigger(expression);
         user.transform.LookAt(user.transform.forward);
         yield return new WaitForSeconds(3);
         user.ChangeState(new ChaseState(user));
     }
}
