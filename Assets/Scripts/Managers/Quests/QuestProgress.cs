using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestProgress : MonoBehaviour
{
    public QuestType.MissionType type = QuestType.MissionType.Dialogue;
    public int questStages = 0;
    public float questProgress = 0;
    public float questTime = 0;
    public int dialogueStage = 0;
    public int indexDialogue = -1;
    public int zoneStage = 0;
    public int sabotageZonesStage = 0;
    public int objectivesStage = 0;
    public List<int> collectedItems = new List<int>();
    public List<int> enemiesKilled = new List<int>();

    public void ResetQuest()
    {
        type = QuestType.MissionType.Dialogue;
        questStages = 0;
        questProgress = 0;
        questTime = 0;
        dialogueStage = 0;
        indexDialogue = -1;
        collectedItems.Clear();
        enemiesKilled.Clear();
    }
}
