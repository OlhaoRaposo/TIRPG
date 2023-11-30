using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject creditsPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisablePanels();
        }
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
    void DisablePanels()
    {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
}
