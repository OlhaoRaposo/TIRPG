using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float timeToDestroy;
    [SerializeField] 
    private bool moveFoward;

    private void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if(moveFoward)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            PlayerHPController.instance.ChangeHP(damage, true);
            other.gameObject.GetComponent<PlayerMovement>().TakeKnockback();
        }else if(other.gameObject.CompareTag("Enemy")) {
            if(other.gameObject.TryGetComponent(out EnemyBehaviour enemyBehaviour))
                enemyBehaviour.TakeDamage(damage, DamageElementManager.DamageElement.Physical);
        }
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other != null)
            Destroy(gameObject);
    }
}
