using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    public static CheatMenu instance;

    [SerializeField] KeyCode shortcut = KeyCode.F4;

    [SerializeField] Transform fastTravelForest;
    [SerializeField] Transform fastTravelLobby;
    [SerializeField] Transform fastTravelCity;
    [SerializeField] Transform fastTravelMountain;
    [SerializeField] Transform fastTravelVillage;
    [SerializeField] Transform fastTravelBeach;

    bool hasInfiniteHP = false;
    bool hasInfiniteStamina = false;
    bool hasInfiniteAmmo = false;

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (SceneController.instance.GetIsInMainMenu()) return;

        if (Input.GetKeyDown(shortcut) && !UIManager.instance.GetIsInMenus())
        {
            UIManager.instance.ToggleCheatMenu();
        }
    }
    public void FastTravel(string location)
    {
        switch (location)
        {
            case "Forest":
                PlayerMovement.instance.TeleportPlayer(fastTravelForest.position);
                break;
            case "Lobby":
                PlayerMovement.instance.TeleportPlayer(fastTravelLobby.position);
                break;
            case "City":
                PlayerMovement.instance.TeleportPlayer(fastTravelCity.position);
                break;
            case "Mountain":
                PlayerMovement.instance.TeleportPlayer(fastTravelMountain.position);
                break;
            case "Village":
                PlayerMovement.instance.TeleportPlayer(fastTravelVillage.position);
                break;
            case "Beach":
                PlayerMovement.instance.TeleportPlayer(fastTravelBeach.position);
                break;
            default:
                Debug.LogError("Invalid Location", this);
                break;
        }
    }
    public void GainInfluencePoints(string influentialSide)
    {
        switch (influentialSide)
        {
            case "Nature":
                LoyaltySystem.instance.AddPointsInfluenceNature(100);
                break;
            case "City":
                LoyaltySystem.instance.AddPointsInfluenceCity(100);
                break;
            default:
                Debug.LogError("Invalid Influential Side", this);
                break;
        }
    }
    public void ToggleInfiniteHP()
    {
        hasInfiniteHP = !hasInfiniteHP;
    }
    public void ToggleInfiniteStamina()
    {
        hasInfiniteStamina = !hasInfiniteStamina;
    }
    public void ToggleInfiniteAmmo()
    {
        hasInfiniteAmmo = !hasInfiniteAmmo;
    }
    public void LevelUp()
    {
        PlayerStats.instance.LevelUp();
    }
    public bool GetHasInfiniteHP()
    {
        return hasInfiniteHP;
    }
    public bool GetHasInfiniteStamina()
    {
        return hasInfiniteStamina;
    }
    public bool GetHasInfiniteAmmo()
    {
        return hasInfiniteAmmo;
    }
}
