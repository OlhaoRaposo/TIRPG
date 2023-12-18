using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject charactersPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisablePanels();
        }
    }
    public void CallStartGame()
    {
        SceneController.instance?.LoadSceneByIndex(1);
    }
    public void ToggleOptions()
    {
        DisablePanels();
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }
    public void ToggleCredits()
    {
        DisablePanels();
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }
    public void ToggleCharacters()
    {
        DisablePanels();
        charactersPanel.SetActive(!charactersPanel.activeSelf);
    }
    public void DisablePanels()
    {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        charactersPanel.SetActive(false);
    }
}
