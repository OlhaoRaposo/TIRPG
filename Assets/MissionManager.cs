using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour {
    public static MissionManager instance;
    public List<Mission> missions;
    public List<GameObject> activeMissions;
    [SerializeField]private GameObject questPrefab;
    [SerializeField]private Transform missionGroup;
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }
   
    public void AddMission(string mission) { 
        Mission missionToAdd = GetMission(mission);
        GameObject missionObject = Instantiate(questPrefab, transform.position, Quaternion.identity, missionGroup);
        missionObject.GetComponent<QuestComponent>().questTitle.text = missionToAdd.missionTitle;
        missionObject.GetComponent<QuestComponent>().questDescription.text = missionToAdd.missionDescription;
        activeMissions.Add(missionObject);
    }
    private Mission GetMission(string missionTitle) {
        return missions.Find(mission => mission.missionTitle == missionTitle);
    }
    
    public void RemoveMission(string missionTitle) {
        GameObject missionToRemove = activeMissions.Find(mission => mission.GetComponent<QuestComponent>().questTitle.text == missionTitle);
        activeMissions.Remove(missionToRemove);
        Destroy(missionToRemove);
    }
}
[Serializable]
public class Mission {
    public string missionTitle;
    public string missionDescription;
}