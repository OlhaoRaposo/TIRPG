using TMPro;
using UnityEngine;

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
