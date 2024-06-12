using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_FastTravel : MonoBehaviour, IInteractable
{
    public bool isTutorial = false;
    public bool isBossRoom = false;
    public void Interact(PlayerInteractions player)
    {
        if (isTutorial) {
            PlayerMovement.instance.TeleportPlayer(new Vector3(648.67f,82.11f,235.04f));
            return;
        }
        if (isBossRoom) {
            PlayerMovement.instance.TeleportPlayer(new Vector3(322.545f,82.2154f,-760.2823f));
            return;
        }
        UIManager.instance.ToggleFastTravelMenu();
    }
}
