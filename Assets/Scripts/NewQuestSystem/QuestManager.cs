using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [Header("Adicionar todas quests do jogo aqui:")]
    public List<Quest> allQuestsInDatabase = new List<Quest>();
    [Header("Para fins de teste:")]
    public List<Quest> allActiveQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();
    public Quest ReturnQuest(string questCode) {
        foreach (var quest in allQuestsInDatabase) {
            if (quest.questCode == questCode) {
                return quest;
            }
        }
        return null;
    }
    private void Start() {
        if (instance == null) {
            instance = this;
        }else Destroy(this);
    }

    #region AuxiliarRegion
    
    public bool IsQuestActive(Quest quest)
    {
        bool aux = false;
        foreach (var obj in allActiveQuests) {
          if(obj.questCode == quest.questCode)
              aux = true;
        }
        return aux;
    }
    public bool IsQuestComplete(Quest quest)
    {
        bool aux = false;
        foreach (var obj in completedQuests) {
            if(obj.questCode == quest.questCode)
                aux = true;
        }
        return aux;
    }
    #endregion

    public void AddQuest(Quest quest) {
        allActiveQuests.Add(quest);
    }
    public void RemoveQuest(Quest quest) {
        allActiveQuests.Remove(quest);
    }
    public void CompleteQuest(Quest quest) {
        completedQuests.Add(quest);
        if (quest.hasAReward) {
            AddReward(quest);
        }
        allActiveQuests.Remove(quest);
    }
    public void AddReward(Quest quest) {
        //Add reward to player
    }
    
}