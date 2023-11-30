using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCanvas : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
