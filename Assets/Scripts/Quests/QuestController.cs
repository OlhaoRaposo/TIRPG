using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class QuestController : MonoBehaviour
{
    public static QuestController instance;

    public List<QuestType> primaryMissions = new List<QuestType>();

    public List<QuestType> secondaryMissions = new List<QuestType>();

    public List<QuestType> temporaryMissions = new List<QuestType>();

    public List<QuestType> activeMissions = new List<QuestType>(3);

    public List<QuestType> completedMissions = new List<QuestType>();

    public List<Text> missionTexts = new List<Text>(3);

    public QuestProgress primary;
    public QuestProgress secondary;
    public QuestProgress temporary;
    public List<QuestProgress> controllers = new List<QuestProgress>(3);

    public GameObject[] introducao;
    public GameObject[] introducaoSab;
    public GameObject[] introducaoObj;

    void Awake()
    {
        instance = this;

        controllers.Add(primary);
        controllers.Add(secondary);
        controllers.Add(temporary);

        for (int index = 0; index < activeMissions[0].collectedItemsGoal.Count; index++)
        {
            controllers[0].collectedItems.Add(0);
        }
        for (int index = 0; index < activeMissions[0].deadEnemiesObjective.Count; index++)
        {
            controllers[0].enemiesKilled.Add(0);
        }
    }

void Start()
{
    //ChangeDialog(activeMissions[0]);
    ToggleDescription();
}

    void FixedUpdate()
    {
        /*introducao =  GameObject.FindGameObjectsWithTag("Introducao");
        foreach(GameObject obj in introducao)
        {
            if(!activeMissions[0].explorationZones.Contains(obj))
            {
                activeMissions[0].explorationZones.Add(obj);
            }
        }
        introducaoSab =  GameObject.FindGameObjectsWithTag("IntroducaoSab");
        foreach(GameObject obj in introducaoSab)
        {
            if(!activeMissions[0].sabotageZones.Contains(obj))
            {
                activeMissions[0].sabotageZones.Add(obj);
            }
        }
        introducaoObj =  GameObject.FindGameObjectsWithTag("IntroducaoObjectives");
        foreach(GameObject obj in introducaoObj)
        {
            if(!activeMissions[0].missionObjectives.Contains(obj))
            {
                activeMissions[0].missionObjectives.Add(obj);
            }
        }*/

        QuestType quest;

        for (int index = 0; index < activeMissions.Count; index++)
        {
            quest = activeMissions[index];
            
            int aux = 0;
            switch(quest.classification)
            {
                case QuestType.MissionRating.Primary:
                    aux = 0;
                    break;
                case QuestType.MissionRating.Secondary:
                    aux = 1;
                    break;
                case QuestType.MissionRating.Temporary:
                    aux = 2;
                    break;
            }

            if (quest.hasTimeout)
            {
                //quest.questTime += Time.fixedDeltaTime;
                controllers[aux].questTime += Time.fixedDeltaTime;

                if (/*quest*/controllers[aux].questTime > quest.questTimeLimit)
                {
                    controllers[aux].ResetQuest();
                    activeMissions.Remove(quest);
                    quest.inProgress = false;
                    ToggleDescription();
                }
            }
        }
    }

    public void CheckZones(GameObject zone)
    {
        foreach (QuestType quest in activeMissions)
        {
            int aux = 0;
            switch(quest.classification)
            {
                case QuestType.MissionRating.Primary:
                    aux = 0;
                    break;
                case QuestType.MissionRating.Secondary:
                    aux = 1;
                    break;
                case QuestType.MissionRating.Temporary:
                    aux = 2;
                    break;
            }

            switch (controllers[aux].type)
            {
                case QuestType.MissionType.Exploration:
                    if (zone == quest.explorationZones[controllers[aux].zoneStage]) 
                    {
                        controllers[aux].zoneStage++;
                        ChangeStage(quest);
                    }
                    break;
                case QuestType.MissionType.Sabotage:
                    if (zone == quest.sabotageZones[controllers[aux].sabotageZonesStage])
                    {
                        List<bool> sabotagedTargets = new List<bool>();

                        for (int index = 0; index < quest.objectivesInStage[controllers[aux].sabotageZonesStage]; index++)
                        {
                            sabotagedTargets.Add(false);
                        }

                        GameObject target;

                        for (int index = 0; index < quest.missionObjectives.Count; index++)
                        {
                            target = quest.missionObjectives[index];

                            if (!target.activeSelf && target == quest.missionObjectives[controllers[aux].sabotageZonesStage])
                            {
                                sabotagedTargets[index] = true;
                            }
                        }

                        bool auxiliaryCheck = true;

                        foreach (bool check in sabotagedTargets)
                        {
                            if (!check)
                            {
                                auxiliaryCheck = false;
                            }
                        }

                        if (auxiliaryCheck)
                        {
                            ChangeStage(quest);
                        }
                    }
                    break;
            }
        }
    }

    public void CollectedItems(QuestType.TypesOfCollectibles collectiblesName)
    {
        QuestType quest = null;

        for (int id = 0; id < activeMissions.Count; id++)
        {
            quest = activeMissions[id];

            int aux = 0;
            switch(quest.classification)
            {
                case QuestType.MissionRating.Primary:
                    aux = 0;
                    break;
                case QuestType.MissionRating.Secondary:
                    aux = 1;
                    break;
                case QuestType.MissionRating.Temporary:
                    aux = 2;
                    break;
            }

            if (controllers[aux].type == QuestType.MissionType.ItenCollection)
            {
                for (int index = 0; index < quest.missionCollectibles.Count; index++)
                {
                    if (collectiblesName == quest.missionCollectibles[index])
                    {
                        if (controllers[aux].collectedItems[index] < quest.collectedItemsGoal[index])
                        {
                            controllers[aux].collectedItems[index]++;
                        }
                    }
                }

                List<bool> auxiliaryCheck = new List<bool>(quest.missionCollectibles.Count);

                for (int index = 0; index < quest.collectedItemsGoal.Count; index++)
                {
                    auxiliaryCheck.Add(false);
                }

                for (int index = 0; index < quest.collectedItemsGoal.Count; index++)
                {
                    if (controllers[aux].collectedItems[index] == quest.collectedItemsGoal[index])
                    {
                        auxiliaryCheck[index] = true;
                    }
                }

                bool finalCheck = true;

                foreach (bool check in auxiliaryCheck)
                {
                    if (!check)
                    {
                        finalCheck = false;
                    }
                }

                if (finalCheck)
                {
                    ChangeStage(quest);
                }
            }
        }
    }

    public void EnemyEliminated(QuestType.EnemyTypes enenmyName)
    {
        QuestType quest = null;

        for (int id = 0; id < activeMissions.Count; id++)
        {
            quest = activeMissions[id];

            int aux = 0;
            switch(quest.classification)
            {
                case QuestType.MissionRating.Primary:
                    aux = 0;
                    break;
                case QuestType.MissionRating.Secondary:
                    aux = 1;
                    break;
                case QuestType.MissionRating.Temporary:
                    aux = 2;
                    break;
            }

            if (/*quest*/controllers[aux].type == QuestType.MissionType.KillEnemies)
            {
                for (int index = 0; index < quest.enemies.Count; index++)
                {
                    if (enenmyName == quest.enemies[index])
                    {
                        if (/*quest*/controllers[aux].enemiesKilled[index] < quest.deadEnemiesObjective[index])
                        {
                            /*quest*/controllers[aux].enemiesKilled[index]++;
                        }
                    }
                }

                List<bool> auxiliaryCheck = new List<bool>(quest.enemies.Count);

                for (int index = 0; index < quest.deadEnemiesObjective.Count; index++)
                {
                    auxiliaryCheck.Add(false);
                }

                for (int index = 0; index < quest.deadEnemiesObjective.Count; index++)
                {
                    if (/*quest*/controllers[aux].enemiesKilled[index] == quest.deadEnemiesObjective[index])
                    {
                        auxiliaryCheck[index] = true;
                    }
                }

                bool finalCheck = true;

                foreach (bool check in auxiliaryCheck)
                {
                    if (!check)
                    {
                        finalCheck = false;
                    }
                }

                if (finalCheck)
                {
                    ChangeStage(quest);
                }
            }
        }
    }

    void ChangeStage(QuestType quest)
    {
        int aux = 0;

        switch(quest.classification)
        {
            case QuestType.MissionRating.Primary:
                aux = 0;
                break;
            case QuestType.MissionRating.Secondary:
                aux = 1;
                break;
            case QuestType.MissionRating.Temporary:
                aux = 2;
                break;
        }

        if (/*quest*/controllers[aux].questStages < quest.stageMission.Count - 1 && quest.inProgress)
        {
            /*quest*/controllers[aux].questStages++;
            /*quest*/controllers[aux].type = quest.stageMission[/*quest*/controllers[aux].questStages];

            ChangeDialog(quest);

            ToggleDescription();
        }
        else
        {
            CompleteMission(quest);
        }
    }

    void ToggleDescription()
    {
        int aux = -1;
        switch (activeMissions.Count)
        {
            case 0:
                missionTexts[0].text = "";
                missionTexts[1].text = "";
                missionTexts[2].text = "";
                break;
            case 1:
                
                switch(activeMissions[0].classification)
                {
                    case QuestType.MissionRating.Primary:
                        aux = 0;
                        break;
                    case QuestType.MissionRating.Secondary:
                        aux = 1;
                        break;
                    case QuestType.MissionRating.Temporary:
                        aux = 2;
                        break;
                }

                if (activeMissions[0].questDescription.Count > 0 && activeMissions[0].questDescription[controllers[aux].questStages] != null)
                {
                    missionTexts[0].text = activeMissions[0].questDescription[controllers[aux].questStages];
                    missionTexts[1].text = "";
                    missionTexts[2].text = "";
                }
                else
                {
                    missionTexts[0].text = "";
                    missionTexts[1].text = "";
                    missionTexts[2].text = "";
                }
                break;
            case 2:
                switch(activeMissions[1].classification)
                {
                    case QuestType.MissionRating.Primary:
                        aux = 0;
                        break;
                    case QuestType.MissionRating.Secondary:
                        aux = 1;
                        break;
                    case QuestType.MissionRating.Temporary:
                        aux = 2;
                        break;
                }

                if (activeMissions[1].questDescription.Count > 0 && activeMissions[1].questDescription[controllers[aux].questStages] != null)
                {
                    missionTexts[1].text = activeMissions[1].questDescription[controllers[aux].questStages];
                    missionTexts[2].text = "";
                }
                else
                {
                    missionTexts[1].text = "";
                    missionTexts[2].text = "";
                }
                break;
            case 3:
                switch(activeMissions[2].classification)
                {
                    case QuestType.MissionRating.Primary:
                        aux = 0;
                        break;
                    case QuestType.MissionRating.Secondary:
                        aux = 1;
                        break;
                    case QuestType.MissionRating.Temporary:
                        aux = 2;
                        break;
                }

                if (activeMissions[2].questDescription.Count > 0 && activeMissions[2].questDescription[controllers[aux].questStages] != null)
                {
                    missionTexts[2].text = activeMissions[2].questDescription[controllers[aux].questStages];
                }
                else
                {
                    missionTexts[2].text = "";
                }
                break;
        }
    }

    public void ChangeDialog(QuestType quest)
    {
        int aux = 0;

        switch(quest.classification)
        {
            case QuestType.MissionRating.Primary:
                aux = 0;
                break;
            case QuestType.MissionRating.Secondary:
                aux = 1;
                break;
            case QuestType.MissionRating.Temporary:
                aux = 2;
                break;
        }

        //Verifica o tipo de missão.
        if (controllers[aux].type == QuestType.MissionType.Dialogue)
        {
            int auxiliaryCheck = 0;

            //Verifica se o estágio atual de dialogo é maior que a a introdução da missão.
            if (controllers[aux].dialogueStage > 0)
            {
                for (int index = controllers[aux].dialogueStage; index > 0; index--)
                {
                    auxiliaryCheck += (quest.dialoguesInStage[controllers[aux].dialogueStage - 1]);
                }
            }

            if (controllers[aux].indexDialogue - auxiliaryCheck != 0)
            {
                //Aumenta e indica o texto a ser lido.
                controllers[aux].indexDialogue++;
                
                //Verifica se já há um dialogo reproduzindo
                if(!DialogueManager.instance.isPlayingDialogue)
                {
                    //Reproduz o dialogo
                    quest.dialogue[controllers[aux].indexDialogue].Play();
                }
            }
            else
            {
                //Aumenta o estágio da missão.
                controllers[aux].dialogueStage++;
                //Muda o estágio da missão.
                ChangeStage(quest);
            }
        }
    }

    void AddReward(QuestType quest)
    {
        /*int aux = 0;
            switch(quest.classification)
            {
                case QuestType.MissionRating.Primary:
                    aux = 0;
                    break;
                case QuestType.MissionRating.Secondary:
                    aux = 1;
                    break;
                case QuestType.MissionRating.Temporary:
                    aux = 2;
                    break;
            }

        foreach(QuestType.RewardType type in quest.questReward)
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
        if (quest.team == QuestType.SideInfluence.City)
        {
            LoyaltySystem.instance.AddPointsInfluenceCity(quest.influencePoints);
        }
        else if (quest.team == QuestType.SideInfluence.Nature)
        {
            LoyaltySystem.instance.AddPointsInfluenceNature(quest.influencePoints);
        }
    }

    public void ActivateNextQuest(QuestType quest)
    {
        if (activeMissions.Count > 0)
        {
            foreach (QuestType q in activeMissions)
            {
                if (q.questName == quest.questName)
                {
                    activeMissions.Remove(quest);
                    break;
                }
            }
        }

        bool auxiliaryCheck = false;

        foreach (QuestType q in activeMissions)
        {
            if (q.classification == QuestType.MissionRating.Primary)
            {
                auxiliaryCheck = true;
            }
        }

        if (!auxiliaryCheck)
        {
            for (int index = 0; index < quest.nextMission.collectedItemsGoal.Count; index++)
            {
                controllers[0].collectedItems.Add(0);
            }
            for (int index = 0; index < quest.nextMission.deadEnemiesObjective.Count; index++)
            {
                controllers[0].enemiesKilled.Add(0);
            }
            activeMissions.Add(quest.nextMission);
            ChangeDialog(quest);
            ToggleDescription();
            quest.nextMission.inProgress = true;
        }
    }

    public void ActivateSecondaryMission(QuestType quest)
    {
        bool auxiliaryCheck = false;

        foreach (QuestType q in activeMissions)
        {
            if (q.classification == QuestType.MissionRating.Secondary)
            {
                auxiliaryCheck = true;
            }
        }

        if (!auxiliaryCheck)
        {
            for (int index = 0; index < quest.collectedItemsGoal.Count; index++)
            {
                controllers[1].collectedItems.Add(0);
            }
            for (int index = 0; index < quest.deadEnemiesObjective.Count; index++)
            {
                controllers[1].enemiesKilled.Add(0);
            }
            activeMissions.Add(quest);
            quest.inProgress = true;
        }

        ChangeDialog(quest);
        ToggleDescription();
    }

    public void ActivateTemporaryMission(QuestType quest)
    {
        bool auxiliaryCheck = false;

        foreach (QuestType q in activeMissions)
        {
            if (q.classification == QuestType.MissionRating.Temporary)
            {
                auxiliaryCheck = true;
            }
        }

        if (!auxiliaryCheck)
        {
            for (int index = 0; index < quest.collectedItemsGoal.Count; index++)
            {
                controllers[2].collectedItems.Add(0);
            }
            for (int index = 0; index < quest.deadEnemiesObjective.Count; index++)
            {
                controllers[2].enemiesKilled.Add(0);
            }
            activeMissions.Add(quest);
            quest.inProgress = true;
        }

        ChangeDialog(quest);
        ToggleDescription();
    }

    public void CompleteMission(QuestType quest)
    {
        int aux = 0;
        switch(quest.classification)
        {
            case QuestType.MissionRating.Primary:
                aux = 0;
                break;
            case QuestType.MissionRating.Secondary:
                aux = 1;
                break;
            case QuestType.MissionRating.Temporary:
                aux = 2;
                break;
        }

        controllers[aux].ResetQuest();

        AddInfluence(quest);

        AddReward(quest);

        if (quest.nextMission != null)
        {
            ActivateNextQuest(quest);
        }
        else
        {
            activeMissions.Remove(quest);
        }

        quest.inProgress = false;
        quest.concluded = true;

        if (!completedMissions.Contains(quest))
        {
            completedMissions.Add(quest);
        }

        ToggleDescription();
    }
}