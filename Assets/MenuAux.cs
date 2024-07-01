using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuAux : MonoBehaviour
{
    public static MenuAux instance;
    public GameObject continueBttn,startBttn,areYouSure;
    public Text startText;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SaveController.instance.MenuStart();
    }

    public void StartGame() {
        SaveController.instance.LoadGame();
        WorldController.worldController.StartGame();
    }
    public void AreYouSure() {
        if(SaveController.instance.FileExists())
            areYouSure.gameObject.SetActive(true);
        else {
            WorldController.worldController.StartGame();
        }
    }
    public void ResetSave() {
        SaveController.instance.DeleteSave();
        WorldController.worldController.StartGame();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectQuality(TMP_Dropdown value) {
        QualitySettings.SetQualityLevel(value.value,true);
        
    }
}
