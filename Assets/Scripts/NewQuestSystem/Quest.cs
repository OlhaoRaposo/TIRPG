using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestSystem", menuName = "QuestSystem/New Quest", order = 1)]
public class Quest : ScriptableObject
{
    public string questCode;
    public bool hasAReward;
    public Rewards Reward;
    public List<Validation> validations = new List<Validation>();

}
[Serializable]
public class Rewards {
    public enum LoyaltyType{None,City, Nature}
    [Header("Loyalty Type")]
    public LoyaltyType loyaltyType;
    public float influence;
    [Header("XP")]
    public float xp;
}
[Serializable]
public class QuestDialogue {
    public List<String> dialogues = new List<String>();
}
