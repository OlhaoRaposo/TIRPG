using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Door : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;

    [SerializeField] bool isOpen;
    public void Interact(PlayerInteractions player)
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            animator.SetTrigger("Close");
        }
    }
}
