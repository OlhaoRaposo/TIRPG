using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill: MonoBehaviour
{
    [SerializeField] SkillData skillData;

    //Elementos da UI
    [SerializeField] Image icon;

    bool isUnlocked = false;

    private void OnEnable()
    {
        SetupSkillVisuals();
    }
    public void SetupSkillVisuals()
    {
        //Atualizar icone da skill
        icon.sprite = skillData.icon;

        //Checar se as skills necessarias estao desbloqueadas, caso não estejam, deixar icone cinza
        if (CanUnlockSkill())
        {
            icon.color = Color.white;
        }
        else
        {
            if (!isUnlocked) icon.color = Color.grey;
        }
    }
    public void CallSelectSkill()
    {
        UIManager.instance?.SelectSkill(this);
    }
    public void ForceAcquireSkill()
    {
        //Adicionar skillData á lista de skills
        isUnlocked = true;
        PlayerStats.instance?.AddSkill(skillData);

        //Desabilita o botao de adquirir a skill
        UIManager.instance?.DisableGetSkillButton();

        GetComponent<Outline>().enabled = true;

        SkillTree.instance.ResetAllSkillVisuals();
    }
    public void AcquireSkill() 
    {
        if (!CanUnlockSkill()) return; //PODE SER RETIRADO POSTERIORMENTE (VERIFICACAO FEITA NO UIMANAGER P/ HABILITAR O BOTAO)

        //Adicionar skillData á lista de skills
        isUnlocked = true;
        PlayerStats.instance?.AddSkill(skillData);

        //Desabilita o botao de adquirir a skill
        UIManager.instance?.DisableGetSkillButton();

        GetComponent<Outline>().enabled = true;
        
        UIManager.instance.UpdatePointsText();

        SkillTree.instance.ResetAllSkillVisuals();
    }
    public bool CanUnlockSkill()
    {
        if (isUnlocked) return false;

        //Checa se o player possui a quantidade de pontos necessaria
        if (PlayerStats.instance?.GetAvailablePoints() < skillData.skillPointsRequired) return false;

        //Usa SkillTree.instance.GetSkillByData para checar se as skillData.skillsRequired estao desbloqueadas
        if (!CheckAttributeRequirements()) return false;

        //Checa se os skillData.requirements foram cumpridos
        if (!CheckSkillRequirements()) return false;

        return true;
    }
    bool CheckAttributeRequirements()
    {
        foreach(SkillAttributeRequirement req in skillData.requirements)
        {
            switch (req.attribute)
            {
                case SkillAttributeRequirement.Attribute.Strength: if (PlayerStats.instance?.GetStrength() < req.amount) return false; break;

                case SkillAttributeRequirement.Attribute.Agility: if (PlayerStats.instance?.GetAgility() < req.amount) return false; break;

                case SkillAttributeRequirement.Attribute.Intelligence: if (PlayerStats.instance?.GetIntelligence() < req.amount) return false; break;

                case SkillAttributeRequirement.Attribute.Endurance: if (PlayerStats.instance?.GetEndurance() < req.amount) return false; break;
            }
        }

        return true;
    }
    bool CheckSkillRequirements()
    {
        foreach(Skill s in SkillTree.instance?.GetPreviousSkillsByData(skillData))
        {
            if (!s.isUnlocked) return false;
        }

        return true;
    }
    public bool GetLockedState()
    {
        return isUnlocked;
    }
    public SkillData GetData()
    {
        return skillData;
    }
}
