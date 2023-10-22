using System;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float damage;
    [SerializeField] private float fallOffRange;
    [SerializeField] private float travelSpeed;
    [SerializeField] private float lifeTime;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * travelSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, startPos) >= fallOffRange)
        {
            travelSpeed -= 0.1f;
            damage -= 0.1f;
            if(damage <= 0 || travelSpeed <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("TakeDamage", damage);
        }
        Destroy(gameObject);
        
        if (other!= null)
        {
            Destroy(this.gameObject);
        }
    }
}
