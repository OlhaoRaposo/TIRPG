using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Quest 
{
    public string questName;
    public string questDescription;
    public Validation atualPhase;
    public List<Validation> phases = new List<Validation>();
    public Reward rewards;
    public string npcToComplete;
}
[Serializable]
public class Validation {
    public string name;
    public bool isComplete;
}
[Serializable]
public struct Reward {
    public enum  LoyaltyType { City, Nature }
    [Header("Loyalty")]
    public LoyaltyType loyaltyType;
    public float loyalty;
    [Header("Items")]
    public ItemData[] itens;
    [Header("Inventory")]
    public int xp;
    public float gold;
    public string questReward;
}
