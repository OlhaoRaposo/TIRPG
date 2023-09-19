using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] float interactRadius = 1f;
    [SerializeField] float interactDistance = 3f;
    [SerializeField] LayerMask interactLayer;

    void FixedUpdate()
    {
        SphereCheck();
    }

    void RaycastCheck()
    {
        Ray ray = new Ray(transform.position, Camera.main.transform.forward);

        if (Physics.SphereCast(ray, interactRadius, out RaycastHit hit, interactDistance, interactLayer))
        {
            InteractTooltip.instance.ToggleTooltip(hit.transform);
        }
    }
    void SphereCheck()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, interactDistance, interactLayer);
        
        if (col.Length > 0)
        {
            InteractTooltip.instance.ToggleTooltip(col[0].transform);
        }
    }

    public void TakeItem(GameObject item)
    {
        Debug.Log("Peguei um " + item.name);

        //Adicionar item ao inventário
    }

    public void TakeQuest(/* referência da quest */)
    {
        //Adicionar quest à lista de quests
    }
}
