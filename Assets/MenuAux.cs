using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class MenuAux : MonoBehaviour
{
    public void StartGame()
    {
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
