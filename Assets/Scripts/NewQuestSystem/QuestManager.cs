using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    [Header("Adicionar todas quests do jogo aqui:")]
    public List<Quest> allQuestsInDatabase = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();

    private void Start() {
        if(instance == null) {
           instance = this; 
        }else {
           Destroy(this); }
        
        StartQuests();
    }
    private void StartQuests() {
        //Inicia a contagems das validações de todas Quests
        foreach (var quest in allQuestsInDatabase) {
            for (int i = 0; i < quest.step.GetProceduresCount; i++) {
                quest.validationCount.Add(false);
            }
        }
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
        Debug.Log("Was called AddValidation() from QuestManager.cs");
        Quest quest = FindActiveQuest(questCode);
        for (int i = 0; i < quest.validationCount.Count; i++) {
            if(quest.validationCount[i] == false) {
                quest.validationCount[i] = true;
                Debug.Log("Validação " + i + " da quest " + quest.code + " foi validada");
                return;
            }
        }
    }
    public void AddQuest(string questCode) {
        activeQuests.Add(FindQuestOnDatabase(questCode));
        if (activeQuests.Contains(FindQuestOnDatabase(questCode))) {
            foreach (var quest in activeQuests) {
                if (quest == FindQuestOnDatabase(questCode)) {
                    quest.SetActive();
                }
            }
        }
    }
}