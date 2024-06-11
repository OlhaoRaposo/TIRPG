using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_FastTravel : MonoBehaviour, IInteractable
{
    public bool isTutorial = false;
    public void Interact(PlayerInteractions player)
    {
        if (isTutorial) {
            PlayerMovement.instance.TeleportPlayer(new Vector3(700,64,170));
            return;
        }
        UIManager.instance.ToggleFastTravelMenu();
    }
}
