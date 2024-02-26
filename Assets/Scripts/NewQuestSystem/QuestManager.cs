using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [Header("Adicionar todas quests do jogo aqui:")]
    public List<Quest> allQuestsInDatabase = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    private void Start() {
        if(instance == null) {
           instance = this; 
        }else {
           Destroy(this); }
    }
    public Quest FindQuestOnDatabase(string questCode) {
        Quest questToReturn = null;
        foreach (var quest in allQuestsInDatabase) {
            if(quest.code == questCode) {
                questToReturn = quest;
            }
        }
        return questToReturn;
    }
    public Quest FindActiveQuest(string questCode) {
        Quest questToReturn = null;
        foreach (var quest in activeQuests) {
            if(quest.code == questCode) {
                questToReturn = quest;
            }
        }
        return questToReturn;
    }
    
    public void AddValidation(string questCode) {
        Quest quest = FindActiveQuest(questCode);
        int index =0;
        foreach (var step in quest.steps) {
            if (!step.isComplete) {
                step.isComplete = true;
                CalculateValidations(quest);
                quest.currentStep++;
                if (index < quest.steps.Count) {
                    if(!quest.isComplete)
                        quest.steps[index+1].SetActive();
                }
                return;
            }
            index++;
        }
    }
    private void CalculateValidations(Quest quest) {
        bool isCompleted = false;
        foreach (var step in quest.steps) {
            if (!step.isComplete) {
                isCompleted = false;
                return;
            }else {
                isCompleted = true;
            }
        }
        if (isCompleted) {
            quest.isComplete = true;
            CompleteQuest(quest);
        }
    }
    public void CompleteQuest(Quest quest){
        completedQuests.Add(quest);
        activeQuests.Remove(quest);
        AddReward(quest);
        AudioBoard.instance.PlayAudio("Bell");
    }
    private void AddReward(Quest quest) {
        if(quest.questReward.royaltyType == Reward.RoyaltyType.city) {
            LoyaltySystem.instance.AddPointsInfluenceCity(quest.questReward.royaltyValue);
        }else if (quest.questReward.royaltyType == Reward.RoyaltyType.nature) {
            LoyaltySystem.instance.AddPointsInfluenceNature(quest.questReward.royaltyValue);
        }
        if (quest.hasQuestReward) {
            AddQuest(quest.questRewardCode);
        }
    }
    public void AddQuest(string questCode) {
        activeQuests.Add(FindQuestOnDatabase(questCode));
        if (activeQuests.Contains(FindQuestOnDatabase(questCode))) {
            foreach (var quest in activeQuests) {
                if (quest == FindQuestOnDatabase(questCode)) {
                   quest.steps[0].SetActive();
                }
            } 
        }
    }
}