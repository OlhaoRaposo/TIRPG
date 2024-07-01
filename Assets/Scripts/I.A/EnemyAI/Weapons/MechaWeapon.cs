using System;
using System.Collections;
using UnityEngine;

public class MechaWeapon : MonoBehaviour
{
    public EnemyBehaviour user;
    [SerializeField] private float damage;
    public GameObject bullet;
    public GameObject round;
    public Transform gun1,gun2;
    public Transform roundTransform;
    public GameObject stompPrefab;

    public void Attack(string expression){
        Debug.Log("Attacking with: " + expression);
        switch (expression) {
            case "_stomp":
                StartCoroutine(Stomp());
                break;
            case "_bulletHell":
                StartCoroutine(BulletHell());
                break;
            case "_slash":
                StartCoroutine(Slash());
                break;
            case "None":
                StartCoroutine(None());
                break;
        }
    }

    private IEnumerator Stomp() {
        user.enemyAnimator.SetTrigger("_stomp");
        yield return new WaitForSeconds(1.5f);
        Instantiate(stompPrefab, user.transform.position + new Vector3(0,-1,0), user.transform.rotation);
        yield return new WaitForSeconds(3.5f);
        user.ChangeState(new ChaseState(user));
    }
    private IEnumerator BulletHell()
    {
        Vector3 roundRecoil = roundTransform.position + new Vector3(10,10,-15);
        user.agent.SetDestination(user.gameObject.transform.position);
        user.enemyAnimator.SetTrigger("_bulletHell");
        yield return new WaitForSeconds(.25f);
        for (int i = 0; i < 21; i++) {
            user.gameObject.transform.LookAt(user.target.gameObject.transform.position);
            
            if(i % 2 == 0)
                Instantiate(bullet, gun2.position, gun2.rotation);
            else if(i % 2 != 0)
                Instantiate(bullet, gun1.position, gun1.rotation);
            
            yield return new WaitForSeconds(.01f);
            GameObject roundInstance = Instantiate(round, roundTransform.position, gun1.rotation);
            roundInstance.TryGetComponent(out Rigidbody rb);
            rb.AddForce(roundRecoil, ForceMode.Force);
            yield return new WaitForSeconds(.15f);
        }
        yield return new WaitForSeconds(1);
        user.ChangeState(new ChaseState(user));
    }
    private IEnumerator Slash() {
        user.enemyAnimator.SetTrigger("_slash");
        yield return new WaitForSeconds(8);
        user.ChangeState(new ChaseState(user));
    }
    IEnumerator None() {
        user.agent.SetDestination(user.target.transform.position);
        yield return new WaitForSeconds(2);
        user.ChangeState(new ChaseState(user));
    }
}
