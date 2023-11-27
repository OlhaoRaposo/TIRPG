using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] float interactRadius = 1f;
    [SerializeField] float interactDistance = 3f;
    [SerializeField] LayerMask interactLayer;

    IInteractable interactable;
    bool canInteract = false;

    void Update()
    {
        if (interactable == null) return;
        
        if (Input.GetKeyDown(InputController.instance.interaction) && canInteract)
        {
            interactable.Interact(this);
        }
        
    }
    void FixedUpdate()
    {
        SphereCheck();
        RaycastCheck();
    }
    void RaycastCheck()
    {
        Ray ray = new Ray(transform.position, Camera.main.transform.forward);

        if (Physics.SphereCast(ray, interactRadius, out RaycastHit hit, interactDistance, interactLayer))
        {
            canInteract = true;
        }
        else
        {
            canInteract = false;
        }
    }
    void SphereCheck()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, interactDistance, interactLayer);
        
        if (col.Length > 0)
        {
            interactable = col[0].GetComponent<IInteractable>();
            if(!InteractTooltip.instance.GetIsOn())
            {
                InteractTooltip.instance.ToggleTooltip(col[0].transform);
            }
            float tooltipScale = 1 - (transform.position - col[0].transform.position).magnitude/interactDistance;
            InteractTooltip.instance.SetScale(tooltipScale);
        }else {
            interactable = null;
            InteractTooltip.instance.DisableTooltip();
        }
    }
    

    public bool TakeItem(ItemData item)
    {
        Debug.Log("Peguei um " + item.name);

        //Adicionar item ao inventário
        bool canTake = GetComponent<PlayerInventory>().AddItemToInventory(item);
        return canTake;
    }

    public void TakeQuest(/* referência da quest */)
    {
        //Adicionar quest à lista de quests
    }
}