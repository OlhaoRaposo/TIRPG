using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverTpScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
            if(col.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement pm))
        {
            pm.TeleportPlayer(new Vector3(695.79f, 62.19f, 180.25f));
        }
    }
}
