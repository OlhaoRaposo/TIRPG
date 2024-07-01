using System;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialbttn;
    public static TutorialManager instance;

    private void Awake() {
        instance = this;
    }

    public void ActiveTutorial() {
        tutorialbttn.SetActive(true);
    }
    public void CompleteTutorial() {
        WorldController.worldController.tutorialCompleted = true;
        PlayerMovement.instance.TeleportPlayer(new Vector3(648.67f,82.11f,235.04f));
    }
}
