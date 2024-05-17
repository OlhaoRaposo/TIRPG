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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) {
            PlayerHPController.instance.ChangeHP(damage, true);
        }else if(other.gameObject.CompareTag("Enemy")) {
            other.gameObject.SendMessage("TakeDamage", damage, (SendMessageOptions)DamageElementManager.DamageElement.Physical);
        }
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other != null)
            Destroy(gameObject);
    }
}
