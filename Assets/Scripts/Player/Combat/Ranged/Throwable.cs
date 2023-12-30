using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class Throwable : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float damage = 50f;
    [SerializeField] float detonationTime = 2f;
    [SerializeField] float explosionRadius = 2f;
    [SerializeField] LayerMask detectionMask;

    void OnEnable()
    {
        //Invocar a explosao
        Invoke("Explode", detonationTime);
    }
    void Explode()
    {
        //Explodir
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, detectionMask);

        if (colliders.Length > 0)
        {
            foreach(Collider c in colliders)
            {
                if (c.CompareTag("Enemy"))
                {
                    //Dar dano nos inimigos
                    c.gameObject.SendMessage("TakeDamage", damage);
                }
            }
        }

        //Instanciar particulas
        if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
