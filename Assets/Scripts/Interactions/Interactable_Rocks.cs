using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Rocks : MonoBehaviour, IInteractable
{
    [SerializeField] Transform galonPosition;
    [SerializeField] GameObject rocksObject;
    [SerializeField] GameObject particles;
    [SerializeField] ItemData requiredItem;
    GameObject item;
   public void Interact(PlayerInteractions player)
    {
        //item.SetParent(galonPosition);
        //item.localPosition = Vector3.zero;
        if (PlayerInventory.instance.LookForItem(requiredItem))
        {
            item = PlayerInventory.instance.DropItem(requiredItem);
            item.transform.SetParent(galonPosition);
            item.transform.localPosition = Vector3.zero;
        }

        Invoke("DestroyRocks", 5f);
    }

    public void DestroyRocks()
    {
        EffectController.instance.InstantiateParticle(particles, transform.position);
        item.gameObject.SetActive(false);
        gameObject.SetActive(false);
        rocksObject.SetActive(false);
        //Instantiate(particles, transform.position, Quaternion.identity);
    }
}
