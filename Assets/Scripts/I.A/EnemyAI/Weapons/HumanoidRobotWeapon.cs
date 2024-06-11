using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HumanoidRobotWeapon : MonoBehaviour
{
    public EnemyBehaviour user;
    public GameObject bullet;
    public GameObject round;
    private bool isAttacking;
    public Transform gun1;
    
    public void Attack(string expression){
        Debug.Log("Attacking with: " + expression);
        switch (expression) {
            case "_shoot1":
                StartCoroutine(Shoot());
              break;
            case "_shoot2":
                StartCoroutine(Shoot2());
                break;
            case "None":
                StartCoroutine(None());
                break;
        }
        isAttacking = true;
    }

    private void Update()
    {
       if(isAttacking)
           user.transform.LookAt(user.target.transform.position); 
    }

    private IEnumerator Shoot(){ 
        Vector3 offset = user.transform.position - user.target.transform.position;
        user.agent.SetDestination(user.target.transform.position + offset.normalized * 2);
        user.transform.LookAt(user.target.transform.position); 
        yield return new WaitForSeconds(.2f);
        Vector3 roundRecoil = gun1.position + new Vector3(10,10,-15);
        
        user.enemyAnimator.SetLayerWeight(1, 1);
        user.enemyAnimator.SetTrigger("_shoot");
       
        Instantiate(bullet, gun1.position, gun1.rotation);
        yield return new WaitForSeconds(.1f);
        GameObject rnd = Instantiate(round, gun1.position, gun1.rotation);
        rnd.TryGetComponent(out Rigidbody rb);
        rb.AddForce(roundRecoil, ForceMode.Force);
       
        yield return new WaitForSeconds(.1f);
        user.ChangeState(new ChaseState(user));
    }
    private IEnumerator Shoot2(){ 
        Vector3 offset = user.transform.position - user.target.transform.position;
        user.agent.SetDestination(user.target.transform.position + offset.normalized * 2);
        user.transform.LookAt(user.target.transform.position); 
        
        Vector3 roundRecoil = gun1.position + new Vector3(10,25,-25);
        
        user.enemyAnimator.SetLayerWeight(1, 1);

        for (int i = 0; i < 4; i++)
        {
            user.enemyAnimator.SetTrigger("_shoot");
            Instantiate(bullet, gun1.position, gun1.rotation);
            yield return new WaitForSeconds(.1f);
            GameObject rnd = Instantiate(round, gun1.position, gun1.rotation);
            rnd.TryGetComponent(out Rigidbody rb);
            rb.AddForce(roundRecoil, ForceMode.Force);
            yield return new WaitForSeconds(.3f);
        }
        
       
        yield return new WaitForSeconds(.6f);
        user.ChangeState(new ChaseState(user));
    }
    
    private IEnumerator None() {
        Vector3 offset = user.transform.position - user.target.transform.position;
        offset = offset.normalized * 3;
        offset += Random.insideUnitSphere * 4;
        user.agent.SetDestination(offset);
        yield return new WaitForSeconds(Random.Range(2,3));
        user.ChangeState(new ChaseState(user));
    }
    
}
