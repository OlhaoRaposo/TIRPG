using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Attribute Data", menuName = "Character Attribute")]
public class CharacterAttributeData : ScriptableObject
{
    public int strength;
    public int dexterity;
    public int endurance;
    public int intelligence;
}
