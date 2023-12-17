using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] float interactRadius = .05f;
    [SerializeField] float interactDistance = 2f;
    [SerializeField] LayerMask interactLayer;

    IInteractable interactable;
    bool canInteract = false;

    void Update()
    {
        if (interactable == null || UIManager.instance.GetIsInMenus()) return;
        
        if (canInteract)
        {
            InteractTooltip.instance.SetHighlightInteraction(true);
            if (Input.GetKeyDown(InputController.instance.interaction))
            {
                interactable.Interact(this);
            }
        }
        else
        {
            InteractTooltip.instance.SetHighlightInteraction(false);
        }
        
    }
    void FixedUpdate()
    {
        SphereCheck();
        RaycastCheck();
    }
    void RaycastCheck()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.SphereCast(ray, interactRadius, out RaycastHit hit, 10f, interactLayer))
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
        bool canTake = PlayerInventory.instance.AddItemToInventory(item);
        return canTake;
    }

    public void TakeQuest(/* referência da quest */)
    {
        //Adicionar quest à lista de quests
    }
}