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
    [SerializeField] GameObject shopPanel;

    [SerializeField] ShopSlot[] shopSlots_buy;
    [SerializeField] ShopSlot[] shopSlots_sell;
    MerchantInventory currentMerchant;

    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject staminaBar;

    [SerializeField] GameObject equipedWeapons;

    [SerializeField] GameObject crosshair;

    [SerializeField] Text feedbackText;

    [SerializeField] Text xpText;

    [SerializeField] Text strengthText;
    [SerializeField] Text dexterityText;
    [SerializeField] Text enduranceText;
    [SerializeField] Text intelligenceText;

    [SerializeField] Text healthPointsText;
    [SerializeField] Text staminaPointsText;

    [SerializeField] Text availablePointsText;

    bool isInMenus = false;
    bool cursorState = true;
    void Awake()
    {
        instance = this;
        //Cursor.lockState = CursorLockMode.None;
    }
    /*void Start()
    {
        //As próximas linhas servem somente para inicializar os slots do inventário (vou tirar isso dps)
        ToggleInGameMenus();
        Invoke("ToggleInGameMenus", .1f);
    }*/
    void Update()
    {
        if (currentMerchant != null)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                ToggleShopPanel();
            }
        }
    }
    public ShopSlot[] GetShopSlotsBuy()
    {
        return shopSlots_buy;
    }
    public ShopSlot[] GetShopSlotsSell()
    {
        return shopSlots_sell;
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
    public void UpdateIntelligence(int intelligence)
    {
        intelligenceText.text = "Intelligence: " + intelligence.ToString();
    }
    public void UpdateAvailablePoints(int points)
    {
        availablePointsText.text = points.ToString();
    }
    public void ShowTextFeedback(string s)
    {
        feedbackText.gameObject.SetActive(true);
        feedbackText.text = s;

        Invoke("HideTextFeedback", 3f);
    }
    public void HideTextFeedback()
    {
        feedbackText.gameObject.SetActive(false);
    }
    public void ToggleCrosshair()
    {
        crosshair?.SetActive(!crosshair.activeSelf);
    }
    public void ToggleInGameMenus()
    {
        ToggleCursorLockMode();
        isInMenus = !isInMenus;

        if (currentMerchant == null)
        {
            inGameMenusParent.SetActive(!inGameMenusParent.activeSelf);
            DisableAllPanels();
            ToggleCrosshair();
            ToggleInventoryPanel();
        }
        else
        {
            DisableAllPanels();
        }
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
    public void ToggleShopPanel(MerchantInventory merchantInventory)
    {
        isInMenus = !isInMenus;

        DisableAllPanels();
        ToggleCursorLockMode();
        ToggleCrosshair();
        currentMerchant = merchantInventory;
        shopPanel.SetActive(!shopPanel.activeSelf);
        merchantInventory.SetShopUI();
    }
    void ToggleShopPanel()
    {
        isInMenus = !isInMenus;

        DisableAllPanels();
        ToggleCursorLockMode();
        ToggleCrosshair();
        currentMerchant = null;
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
    void ToggleCursorLockMode()
    {
        cursorState = !cursorState;
        PlayerCamera.instance.ToggleAimLock(cursorState);
    }
    void DisableAllPanels()
    {
        inventoryPanel?.SetActive(false);
        statsPanel?.SetActive(false);
        questsPanel?.SetActive(false);
        optionsPanel?.SetActive(false);
        shopPanel?.SetActive(false);

        currentMerchant = null;
    }
    public void ToggleHUD()
    {
        healthBar.SetActive(!healthBar.activeSelf);
        staminaBar.SetActive(!staminaBar.activeSelf);

        equipedWeapons.SetActive(!equipedWeapons.activeSelf);
    }
    public bool GetIsInMenus()
    {
        return isInMenus;
    }
    public MerchantInventory GetCurrentMerchant()
    {
        return currentMerchant;
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
    public void CallIncreaseIntelligence()
    {
        PlayerStats.instance.IncreaseIntelligence();
    }
    public void CallMainMenu()
    {
        SceneController.instance.LoadSceneByIndex(0);
    }
    public void CallQuitGame()
    {
        SceneController.instance.QuitGame();
    }
}
