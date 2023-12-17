using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] CharacterAttributeData startAttributeData;

    [Header("Levelup formula variables (levelUpXp = ax� + bx + c)")]
    [SerializeField] float a;
    [SerializeField] float b;
    [SerializeField] float c;

    int level = 0;

    int currentXp = 0;
    int levelupXp = 0;

    float xpMultiplier = 1f;

   public int strength;
   public int dexterity;
   public int endurance;
   public int intelligence;

    float meleeDamageMultiplier = 1f;
    float meleeAttackSpeedMultiplier = 1f;

    int pointsAddedWhenLevelUp = 5;
    int availablePoints = 0;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SetStartAttributes();
        LevelUp();
    }
    void SetStartAttributes()
    {
        CharacterAttributeData startData = startAttributeData;
        strength = startData.strength;
        dexterity = startData.dexterity;
        endurance = startData.endurance;
        intelligence = startData.intelligence;
    }
    public void GainXp(int xp)
    {
        currentXp += (int)(xp * xpMultiplier);

        if (currentXp >= levelupXp)
        {
            LevelUp();
        }
        else
        {
            UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
        }
    }
    [ContextMenu("Level up")]
    public void LevelUp()
    {
        currentXp = 0;
        level++;
        pointsAddedWhenLevelUp = Mathf.FloorToInt(intelligence / 2f);
        availablePoints += pointsAddedWhenLevelUp;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateHUDLevel(level);
        SetLevelUpXp(level);
    }
    public void SetLevelUpXp(int nextLevel)
    {
        //Funcao que calcula xp necessario para upar de nivel
        levelupXp = (int)Mathf.Floor(a * (nextLevel ^ 3) + b * nextLevel + c);

        //Update UI
        UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
    }
    public void IncreaseStrength()
    {
        if (!DecreasePoints()) return;

        //Aumenta o dano causado
        IncreaseMeleeDamageMultipier();

        strength++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateStrength(strength);
    }
    public void IncreaseDexterity()
    {
        if (!DecreasePoints()) return;

        //Melhora a precisao dos tiros
        DecreaseBulletSpread();
        //Aumenta a velocidade de ataques melee
        IncreaseMeleeAttackSpeedMultiplier();

        dexterity++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateDexterity(dexterity);
    }
    public void IncreaseEndurance()
    {
        if (!DecreasePoints()) return;

        //Aumentar hp maximo
        //Aumentar stamina

        endurance++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateEndurance(endurance);
    }
    public void IncreaseIntelligence()
    {
        if (!DecreasePoints()) return;

        //Aumenta o multiplicador de xp
        IncreaseXpMultiplier();

        intelligence++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateIntelligence(intelligence);
    }
    bool DecreasePoints()
    {
        if (availablePoints <= 0) return false;

        availablePoints--;
        return true;
    }
    void IncreaseXpMultiplier()
    {
        xpMultiplier += .1f;
    }
    void IncreaseMeleeDamageMultipier()
    {
        meleeDamageMultiplier += .1f;
    }
    void IncreaseMeleeAttackSpeedMultiplier()
    {
        meleeAttackSpeedMultiplier += .1f;
    }
    void DecreaseBulletSpread()
    {
        //Diminuir distancia de espalhamento
        //Diminuir chance de espalhamento
    }
    public float GetMeleeDamageMultiplier()
    {
        return meleeDamageMultiplier;
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
    public int GetIntelligence()
    {
        return intelligence;
    }
}
