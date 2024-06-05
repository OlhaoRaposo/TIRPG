using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] CharacterAttributeData startAttributeData;

    [SerializeField] int maxLevel = 10;
    int level = 0;

    [SerializeField] LevelUpData levelupData;

    [SerializeField] int maxXpRequirement = 10000;
    int actualMaxXpRequirement;

    bool canGetXp = true;
    int currentXp = 0;
    int levelupXp = 0;

    float xpMultiplier = 1f;

    public int strength;
    public int agility;
    public int endurance;
    public int intelligence;

    float meleeDamageMultiplier = 1f;
    float meleeAttackSpeedMultiplier = 1f;

    int pointsAddedWhenLevelUp = 5;
    int availablePoints = 0;

    [SerializeField] List<SkillData> skills = new List<SkillData>();
    /*List<Skill> rangedSkills = new List<Skill>();
    List<Skill> meleeSkills = new List<Skill>();
    List<Skill> passiveSkills = new List<Skill>();
    List<Skill> grenadeSkills = new List<Skill>();
    List<Skill> otherSkills = new List<Skill>();*/

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        actualMaxXpRequirement = maxXpRequirement / 2;
        SetStartAttributes();
        LevelUp();
    }
    void SetStartAttributes()
    {
        CharacterAttributeData startData = startAttributeData;
        strength = startData.strength;
        agility = startData.agility;
        endurance = startData.endurance;
        intelligence = startData.intelligence;
    }


    public void GainXp(int xp)
    {
        if (!canGetXp) return;

        currentXp += (int)(xp * xpMultiplier);
        if (currentXp >= levelupXp)
        {
            int remainingXp = currentXp - levelupXp;

            if (level < maxLevel)
            {
                LevelUp();
            }
            else
            {
                currentXp = levelupXp;
                UIManager.instance.UpdateXpStats(currentXp, levelupXp);
                canGetXp = false;
            }

            if (remainingXp > 0) GainXp(remainingXp);
        }
        else
        {
            UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
        }
    }
    [ContextMenu("(Testing only) Level up")]
    public void LevelUp()
    {
        currentXp = 0;
        level++;
        pointsAddedWhenLevelUp = Mathf.FloorToInt(intelligence / 2f);
        availablePoints += pointsAddedWhenLevelUp;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateHUDLevel(level);
        
        levelupXp = GetLevelUpXp();

        //Debug.Log(levelupXp);

        UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
    }
    [ContextMenu("Set levelup values")]
    public void SetLevelUpValues()
    {
        levelupData.Clear();
        for (int i = 0; i < maxLevel; i++)
        {
            LevelUp();
            levelupData.AddLevelUpXp(levelupXp);
        }
    }
    [ContextMenu("(Testing only) Get more points")]
    public void GainPoints()
    {
        availablePoints += 10;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
    }


    //Funcao que calcula xp necessario para upar de nivel
    public void SetLevelUpXp()
    {
        float cosAngle = level * (/*180*/Mathf.PI / maxLevel);

        float cosMultiplier = 1f;
        /*if (cosAngle > (Mathf.Deg2Rad * 45) && cosAngle < (Mathf.Deg2Rad * 135))
        {
            Debug.Log(level);
            *//*Debug.Log(cosAngle);
            Debug.Log(cosAngle - Mathf.Deg2Rad * 45);
            Debug.Log(Mathf.Deg2Rad * 90);
            Debug.Log((cosAngle - Mathf.Deg2Rad * 45) / (Mathf.Deg2Rad * 90));*//*

            //cosMultiplier = 1 - Mathf.Lerp(-0.25f, 0.25f, (cosAngle - Mathf.Deg2Rad * 45) / (Mathf.Deg2Rad * 90));
            //cosMultiplier = 1 - Mathf.PingPong((cosAngle - Mathf.Deg2Rad * 45) / (Mathf.Deg2Rad * 90), .25f);
        }*/

        cosMultiplier = 1 - Mathf.PingPong(cosAngle / Mathf.PI, .25f);
        //Debug.Log(cosMultiplier);

        levelupXp = (int)(((-Mathf.Cos(cosAngle) * actualMaxXpRequirement) + actualMaxXpRequirement) * cosMultiplier);

        //Update UI
        UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
    }
    public int GetLevelUpXp()
    {
        if (levelupData.xpPerLevel.Count == 0) SetLevelUpValues();

        return levelupData.xpPerLevel[level - 1];
    }

    public void AddSkill(SkillData skillData)
    {
        skills.Add(skillData);
        DecreasePoints(skillData.skillPointsRequired);
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
    public void IncreaseAgility() //NECESSARIO ATUALIZAR
    {
        if (!DecreasePoints()) return;

        //Melhora a precisao dos tiros
        DecreaseBulletSpread();
        //Aumenta a velocidade de ataques melee
        IncreaseMeleeAttackSpeedMultiplier();

        agility++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateAgility(agility);
    }
    public void IncreaseEndurance()
    {
        if (!DecreasePoints()) return;

        //Aumentar hp maximo
        PlayerHPController.instance.IncreaseMaxHP(20f);
        //Aumentar stamina
        PlayerHPController.instance.IncreaseStamina(20f);

        UIManager.instance.UpdateHealthStats((int)PlayerHPController.instance.GetHp(), (int)PlayerHPController.instance.GetMaxHp());
        UIManager.instance.UpdateStaminaStats((int)PlayerHPController.instance.GetStamina(), (int)PlayerHPController.instance.GetMaxStamina());

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
    bool DecreasePoints(uint points)
    {
        if (availablePoints < points) return false;

        availablePoints -= (int)points;
        return true;
    }
    public int GetAvailablePoints()
    {
        return availablePoints;
    }

    void IncreaseXpMultiplier()
    {
        xpMultiplier += .1f;
    }
    void IncreaseMeleeDamageMultipier()
    {
        meleeDamageMultiplier += .1f;
    }
    void IncreaseMeleeAttackSpeedMultiplier() //NECESSARIO ATUALIZAR
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
    public int GetAgility()
    {
        return agility;
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
