using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] CharacterAttributeData startAttributeData;

    [Header("Levelup formula variables (levelUpXp = ax² + bx + c)")]
    [SerializeField] float a, b, c;

    int level = 0;

    int currentXp = 0;
    int levelupXp = 0;

    int strength;
    int dexterity;
    int endurance;

    [SerializeField] int pointsAddedWhenLevelUp = 5;
    int availablePoints = 0;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LevelUp();
        SetStartAttributes();
    }
    void SetStartAttributes()
    {
        CharacterAttributeData startData = startAttributeData;
        strength = startData.strength;
        dexterity = startData.dexterity;
        endurance = startData.endurance;
    }
    public void GainXp(int xp)
    {
        currentXp += xp;

        if (currentXp >= levelupXp)
        {
            LevelUp();
        }
        else
        {
            UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
        }
    }
    public void LevelUp()
    {
        currentXp = 0;
        level++;
        availablePoints += pointsAddedWhenLevelUp;
        SetLevelUpXp(level);
    }
    public void SetLevelUpXp(int nextLevel)
    {
        //Funcao que calcula xp necessario para upar de nivel
        levelupXp = (int)Mathf.Floor(a * (nextLevel ^ 3) + b * nextLevel + c);

        //Update UI
        UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
    }
    public void AddStrength()
    {
        if (availablePoints <= 0) return;

        availablePoints--;
        strength++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
    }
    public void AddDexterity()
    {
        if (availablePoints <= 0) return;

        availablePoints--;
        dexterity++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
    }
    public void AddEndurance()
    {
        if (availablePoints <= 0) return;

        availablePoints--;
        endurance++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
    }
    public int GetStrength()
    {
        return strength;
    }
    public int GetDexterity()
    {
        return dexterity;
    }
    public int GetEndurance()
    {
        return endurance;
    }
}
