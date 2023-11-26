using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Rocks : MonoBehaviour, IInteractable
{
    [SerializeField] Transform galonPosition;
    [SerializeField] GameObject particles;
    Transform item;
   public void Interact(PlayerInteractions player)
    {
        item.SetParent(galonPosition);
        item.localPosition = Vector3.zero;

        Invoke("DestroyRocks", 5f);
    }

    public void DestroyRocks()
    {
        item.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Instantiate(particles, transform.position, Quaternion.identity);
    }
}
