using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] GameObject inGameMenusParent;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject statsPanel;
    [SerializeField] GameObject questsPanel;
    [SerializeField] GameObject optionsPanel;

    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject staminaBar;

    [SerializeField] GameObject equipedWeapons;

    [SerializeField] Text xpText;

    [SerializeField] Text strengthText;
    [SerializeField] Text dexterityText;
    [SerializeField] Text enduranceText;

    [SerializeField] Text healthPointsText;
    [SerializeField] Text staminaPointsText;

    [SerializeField] Text availablePointsText;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //As próximas linhas servem somente para inicializar os slots do inventário (vou tirar isso dps)
        ToggleInGameMenus();
        Invoke("ToggleInGameMenus", .1f);
    }

    public void UpdateXpStats(int xp, int maxXp)
    {
        xpText.text = $"XP: {xp}/{maxXp}";
    }
    public void UpdateHealthStats(int hp, int maHXp)
    {
        healthPointsText.text = $"Health {hp}/{maHXp}";
    }
    public void UpdateStaminaStats(int stamina, int maxStamina)
    {
        staminaPointsText.text = $"Stamina {stamina}/{maxStamina}";
    }
    public void UpdateStrength(int strength)
    {
        strengthText.text = "Strength: " + strength.ToString();
    }
    public void UpdateDexterity(int dexterity)
    {
        dexterityText.text = "Dexterity: " + dexterity.ToString();
    }
    public void UpdateEndurance(int endurance)
    {
        enduranceText.text = "Endurance: " + endurance.ToString();
    }
    public void UpdateAvailablePoints(int points)
    {
        availablePointsText.text = points.ToString();
    }
    public void ToggleInGameMenus()
    {
        inGameMenusParent.SetActive(!inGameMenusParent.activeSelf);
        DisableAllPanels();
        ToggleInventoryPanel();
    }
    public void ToggleInventoryPanel()
    {
        DisableAllPanels();
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
    public void ToggleStatsPanel()
    {
        DisableAllPanels();
        statsPanel.SetActive(!inventoryPanel.activeSelf);
    }
    public void ToggleQuestsPanel()
    {
        DisableAllPanels();
        questsPanel.SetActive(!inventoryPanel.activeSelf);
    }
    public void ToggleOptionsPanel()
    {
        DisableAllPanels();
        optionsPanel.SetActive(!inventoryPanel.activeSelf);
    }
    void DisableAllPanels()
    {
        inventoryPanel?.SetActive(false);
        statsPanel?.SetActive(false);
        questsPanel?.SetActive(false);
        optionsPanel?.SetActive(false);
    }
    public void ToggleHUD()
    {
        healthBar.SetActive(!healthBar.activeSelf);
        staminaBar.SetActive(!staminaBar.activeSelf);

        equipedWeapons.SetActive(!equipedWeapons.activeSelf);
    }
    public void CallIncreaseStrength()
    {
        PlayerStats.instance.IncreaseStrength();
    }
    public void CallIncreaseDexterity()
    {
        PlayerStats.instance.IncreaseDexterity();
    }
    public void CallIncreaseEndurance()
    {
        PlayerStats.instance.IncreaseEndurance();
    }
}
