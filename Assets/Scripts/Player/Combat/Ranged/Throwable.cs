using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody), typeof (BoxCollider))]
public class Throwable : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float detonationTime = 2f;
    [SerializeField] float explosionRadius = 2f;
    [SerializeField] LayerMask detectionLayer;

    void OnEnable()
    {
        //Invocar a explosao
        Invoke("Explode", detonationTime);
    }
    void Explode()
    {
        //Explodir
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, detectionLayer);

        if (colliders.Length > 0)
        {
            foreach(Collider c in colliders)
            {
                //Dar dano nos inimigos
            }
        }

        //Instanciar particulas
        if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
