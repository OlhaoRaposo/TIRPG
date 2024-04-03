using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
  [Header("Info")]
  public string code;
  public string title;
  public string description;
  public bool isComplete = false;
  public int currentStep = 0;
  public bool hasQuestReward;
  public string questRewardCode;
  [Header("Accessibility")] 
  public Reward questReward;
  public List<Dialogue> dialogue;
  public List<Dialogue> questAlreadyGiven;
  public List<Step> steps = new List<Step>();
  public string Name {set => code = value; get => code;}
}

[System.Serializable]
public class Reward
{
  public enum RoyaltyType {
    city,nature
  }
  public List<Dialogue> rewardDialogue;
  public RoyaltyType royaltyType;
  public float royaltyValue;
}
