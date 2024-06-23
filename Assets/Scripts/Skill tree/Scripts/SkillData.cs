using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillData", menuName = "SkillTree/SkillData")]
public class SkillData : ScriptableObject
{
    public string _name;
    [TextArea(1, 5)] public string description;

    //variavel que representa o que a skill afeta
    public List<SkillEffect> skillType;
    
    public Sprite icon;

    //pontos necessarios para adquirir a skill
    public uint skillPointsRequired;
    
    //requisitos de atributos para adquirir a skill
    public List<SkillAttributeRequirement> requirements;

    //lista que representa quais skills sao necessarias possuir previamente para adquiri-la
    public List<SkillData> skillsRequired;
}

[System.Serializable]
public struct SkillAttributeRequirement
{
    public enum Attribute {Strength, Agility, Intelligence, Endurance}
    public Attribute attribute;
    public uint amount;
}

//FAZER UM STRUCT COM O ATRIBUTO, QUANTO ELE AFETA, ETC
[System.Serializable]
public struct SkillEffect
{
    public enum SkillEffects { Xp, MeleeDamage, RangedDamage, Stamina, StaminaRegen, HealthPoints, AcidDamage, LightningDamage, FireDamage, PhysicalDamage }
    public SkillEffects effects;
    public float amount;
}

