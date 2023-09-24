using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Quests", menuName = "Quests/Create New Quest")]
public class QuestType : ScriptableObject
{
    [Header ("Quest Name")]
    public string questName;
    
    [Header ("Mission Settings")]
    public bool inProgress;

    public enum MissionRating{Primary, Secondary, Temporary}
    
    public MissionRating classification;
    
    public enum MissionType{Exploration, ItenCollection, KillEnemies, Sabotage, Dialogue}

    public List<MissionType> stageMission = new List<MissionType>();

    public List<string> questDescription = new List<string>();

    public MissionType type;

    public int questStages;

    public float questProgress;
    
    public bool hasTimeout;

    public float questTime;

    public float questTimeLimit;
    
    public QuestType nextMission;
    
    public bool concluded;

    [Header ("Mission Rewards")]
    public float influencePoints;

    public enum SideInfluence{None, Nature, City}

    public SideInfluence team;
  
    public enum RewardType{Guns, SkillPoints, Accessories, Consumables, Throwables}

    public List<RewardType> questReward = new List<RewardType>();
    
    public List<int> rewardQuantity = new List<int>();

    public List<GameObject> rewardObjects = new List<GameObject>();
    
    public List<bool> unlockItems = new List<bool>();

    [Header ("HUD Changes")]
    public List<DialogueTemp> dialogue = new List<DialogueTemp>();
    
    public List<Image> images = new List<Image>();
    
    public List<Sprite> sprites = new List<Sprite>();
    
    public List<GameObject> characters = new List<GameObject>();
    
    public List<Animator> animations = new List<Animator>();
    
    [Header ("Specific Objectives")]
    public List<GameObject> explorationZones = new List<GameObject>();
    
    public List<GameObject> sabotageZones = new List<GameObject>();
    
    public List<GameObject> missionObjectives = new List<GameObject>();
    
    public enum TypesOfCollectibles{Fruits, MechanicalParts}
    
    public List<TypesOfCollectibles> missionCollectibles = new List<TypesOfCollectibles>();
    
    public List<int> collectedItems = new List<int>();
    
    public List<int> collectedItemsGoal = new List<int>();

    public enum EnemyTypes{MilitaryRobots, LegendaryAnimals}
    
    public List<EnemyTypes> enemies = new List<EnemyTypes>();
    
    public List<int> enemiesKilled = new List<int>();

    public List<int> deadEnemiesObjective = new List<int>();
    
    public GameObject boss;
}