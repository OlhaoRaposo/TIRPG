using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class QuestDatabase
{
    public List<Quest> allQuestsInDatabase = new List<Quest>();
}
public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private GameObject questsHierarchy;
    [SerializeField]
    private GameObject questPrefab;
    
    public static QuestManager instance;
    //AtualQuests
    public List<Quest> activeQuests = new List<Quest>();
    //Invetory Quests
    public List<Quest> completedQuests = new List<Quest>();
    //Database
    public QuestDatabase database;
    private string jsonPath;
    public Quest[] questToAdd;
    private void Awake() {
        instance = this;
        jsonPath = Application.dataPath + "/QuestJsonDatabase.json";
    }
    private void Start() { 
        InsertOnDatabase();
    }
    #region JsonRegion
   public Quest Read(string questName) {
        if (File.Exists(jsonPath)) {
            QuestDatabase data = new QuestDatabase();
            string s = File.ReadAllText(jsonPath);
            data = JsonUtility.FromJson<QuestDatabase>(s);
            foreach (var var in data.allQuestsInDatabase) {
                if(var.questName == questName) {
                    return var;
                }
            }
        }
        return null;
    }
    void InsertOnDatabase() {
        QuestDatabase newData = new QuestDatabase();
        if (File.Exists(jsonPath)) {
            string content = File.ReadAllText(jsonPath);
            newData = JsonUtility.FromJson<QuestDatabase>(content);
            database = newData;
            bool contain = false;
            List<Quest> questsToAddBase = new List<Quest>();
            foreach (var var in newData.allQuestsInDatabase) {
                foreach (var x in questToAdd) {
                    if (var.questName == x.questName) {
                        contain = true;
                        return;
                    }else {
                        contain = false;
                    }
                }
                if (contain)
                    return;
                else {
                    foreach (var quest in questToAdd) {
                        newData.allQuestsInDatabase.Add(quest);
                    }
                    database = newData;
                    string s = JsonUtility.ToJson(newData, true);
                    File.WriteAllText(jsonPath, s);
                }
            } 
        }else {
            foreach (var quest in questToAdd) {
                newData.allQuestsInDatabase.Add(quest);
            }
            database = newData;
            string s = JsonUtility.ToJson(newData, true);
            File.WriteAllText(jsonPath, s);
        }
    }
    #endregion
    public void SendValidation(string questName, string phaseName) {
        if(activeQuests.Count == 0) {
            Debug.Log("No Quests Active");
            return;
        }
        foreach (var obj in activeQuests) {
            if (obj.questName == questName) {
                foreach (var var in obj.phases) {
                    if (var.name == phaseName) {
                        var.isComplete = true;
                        AttQuestPhase();
                        Debug.Log("Validation Sent to " + obj);
                        return;
                    }
                }
            }
        }
    }
    public bool CheckIfIsComplete(string questName) {
        foreach (var var in activeQuests) {
            if (var.questName == questName) {
                foreach (var phase in var.phases) {
                    if (phase.isComplete) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void AttQuestPhase()
    {
        foreach (var quest in activeQuests) {
            foreach (var phase in quest.phases) {
                if(phase.isComplete == false) {
                    quest.atualPhase = phase;
                    return;
                }
            }
        }
    }
    public bool CheckIfALreadyHaveQuest(string questName) {
        foreach (var var in activeQuests) {
            if (var.questName == questName) {
                return true;
            }
        }
        return false;
    }
    
    public void AddQuest(string questName) {
        activeQuests.Add(Read(questName));
       GameObject quest =  Instantiate(questPrefab, transform.position,quaternion.identity, questsHierarchy.transform);
       quest.GetComponentInChildren<TextMeshProUGUI>().text = Read(questName).questDescription;
       AttQuestPhase();
    }
    public void CompleteQuest(string questName) {
        AddLoyaltyPoints(Read(questName));
        AddItens(Read(questName));
        AddXP(Read(questName));
        CheckIfIsLinkedToAnotherQuest(Read(questName));
        completedQuests.Add(Read(questName));
        if (activeQuests.Count > 0) {
            foreach (var var in activeQuests) {
                if (var.questName == questName) {
                    activeQuests.Remove(var);
                    return;
                } 
            }
        }else {
            Debug.Log("No Quests Active, What are you doing?");
        }
    }
    private void CheckIfIsLinkedToAnotherQuest(Quest quest)
    {
        if (quest.rewards.questReward != "")
            AddQuest(quest.rewards.questReward);
        else
            return;
    }
    
    private static void AddXP(Quest quest) {
        PlayerStats.instance.GainXp(quest.rewards.xp);
    }
    
    private static void AddItens(Quest quest) {
        foreach (var var in quest.rewards.itens) {
            PlayerInventory.instance.AddItemToInventory(var);
        }
    }
    
    public void FailQuest(Quest quest) {
        activeQuests.Remove(quest);
    }
    private static void AddLoyaltyPoints(Quest quest) 
    {
        switch (quest.rewards.loyaltyType) {
            case Reward.LoyaltyType.City:
                LoyaltySystem.instance.AddPointsInfluenceCity(quest.rewards.loyalty);
                break;
            case Reward.LoyaltyType.Nature:
                LoyaltySystem.instance.AddPointsInfluenceNature(quest.rewards.loyalty);
                break;
        }
    }
}