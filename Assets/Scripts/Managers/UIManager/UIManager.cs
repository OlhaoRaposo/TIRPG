using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [SerializeField] GameObject skillTreePanel;
    RectTransform skillTreeTransform;

    /*
    [SerializeField] GameObject strengthSkillTreePanel;
    [SerializeField] GameObject enduranceSkillTreePanel;
    [SerializeField] GameObject agilitySkillTreePanel;
    [SerializeField] GameObject intelligenceSkillTreePanel;
    */

    [SerializeField] Text merchantInventoryLabel;
    [SerializeField] Text shopInfluenceInfo;
    [SerializeField] ShopSlot[] shopSlots_buy;
    [SerializeField] ShopSlot[] shopSlots_sell;
    MerchantInventory currentMerchant;

    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject staminaBar;
    [SerializeField] Text lvlText_hud;

    [SerializeField] GameObject equipedWeapons;

    [SerializeField] GameObject crosshair;

    [SerializeField] Text feedbackText;

    [SerializeField] Text xpText;
    [SerializeField] Text lvlText_menu;

    [SerializeField] Text strengthText;
    [SerializeField] Text agilityText;
    [SerializeField] Text enduranceText;
    [SerializeField] Text intelligenceText;

    [SerializeField] Text healthPointsText;
    [SerializeField] Text staminaPointsText;
    [SerializeField] Text attributeDescription;

    [SerializeField] string strengthDescription;
    [SerializeField] string enduranceDescription;
    [SerializeField] string agilityDescription;
    [SerializeField] string intelligenceDescription;

    [SerializeField] Text ammoText;

    [SerializeField] Text availablePointsText;

    [SerializeField] GameObject skillPanel;
    [SerializeField] Image skillIcon;
    [SerializeField] Text skillName;
    [SerializeField] Text skillDescription;
    [SerializeField] Text skillAttributeRequirements;
    [SerializeField] Text skillPointsRequirement;
    [SerializeField] Button getSkillButton;
    Skill selectedSkill;

    string selectedAttribute = "";

    bool isInSkillTree = false;
    bool isInMenus = false;
    bool cursorState = true;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        /*
        //As pr�ximas linhas servem somente para inicializar os slots do invent�rio (vou tirar isso dps)
        ToggleInGameMenus();
        Invoke("ToggleInGameMenus", .1f);
        */

        skillTreeTransform = skillTreePanel.transform.Find("Skills").GetComponent<RectTransform>();
    }
    void Update()
    {
        if (currentMerchant != null)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                ToggleShopPanel();
            }
        }

        //Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }
    public ShopSlot[] GetShopSlotsBuy()
    {
        return shopSlots_buy;
    }
    public ShopSlot[] GetShopSlotsSell()
    {
        return shopSlots_sell;
    }
    public void UpdateHUDLevel(int lvl)
    {
        if (lvlText_hud == null) return;

        lvlText_hud.text = "Lv. " + lvl.ToString();
        UpdateMenuLevel(lvl);
    }
    public void UpdateAmmo(string s)
    {
        ammoText.text = s;
    }
    public void UpdateMenuLevel(int lvl)
    {
        lvlText_menu.text = "Level " + lvl.ToString();
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
        strengthText.text = strength.ToString();
    }
    public void UpdateAgility(int agility)
    {
        agilityText.text = agility.ToString();
    }
    public void UpdateEndurance(int endurance)
    {
        enduranceText.text = endurance.ToString();
    }
    public void UpdateIntelligence(int intelligence)
    {
        intelligenceText.text = intelligence.ToString();
    }
    public void UpdateAllAttributes()
    {
        UpdateStrength(PlayerStats.instance.GetStrength());
        UpdateAgility(PlayerStats.instance.GetAgility());
        UpdateEndurance(PlayerStats.instance.GetEndurance());
        UpdateIntelligence(PlayerStats.instance.GetIntelligence());
    }
    public void UpdateAvailablePoints(int points)
    {
        availablePointsText.text = points.ToString();
    }
    void UpdateAttributeDescription()
    {
        switch (selectedAttribute)
        {
            default: Debug.LogError("Invalid selected attribute (string)"); break;

            case "strength": attributeDescription.text = strengthDescription; break;
            case "endurance": attributeDescription.text = enduranceDescription; break;
            case "agility": attributeDescription.text = agilityDescription; break;
            case "intelligence": attributeDescription.text = intelligenceDescription; break;
        }
    }
    public void ShowTextFeedback(string s)
    {
        if (feedbackText == null) return;

        feedbackText?.gameObject.SetActive(true);
        feedbackText.text = s;

        Invoke("HideTextFeedback", 3f);
    }
    public void HideTextFeedback()
    {
        if (feedbackText == null) return;

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

        if(isInMenus == true)
        {
            PlayerController.instance?.ToggleWeaponSwap(false);
            PlayerGun.instance?.ShootToggle(false);
            PlayerMeleeCombat.instance?.MeleeAttackToggle(false);
        }
        else
        {
            PlayerController.instance?.ToggleWeaponSwap(true);
            PlayerGun.instance?.ShootToggle(true);
            PlayerMeleeCombat.instance?.MeleeAttackToggle(true);
        }

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
        UpdateAllAttributes();
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
        UpdateShopInfluenceInfo();
        UpdateMerchantNameText();
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
    void UpdateMerchantNameText()
    {
        merchantInventoryLabel.text = currentMerchant.GetInventoryData().merchantName + "'s Inventory";
    }
    public void UpdateShopInfluenceInfo()
    {
        switch (currentMerchant.GetInventoryData().influentialSide)
        {
            case LoyaltySystem.InfluentialSide.Nature:
                shopInfluenceInfo.text = $"Nature Influence:\n{LoyaltySystem.instance.GetInfluencePointsNature()}";
                break;
            case LoyaltySystem.InfluentialSide.City:
                shopInfluenceInfo.text = $"City Influence:\n{LoyaltySystem.instance.GetInfluencePointsCity()}";
                break;
        }
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
    public void SelectAttribute(string attribute)
    {
        selectedAttribute = attribute;

        UpdateAttributeDescription();
    }
    public void ToggleSkillTreePanel()
    {
        selectedSkill = null;
        DisableSelectedSkillPanel();

        skillTreePanel.SetActive(!skillTreePanel.activeSelf);
        ReseetSkillTreePosition();
        /*
        switch (selectedAttribute)
        {
            default: Debug.LogError("Invalid selected attribute (string)"); return;

            case "strength": strengthSkillTreePanel.SetActive(!strengthSkillTreePanel.activeSelf); break;
            case "endurance": enduranceSkillTreePanel.SetActive(!enduranceSkillTreePanel.activeSelf); break;
            case "agility": agilitySkillTreePanel.SetActive(!agilitySkillTreePanel.activeSelf); break;
            case "intelligence": intelligenceSkillTreePanel.SetActive(!intelligenceSkillTreePanel.activeSelf); break;
        }
        */

        statsPanel.SetActive(!statsPanel.activeSelf);

        isInSkillTree = !isInSkillTree;
        SkillTree.instance.GetDragClass().SetSkillTreeState(isInSkillTree);
    }
    void ReseetSkillTreePosition()
    {
        skillTreeTransform.localPosition = Vector3.zero;
    }
    public void SetSkillTreePosition(Vector3 newPos)
    {
        skillTreeTransform.position += newPos;
    }
    public void DisableSelectedSkillPanel()
    {
        skillPanel.SetActive(false);
    }
    public void EnableSelectedSkillPanel()
    {
        if (selectedSkill != null) return;

        skillPanel.SetActive(true);
    }
    public void SelectSkill(Skill skill)
    {
        EnableSelectedSkillPanel();

        selectedSkill = skill;

        skillName.text = skill.GetData()._name;
        skillIcon.sprite = skill.GetData().icon;
        UpdatePointsText();
        skillDescription.text = skill.GetData().description;
        FillAttributeRequirementsText(skill.GetData());

        getSkillButton.interactable = skill.CanUnlockSkill();
    }
    public void DisableGetSkillButton()
    {
        getSkillButton.interactable = false;
    }
    void FillAttributeRequirementsText(SkillData skillData)
    {
        skillAttributeRequirements.text = "";

        foreach (SkillAttributeRequirement req in skillData.requirements)
        {
            switch (req.attribute)
            {
                case SkillAttributeRequirement.Attribute.Strength:
                    skillAttributeRequirements.text += $"Strength: {PlayerStats.instance?.GetStrength()}/{req.amount}\n";
                    break;

                case SkillAttributeRequirement.Attribute.Agility:
                    skillAttributeRequirements.text += $"Agility: {PlayerStats.instance?.GetAgility()}/{req.amount}\n";
                    break;

                case SkillAttributeRequirement.Attribute.Intelligence:
                    skillAttributeRequirements.text += $"Intelligence: {PlayerStats.instance?.GetIntelligence()}/{req.amount}\n";
                    break;

                case SkillAttributeRequirement.Attribute.Endurance:
                    skillAttributeRequirements.text += $"Endurance: {PlayerStats.instance?.GetEndurance()}/{req.amount}\n";
                    break;
            }
        }
    }
    public void UpdatePointsText()
    {
        skillPointsRequirement.text = $"Points required:\n{PlayerStats.instance?.GetAvailablePoints()}/{selectedSkill.GetData().skillPointsRequired}";
    }
    public void CallGetSkill()
    {
        selectedSkill.AcquireSkill();
    }
    public void CallIncreaseStrength()
    {
        PlayerStats.instance.IncreaseStrength();
    }
    public void CallIncreaseAgility()
    {
        PlayerStats.instance.IncreaseAgility();
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
