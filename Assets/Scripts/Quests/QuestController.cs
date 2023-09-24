using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
    public static QuestController instance;
    
    [SerializeField]List<QuestType> primaryMissions = new List<QuestType>();

    [SerializeField]List<QuestType> secondaryMissions = new List<QuestType>();

    [SerializeField]List<QuestType> temporaryMissions = new List<QuestType>();

    [SerializeField]List<QuestType> activeMissions = new List<QuestType>(3);

    [SerializeField]List<QuestType> completedMissions = new List<QuestType>();

    [SerializeField]List<Text> missionTexts = new List<Text>(3);

    void Awake()
    {
        //Garante que exista apenas um controlador na cena.
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

   void Start()
   {
        ChangeDialog(activeMissions[0]);
   }

    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.X))
        {
            EnemyEliminated(QuestType.EnemyTypes.MilitaryRobots);
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            CollectedItems(QuestType.TypesOfCollectibles.Fruits);
        }

        if(Input.GetMouseButtonDown(1))
        {
            ChangeStage(activeMissions[0]);
        }*/
    }
    
    void FixedUpdate()
    {
        QuestType quest;

        for(int index = 0; index < activeMissions.Count; index++)
        {
            quest = activeMissions[index];

            if(quest.hasTimeout)
            {
                quest.questTime += Time.fixedDeltaTime;

                if(quest.questTime > quest.questTimeLimit)
                {
                    activeMissions.Remove(quest);
                    quest.inProgress = false;
                    ToggleDescription();
                }
            }
        }
    }
    
    void CheckZones(GameObject zone)
    {
        foreach(QuestType quest in activeMissions)
        {
            switch(quest.type)
            {
                case QuestType.MissionType.Exploration:
                    foreach(GameObject zones in quest.explorationZones)
                    {
                        if(zone == zones)
                        {
                            ChangeStage(quest);
                        }
                    }
                    break;
                case QuestType.MissionType.Sabotage:
                    foreach(GameObject zones in quest.sabotageZones)
                    {
                        if(zone == zones)
                        {
                            List<bool> sabotagedTargets = new List<bool>();

                            for(int index = 0; index < quest.missionObjectives.Count; index++)
                            {
                                sabotagedTargets.Add(false);
                            }

                            GameObject target;

                            for(int index = 0; index < quest.missionObjectives.Count; index++)
                            {
                                target = quest.missionObjectives[index];

                                if(!target.activeSelf)
                                {
                                    sabotagedTargets[index] = true;
                                }
                            }

                            bool auxiliaryCheck = true;

                            foreach(bool check in sabotagedTargets)
                            {
                                if(!check)
                                {
                                    auxiliaryCheck = false;
                                }
                            }

                            if(auxiliaryCheck)
                            {
                                ChangeStage(quest);
                            }
                        }
                    }
                    break;
            }
        }
    }
    
    void CollectedItems(QuestType.TypesOfCollectibles collectiblesName)
    {
        
        QuestType quest = null;

        for(int id = 0; id < activeMissions.Count; id++)
        {
            quest = activeMissions[id];

            if(quest.type == QuestType.MissionType.ItenCollection)
            {
                for(int index = 0; index < quest.missionCollectibles.Count; index++)
                {
                    if(collectiblesName == quest.missionCollectibles[index])
                    {
                        if(quest.collectedItems[index] < quest.collectedItemsGoal[index])
                        {
                            quest.collectedItems[index]++;
                        }
                    }
                }

                List<bool> auxiliaryCheck = new List<bool>(quest.missionCollectibles.Count);

                for(int index = 0; index < quest.collectedItemsGoal.Count; index++)
                {
                    auxiliaryCheck.Add(false);
                }

                for(int index = 0; index < quest.collectedItemsGoal.Count; index++)
                {
                    if(quest.collectedItems[index] == quest.collectedItemsGoal[index])
                    {
                        auxiliaryCheck[index] = true;
                    }
                }
                
                bool finalCheck = true;

                foreach(bool check in auxiliaryCheck)
                {
                    if(!check)
                    {
                        finalCheck = false;
                    }
                }

                if(finalCheck)
                {
                    ChangeStage(quest);
                }
            }
        }
    }
    
    void EnemyEliminated(QuestType.EnemyTypes enenmyName)
    {
        
        QuestType quest = null;

        for(int id = 0; id < activeMissions.Count; id++)
        {
            quest = activeMissions[id];

            if(quest.type == QuestType.MissionType.KillEnemies)
            {
                for(int index = 0; index < quest.enemies.Count; index++)
                {
                    if(enenmyName == quest.enemies[index])
                    {
                        if(quest.enemiesKilled[index] < quest.deadEnemiesObjective[index])
                        {
                            quest.enemiesKilled[index]++;
                        }
                    }
                }

                List<bool> auxiliaryCheck = new List<bool>(quest.enemies.Count);

                for(int index = 0; index < quest.deadEnemiesObjective.Count; index++)
                {
                    auxiliaryCheck.Add(false);
                }

                for(int index = 0; index < quest.deadEnemiesObjective.Count; index++)
                {
                    if(quest.enemiesKilled[index] == quest.deadEnemiesObjective[index])
                    {
                        auxiliaryCheck[index] = true;
                    }
                }
                
                bool finalCheck = true;

                foreach(bool check in auxiliaryCheck)
                {
                    if(!check)
                    {
                        finalCheck = false;
                    }
                }

                if(finalCheck)
                {
                    ChangeStage(quest);
                }
            }
        }
    }
    
    void ChangeStage(QuestType quest)
    {
        if(quest.questStages < quest.stageMission.Count - 1 && quest.inProgress)
        {
            quest.questStages++;
            quest.type = quest.stageMission[quest.questStages];
            ToggleDescription();
        }
        else
        {
            CompleteMission(quest);
        }
    }
    
    void ToggleDescription()
    {
        switch(activeMissions.Count)
        {
            case 0:
                missionTexts[0].text = "";
                missionTexts[1].text = "";
                missionTexts[2].text = "";
                break;
            case 1:
                if(activeMissions[0].questDescription.Count > 0)
                {
                    missionTexts[0].text = activeMissions[0].questDescription[activeMissions[0].questStages];
                }
                else
                {
                    missionTexts[0].text = "";
                }
                break;
            case 2:
                if(activeMissions[1].questDescription.Count > 0)
                {
                    missionTexts[1].text = activeMissions[1].questDescription[activeMissions[1].questStages];
                }
                else
                {
                    missionTexts[1].text = "";
                }
                break;
            case 3:
                if(activeMissions[2].questDescription.Count > 0)
                {
                    missionTexts[2].text = activeMissions[2].questDescription[activeMissions[2].questStages];
                }
                else
                {
                    missionTexts[2].text = "";
                }
                break;
        }
    }
    
    void ChangeDialog(QuestType quest)
    {
        if(!DialogueManager.instance.isPlayingDialogue)
        {
            quest.dialogue[0].Play();
        }
    }
    
    void SwitchCharacter()
    {
        
    }
    
    void ToggleAnimation()
    {
        
    }
    
    void ChangeImage()
    {
        
    }
    
    void AddReward(QuestType quest)
    {
        /*foreach(QuestType.RewardType type in quest.questReward)
        {
            if(type == QuestType.RewardType.Guns || type == QuestType.RewardType.Accessories)
            {
                switch(type)
                {
                    case QuestType.RewardType.Guns:
                        //foreach(Desbloqueaveis premio in listaDeArmasBloqueados)
                        {
                            for(int index = 0; index < quest.rewardObjects.Count; index++)
                            {
                                string nomeDaArma = quest.rewardObjects[index].ToString();
                                string teste = premio.ToString();

                                if(nomeDaArma == teste)
                                {
                                    premio = quest.unlockItems[index];
                                }
                            }
                        }
                        break;
                    case QuestType.RewardType.Accessories:
                        //foreach(Desbloqueaveis premio in listaDeAcessoriosBloqueados)
                        {
                            for(int index = 0; index < quest.rewardObjects.Count; index++)
                            {
                                string nomeDoAcessorio = quest.rewardObjects[index].ToString();
                                string teste = premio.ToString();

                                if(nomeDoAcessorio == teste)
                                {
                                    premio = quest.unlockItems[index];
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                switch(type)
                {
                    case QuestType.RewardType.SkillPoints:
                        int auxilary = -1;
                        for(int index = 0; index < quest.questReward.Count; index++)
                        {
                            if(type != QuestType.RewardType.Guns && type != QuestType.RewardType.Accessories)
                            {
                                auxilary++;
                                if(type == QuestType.RewardType.SkillPoints)
                                {
                                    SkillTreeController.instance.poits += quest.quantity[auxilary];
                                }
                            }
                        }
                        break;
                    case QuestType.RewardType.Throwables:
                        int auxilary = -1;
                        for(int index = 0; index < quest.questReward.Count; index++)
                        {
                            if(type != QuestType.RewardType.Guns && type != QuestType.RewardType.Accessories)
                            {
                                auxilary++;
                                if(type == QuestType.RewardType.Throwables)
                                {
                                    
                                }
                            }
                        }
                        break;
                    case QuestType.RewardType.Consumables;
                        int auxilary = -1;
                        for(int index = 0; index < quest.questReward.Count; index++)
                        {
                            if(type != QuestType.RewardType.Guns && type != QuestType.RewardType.Accessories)
                            {
                                auxilary++;
                                if(type == QuestType.RewardType.Consumables)
                                {
                                    
                                }
                            }
                        }
                        break;
                }
            }
        }*/
    }
    
    void AddInfluence(QuestType quest)
    {
        if(quest.team == QuestType.SideInfluence.City)
        {
            LoyaltySystem.instance.AddPointsInfluenceCity(quest.influencePoints);
        }
        else if(quest.team == QuestType.SideInfluence.Nature)
        {
            LoyaltySystem.instance.AddPointsInfluenceNature(quest.influencePoints);
        }
    }
    
    void ActivateNextQuest(QuestType quest)
    {
        if(activeMissions.Count > 0)
        {
            foreach(QuestType q in activeMissions)
            {
                if(q.questName == quest.questName)
                {
                    activeMissions.Remove(quest);
                    break;
                }
            }
        }
        
        bool auxiliaryCheck = false;

        foreach(QuestType q in activeMissions)
        {
            if(q.classification == QuestType.MissionRating.Primary)
            {
                auxiliaryCheck = true;
            }
        }

        if(!auxiliaryCheck)
        {
            activeMissions.Add(quest.nextMission);
            quest.nextMission.inProgress = true;
        }
    }
    
    void ActivateSecondaryMission(QuestType quest)
    {
        bool auxiliaryCheck = false;

        foreach(QuestType q in activeMissions)
        {
            if(q.classification == QuestType.MissionRating.Secondary)
            {
                auxiliaryCheck = true;
            }
        }

        if(!auxiliaryCheck)
        {
            activeMissions.Add(quest);
            quest.inProgress = true;
        }

        ToggleDescription();
    }
    
    void ActivateTemporaryMission(QuestType quest)
    {
        bool auxiliaryCheck = false;

        foreach(QuestType q in activeMissions)
        {
            if(q.classification == QuestType.MissionRating.Temporary)
            {
                auxiliaryCheck = true;
            }
        }

        if(!auxiliaryCheck)
        {
            activeMissions.Add(quest);
            quest.inProgress = true;
        }

        ToggleDescription();
    }
    
    void CompleteMission(QuestType quest)
    {
        AddInfluence(quest);

        AddReward(quest);

        if(quest.nextMission != null)
        {
            ActivateNextQuest(quest);
        }
        else
        {
            activeMissions.Remove(quest);
        }

        quest.inProgress = false;
        quest.concluded = true;

        if(!completedMissions.Contains(quest))
        {
            completedMissions.Add(quest);
        }
        
        ToggleDescription();
    }
}