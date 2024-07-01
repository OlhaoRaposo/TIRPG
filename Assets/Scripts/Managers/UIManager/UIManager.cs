using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Panels")]
    [SerializeField] GameObject inGameMenusParent;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject statsPanel;
    [SerializeField] GameObject questsPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject shopPanel;

    [Header("Skill Tree")]
    [SerializeField] GameObject skillTreePanel;
    RectTransform skillTreeTransform;
    [SerializeField] GameObject skillPanel;
    [SerializeField] Image skillIcon;
    [SerializeField] Text skillName;
    [SerializeField] Text skillDescription;
    [SerializeField] Text skillAttributeRequirements;
    [SerializeField] Text skillPointsRequirement;
    [SerializeField] Button getSkillButton;
    Skill selectedSkill;

    [Header("Fast Travel & Cheats")]
    [SerializeField] GameObject fastTravelMenu;
    [SerializeField] GameObject cheatMenuPanel;

    [Header("Shop")]
    [SerializeField] Text merchantInventoryLabel;
    [SerializeField] Text shopInfluenceInfo;
    [SerializeField] ShopSlot[] shopSlots_buy;
    [SerializeField] ShopSlot[] shopSlots_sell;
    MerchantInventory currentMerchant;

    [Header("Hud")]
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject staminaBar;
    [SerializeField] Text lvlText_hud;
    [SerializeField] GameObject equipedWeapons;
    [SerializeField] GameObject crosshair;
    [SerializeField] Text feedbackText;
    [SerializeField] Text ammoText;

    [Header("Player Attributes")]
    [SerializeField] Text strengthText;
    [SerializeField] Text agilityText;
    [SerializeField] Text enduranceText;
    [SerializeField] Text intelligenceText;
    [SerializeField] Text attributeDescription;

    [Header("Attribute description")]
    [SerializeField] [TextArea(1, 4)] string strengthDescription;
    [SerializeField] [TextArea(1, 4)] string enduranceDescription;
    [SerializeField] [TextArea(1, 4)] string agilityDescription;
    [SerializeField] [TextArea(1, 4)] string intelligenceDescription;

    [Header("Player Status")]
    [SerializeField] Text healthPointsText;
    [SerializeField] Text staminaPointsText;
    [SerializeField] Text xpMultiplierText;
    [SerializeField] Text meleeMultiplierText;
    [SerializeField] Text rangedMultiplierText;
    [SerializeField] Text movementSpeedMultiplierText;
    [SerializeField] Text staminaRegenMultiplierText;
    [SerializeField] Text fireMultiplierText;
    [SerializeField] Text lightningMultiplierText;
    [SerializeField] Text physicalMultiplierText;
    [SerializeField] Text acidMultiplierText;
    [SerializeField] Text xpText;
    [SerializeField] Text lvlText_menu;
    [SerializeField] Text availablePointsText;

    [Header("Inventory")]
    [SerializeField] Text meleeWeaponName;
    [SerializeField] Text rangedWeaponName;
    [SerializeField] Text consumableName;
    [SerializeField] Text throwableName;

    [SerializeField] Text meleeWeaponDesc;
    [SerializeField] Text rangedWeaponDesc;

    string selectedAttribute = "";

    bool isInSkillTree = false;
    bool isInMenus = false;
    bool cursorLockState = false;
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
        if (Input.GetKey(KeyCode.Escape))
        {
            ToggleShopPanel();
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
    public void UpdateMeleeName(string n)
    {
        meleeWeaponName.text = n;
    }
    public void UpdateRangedName(string n)
    {
        rangedWeaponName.text = n;
    }
    public void UpdateConsumableName(string n)
    {
        consumableName.text = n;
    }
    public void UpdateThrowableName(string n)
    {
        throwableName.text = n;
    }
    public void UpdateMeleeName()
    {
        meleeWeaponName.text = "Corpo a corpo";
    }
    public void UpdateRangedName()
    {
        rangedWeaponName.text = "Arma à distância";
    }
    public void UpdateConsumableName()
    {
        consumableName.text = "Consumível";
    }
    public void UpdateThrowableName()
    {
        throwableName.text = "Arremessável";
    }
    public void UpdateMeleeWeaponDescription(string damage, DamageElementManager.DamageElement type)
    {
        string typeString = "";
        switch (type)
        {
            case DamageElementManager.DamageElement.Acid: 
                typeString = "Ácido";
                break;
            case DamageElementManager.DamageElement.Fire:
                typeString = "Fogo";
                break;
            case DamageElementManager.DamageElement.Lightning:
                typeString = "Elétrico";
                break;
            case DamageElementManager.DamageElement.Physical:
                typeString = "Físico";
                break;
        }

        meleeWeaponDesc.text = $"Dano: {damage}\nTipo de dano: {typeString}";
    }
    public void UpdateRangedWeaponDescription(string damage, DamageElementManager.DamageElement type)
    {
        string typeString = "";
        switch (type)
        {
            case DamageElementManager.DamageElement.Acid:
                typeString = "Ácido";
                break;
            case DamageElementManager.DamageElement.Fire:
                typeString = "Fogo";
                break;
            case DamageElementManager.DamageElement.Lightning:
                typeString = "Elétrico";
                break;
            case DamageElementManager.DamageElement.Physical:
                typeString = "Físico";
                break;
        }

        rangedWeaponDesc.text = $"Dano: {damage}\nTipo de dano: {typeString}";
    }
    public void UpdateMenuLevel(int lvl)
    {
        lvlText_menu.text = "Level " + lvl.ToString();
    }
    public void UpdateXpStats(int xp, int maxXp)
    {
        xpText.text = $"Xp: {xp}/{maxXp}";
    }
    public void UpdateHealthStats(int hp, int maHXp)
    {
        healthPointsText.text = $"Vida {hp}/{maHXp}";
    }
    public void UpdateStaminaStats(int stamina, int maxStamina)
    {
        staminaPointsText.text = $"Estamina {stamina}/{maxStamina}";
    }
    public void UpdateXpMultiplier(string mult)
    {
        xpMultiplierText.text = "Multiplicador xp: " + mult + "x";
    }
    public void UpdateMeleeMultiplier(string mult)
    {
        meleeMultiplierText.text = "Multiplicador corpo-a-corpo: " + mult + "x";
    }
    public void UpdateRangedMultiplier(string mult)
    {
        rangedMultiplierText.text = "Multiplicador a distância: " + mult + "x";
    }
    public void UpdateMovementSpeedMultiplier(string mult)
    {
        movementSpeedMultiplierText.text = "Multiplicador de movimento: " + mult + "x";
    }
    public void UpdateStaminaRegenMultiplier(string mult)
    {
        staminaRegenMultiplierText.text = "Multiplicador regeneração stamina: " + mult + "x";
    }
    public void UpdateFireMultiplier(string mult)
    {
        fireMultiplierText.text = "Multiplicador fogo: " + mult + "x";
    }
    public void UpdateLightningMultiplier(string mult)
    {
        lightningMultiplierText.text = "Multiplicador elétrico: " + mult + "x";
    }
    public void UpdatePhysicalMultiplier(string mult)
    {
        physicalMultiplierText.text = "Multiplicador físico: " + mult + "x";
    }
    public void UpdateAcidMultiplier(string mult)
    {
        acidMultiplierText.text = "Multiplicador ácido: " + mult + "x";
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

        CancelInvoke("HideTextFeedback");
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

        //PlayerController.instance?.ToggleWeaponSwap(!isInMenus);
        PlayerMovement.instance.WeaponSwap();
        PlayerGun.instance?.ShootToggle(!isInMenus);
        PlayerMeleeCombat.instance?.MeleeAttackToggle(!isInMenus);

        if (currentMerchant == null)
        {
            inGameMenusParent.SetActive(!inGameMenusParent.activeSelf);

            DisableAllPanels();
            DisablePanel(cheatMenuPanel);

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
    public void ToggleFastTravelMenu()
    {
        fastTravelMenu.SetActive(!fastTravelMenu.activeSelf);
        ToggleCursorLockMode();
    }
    public void ToggleCheatMenu()
    {
        cheatMenuPanel.SetActive(!cheatMenuPanel.activeSelf);
        ToggleCursorLockMode();
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
        if (currentMerchant == null) return;

        isInMenus = !isInMenus;

        DisableAllPanels();
        ToggleCursorLockMode();
        ToggleCrosshair();
        currentMerchant = null;
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
    void UpdateMerchantNameText()
    {
        merchantInventoryLabel.text = "Inventário do mercador";
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
    public void SetCursorLockState(bool state)
    {
        cursorLockState = state;
        //Debug.Log($"Is the cursor locked? {cursorLockState}");
    }
    void ToggleCursorLockMode()
    {
        cursorLockState = !cursorLockState;
        PlayerCameraMovement.instance.ToggleAimLock(cursorLockState);
    }
    void DisablePanel(GameObject go)
    {
        go.SetActive(false);
    }
    void DisableAllPanels()
    {
        inventoryPanel?.SetActive(false);
        statsPanel?.SetActive(false);
        questsPanel?.SetActive(false);
        optionsPanel?.SetActive(false);
        shopPanel?.SetActive(false);
        DisableSkillTree();

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
    void DisableSkillTree()
    {
        if (selectedSkill != null)
        {
            selectedSkill = null;
            DisableSelectedSkillPanel();
        }

        skillTreePanel.SetActive(false);
        ReseetSkillTreePosition();

        isInSkillTree = false;
        SkillTree.instance.GetDragClass().SetSkillTreeState(isInSkillTree);
    }
    void ReseetSkillTreePosition()
    {
        skillTreeTransform.localPosition = Vector3.zero;
    }
    public void SetSkillTreePosition(Vector3 newPos, Vector2 limits)
    {
        Vector3 posToAdd = newPos + skillTreeTransform.localPosition;
        posToAdd = new Vector3(Mathf.Clamp(posToAdd.x, -limits.x, limits.x), Mathf.Clamp(posToAdd.y, -limits.y, limits.y), 0);
        skillTreeTransform.localPosition = posToAdd;

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
                    skillAttributeRequirements.text += $"Força: {PlayerStats.instance?.GetStrength()}/{req.amount}\n";
                    break;

                case SkillAttributeRequirement.Attribute.Agility:
                    skillAttributeRequirements.text += $"Agilidade: {PlayerStats.instance?.GetAgility()}/{req.amount}\n";
                    break;

                case SkillAttributeRequirement.Attribute.Intelligence:
                    skillAttributeRequirements.text += $"Inteligencia: {PlayerStats.instance?.GetIntelligence()}/{req.amount}\n";
                    break;

                case SkillAttributeRequirement.Attribute.Endurance:
                    skillAttributeRequirements.text += $"Resistencia: {PlayerStats.instance?.GetEndurance()}/{req.amount}\n";
                    break;
            }
        }
    }
    public void UpdatePointsText()
    {
        skillPointsRequirement.text = $"Pontos necessários:\n{PlayerStats.instance?.GetAvailablePoints()}/{selectedSkill.GetData().skillPointsRequired}";
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
        //Disable in game canvas
        GameObject.Find("====CANVAS====").SetActive(false);

        SceneController.instance.LoadMenu();
    }
    public void CallQuitGame()
    {
        SceneController.instance.QuitGame();
    }
}
