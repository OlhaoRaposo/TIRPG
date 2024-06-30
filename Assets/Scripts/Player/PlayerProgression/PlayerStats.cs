using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] CharacterAttributeData startAttributeData;

    [SerializeField] int maxLevel = 18;
    int level = 0;

    [SerializeField] LevelUpData levelupData;

    [SerializeField] int maxXpRequirement = 10000;
    int actualMaxXpRequirement;

    bool canGetXp = true;
    int currentXp = 0;
    int levelupXp = 0;

    float xpMultiplier = 1f;

    int strength;
    int agility;
    int endurance;
    int intelligence;

    float meleeDamageMultiplier = 1f;
    float rangedDamageMultiplier = 1f;

    float acidDamageMultiplier = 1f;
    float fireDamageMultiplier = 1f;
    float lightningDamageMultiplier = 1f;
    float physicalDamageMultiplier = 1f;

    float moveSpeedMultiplier = 1f;
    float staminaRegenMultiplier = 1f;

    int pointsAddedWhenLevelUp = 5;
    public int availablePoints = 0;

    [SerializeField] List<SkillData> skills = new List<SkillData>();

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
                UIManager.instance.ShowTextFeedback("You leveled up!");
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

    public void AddSkill(SkillData skillData)
    {
        if (skills.Contains(skillData)) return;

        skills.Add(skillData);
        if (DecreasePoints(skillData.skillPointsRequired))
        {
            foreach(SkillEffect effect in skillData.skillEffects)
            {
                switch (effect.effects)
                {
                    case SkillEffect.SkillEffects.Xp:
                        IncreaseXpMultiplier(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.MeleeDamage:
                        IncreaseMeleeDamageMultipier(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.RangedDamage:
                        IncreaseRangedDamageMultipier(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.MoveSpeed:
                        IncreaseMoveSpeedMultiplier(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.Stamina:
                        IncreaseStamina(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.StaminaRegen:
                        IncreaseStaminaRegen(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.HealthPoints:
                        IncreaseHealthPoints(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.AcidDamage:
                        IncreaseAcidDamageMultiplier(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.LightningDamage:
                        IncreaseLightningDamageMultiplier(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.FireDamage:
                        IncreaseFireDamageMultiplier(effect.amount);
                        break;
                    case SkillEffect.SkillEffects.PhysicalDamage:
                        IncreasePhysicalDamageMultiplier(effect.amount);
                        break;
                    default:
                        Debug.LogError("SkillEffect was not defined");
                        break;

                }
            }
        }
        else
        {
            Debug.Log("Player não possui pontos de habilidade suficientes");
        }
    }

    #region Attribute Buffs
    public void BuffStrength(float multiplier, float duration)
    {
        float startStrengthMultiplier = meleeDamageMultiplier;

        meleeDamageMultiplier *= multiplier;

        StartCoroutine(ResetStrength(startStrengthMultiplier, duration));
    }
    IEnumerator ResetStrength(float startMultiplier, float duration)
    {
        yield return new WaitForSeconds(duration);

        meleeDamageMultiplier = startMultiplier;
    }
    public void BuffAgility(float multiplier, float duration)
    {
        float startmoveSpeedMultiplier = moveSpeedMultiplier;

        moveSpeedMultiplier *= multiplier;

        StartCoroutine(ResetAgility(startmoveSpeedMultiplier, duration));
    }
    IEnumerator ResetAgility(float startMultiplier, float duration)
    {
        yield return new WaitForSeconds(duration);

        moveSpeedMultiplier = startMultiplier;
    }
    public void BuffEndurance(float multiplier, float duration)
    {
        PlayerHPController.instance.BuffHp(multiplier, duration);
        PlayerHPController.instance.BuffStamina(multiplier, duration);
    }
    public void BuffIntelligence(float multiplier, float duration)
    {
        float startXpMultiplier = xpMultiplier;

        xpMultiplier *= multiplier;

        StartCoroutine(ResetIntelligence(startXpMultiplier, duration));
    }
    IEnumerator ResetIntelligence(float startMultiplier, float duration)
    {
        yield return new WaitForSeconds(duration);

        xpMultiplier = startMultiplier;
    }
    public void BuffStaminaRegen(float multiplier, float duration)
    {
        float startStaminaRegenMultiplier = staminaRegenMultiplier;

        staminaRegenMultiplier *= multiplier;

        StartCoroutine(ResetStaminaRegen(startStaminaRegenMultiplier, duration));
    }
    IEnumerator ResetStaminaRegen(float startMultiplier, float duration)
    {
        yield return new WaitForSeconds(duration);

        staminaRegenMultiplier = startMultiplier;
    }
    #endregion

    public void IncreaseStrength()
    {
        if (!DecreasePoints()) return;

        //Aumenta o dano causado
        IncreaseMeleeDamageMultipier(.1f);

        strength++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateStrength(strength);
    }
    public void IncreaseAgility()
    {
        if (!DecreasePoints()) return;

        //Aumenta a velocidade do player
        IncreaseMoveSpeedMultiplier(.1f);

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
        IncreaseXpMultiplier(0.1f);

        intelligence++;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        UIManager.instance.UpdateIntelligence(intelligence);
    }
    bool DecreasePoints()
    {
        if (availablePoints <= 0) return false;

        availablePoints--;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        return true;
    }
    bool DecreasePoints(uint points)
    {
        if (availablePoints < points) return false;

        availablePoints -= (int)points;
        UIManager.instance.UpdateAvailablePoints(availablePoints);
        return true;
    }

    #region Attribute Multipliers
    public void IncreaseXpMultiplier(float amount)
    {
        xpMultiplier += amount;
        UIManager.instance.UpdateXpMultiplier(xpMultiplier.ToString());
    }
    public void IncreaseMoveSpeedMultiplier(float amount)
    {
        moveSpeedMultiplier += amount;
        UIManager.instance.UpdateMovementSpeedMultiplier(moveSpeedMultiplier.ToString());
    }
    public void IncreaseMeleeDamageMultipier(float amount)
    {
        meleeDamageMultiplier += amount;
        UIManager.instance.UpdateMeleeMultiplier(meleeDamageMultiplier.ToString());
    }
    public void IncreaseRangedDamageMultipier(float amount)
    {
        rangedDamageMultiplier += amount;
        UIManager.instance.UpdateRangedMultiplier(rangedDamageMultiplier.ToString());
    }
    public void IncreaseAcidDamageMultiplier(float amount)
    {
        acidDamageMultiplier += amount;
        UIManager.instance.UpdateAcidMultiplier(acidDamageMultiplier.ToString());
    }
    public void IncreaseFireDamageMultiplier(float amount)
    {
        fireDamageMultiplier += amount;
        UIManager.instance.UpdateFireMultiplier(fireDamageMultiplier.ToString());
    }
    public void IncreaseLightningDamageMultiplier(float amount)
    {
        lightningDamageMultiplier += amount;
        UIManager.instance.UpdateLightningMultiplier(lightningDamageMultiplier.ToString());
    }
    public void IncreasePhysicalDamageMultiplier(float amount)
    {
        physicalDamageMultiplier += amount;
        UIManager.instance.UpdatePhysicalMultiplier(physicalDamageMultiplier.ToString());
    }
    public void IncreaseStaminaRegen(float amount)
    {
        staminaRegenMultiplier += amount;
        UIManager.instance.UpdateStaminaRegenMultiplier(staminaRegenMultiplier.ToString());
    }
    public void IncreaseHealthPoints(float amount)
    {
        PlayerHPController.instance.IncreaseMaxHP(amount);
        UIManager.instance.UpdateHealthStats((int)PlayerHPController.instance.GetHp(), (int)PlayerHPController.instance.GetMaxHp());
    }
    public void IncreaseStamina(float amount)
    {
        PlayerHPController.instance.IncreaseStamina(amount);
        UIManager.instance.UpdateStaminaStats((int)PlayerHPController.instance.GetStamina(), (int)PlayerHPController.instance.GetMaxStamina());
    }
    #endregion

    #region Set Attributes

    public void SetLevel(int lvl)
    {
        level = lvl;
        levelupXp = GetLevelUpXp();

        UIManager.instance.UpdateHUDLevel(level);
        UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
    }
    public void SetXp(int xp)
    {
        currentXp = xp;

        UIManager.instance?.UpdateXpStats(currentXp, levelupXp);
    }
    public void SetAvailablePoints(int pts)
    {
        availablePoints = pts;

        UIManager.instance.UpdateAvailablePoints(availablePoints);
    }
    public void SetStrength(int s)
    {
        strength = s;
        meleeDamageMultiplier = 1 + strength * .1f;

        UIManager.instance.UpdateStrength(strength);
        UIManager.instance.UpdateMeleeMultiplier(meleeDamageMultiplier.ToString());
    }
    public void SetAgility(int a)
    {
        agility = a;
        moveSpeedMultiplier = 1 + agility * .1f;

        UIManager.instance.UpdateAgility(agility);
        UIManager.instance.UpdateMovementSpeedMultiplier(moveSpeedMultiplier.ToString());
    }
    public void SetEndurance(int e)
    {
        endurance = e;
        PlayerHPController.instance.SetMaxHp(100f + (e * 20f));
        PlayerHPController.instance.SetMaxStamina(100f + (e * 20f));

        UIManager.instance.UpdateEndurance(endurance);

        UIManager.instance.UpdateHealthStats((int)PlayerHPController.instance.GetHp(), (int)PlayerHPController.instance.GetMaxHp());
        UIManager.instance.UpdateStaminaStats((int)PlayerHPController.instance.GetStamina(), (int)PlayerHPController.instance.GetMaxStamina());
    }
    public void SetIntelligence(int i)
    {
        intelligence = i;
        xpMultiplier = 1 + intelligence * 0.1f;

        UIManager.instance.UpdateIntelligence(intelligence);
        UIManager.instance.UpdateXpMultiplier(xpMultiplier.ToString());
    }

    #endregion

    #region Get Attributes
    public int GetAvailablePoints()
    {
        return availablePoints;
    }
    public float GetXpMultiplier()
    {
        return xpMultiplier;
    }
    public int GetLevelUpXp()
    {
        if (levelupData.xpPerLevel.Count == 0) SetLevelUpValues();

        return levelupData.xpPerLevel[level - 1];
    }
    public float GetMovementSpeedMultiplier()
    {
        return moveSpeedMultiplier;
    }
    public float GetMeleeDamageMultiplier()
    {
        return meleeDamageMultiplier;
    }
    public float GetRangedDamageMultiplier()
    {
        return rangedDamageMultiplier;
    }
    public float GetAcidDamageMultiplier()
    {
        return acidDamageMultiplier;
    }
    public float GetFireDamageMultiplier()
    {
        return fireDamageMultiplier;
    }
    public float GetLightningDamageMultiplier()
    {
        return lightningDamageMultiplier;
    }
    public float GetPhysicalDamageMultiplier()
    {
        return physicalDamageMultiplier;
    }
    public float GetStaminaRegenMultiplier()
    {
        return staminaRegenMultiplier;
    }
    public int GetLevel()
    {
        return level;
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
    #endregion
}
