using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tekoha.Database;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public string path = Application.dataPath + "/tekohaSave.json";
    public static SaveController instance;
    public void Awake() {
        path = Application.dataPath + "/tekohaSave.json";
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit() {
        if(WorldController.worldController.tutorialCompleted)
            SaveGame();
    }

    public void MenuStart() {
        if (File.Exists(path))
            MenuAux.instance.continueBttn.SetActive(true);
        else {
            MenuAux.instance.continueBttn.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
            SaveGame();
        if(Input.GetKeyDown(KeyCode.K))
           LoadGame();
    }

    public void SaveGame() {
     Save save = new Save();
     List<EnemyBehaviour> enemys = FindObjectsOfType<EnemyBehaviour>().ToList();
     List<Interactable_FastTravel> fastTravels = FindObjectsOfType<Interactable_FastTravel>().ToList();
     //GetClosest fast travel
     
     Interactable_FastTravel closestFastTravel = null;
     foreach (var ft in fastTravels) {
            if (closestFastTravel == null) {
                closestFastTravel = ft;
            } else {
                if (Vector3.Distance(PlayerMovement.instance.gameObject.transform.position, ft.gameObject.transform.position) <
                    Vector3.Distance(PlayerMovement.instance.gameObject.transform.position, closestFastTravel.gameObject.transform.position)) {
                    closestFastTravel = ft;
                }
            }
     }
     foreach (var en in enemys) {
         Vector3 distance = en.gameObject.transform.position - PlayerMovement.instance.gameObject.transform.position;
         if(distance.magnitude < 50) {
           PlayerMovement.instance.TeleportPlayer(closestFastTravel.gameObject.transform.position);
         }
     }
     
     //Quests
     save.quests = MissionManager.instance.missions.Select(mission => mission.missionTitle).ToList();
     
     save.playerPosition = PlayerMovement.instance.gameObject.transform.position;
     save.playerRotation = PlayerMovement.instance.gameObject.transform.rotation;
     save.cameraPosition = PlayerMovement.instance.gameObject.transform.position;
     save.cameraRotation = PlayerMovement.instance.gameObject.transform.rotation;
     save.bossesDefeated = WorldController.worldController.bossesDefeated;
     
     save.avaliablePoints = PlayerStats.instance.availablePoints;
     save.xp = PlayerStats.instance.currentXp;
     save.currentLevel = PlayerStats.instance.GetLevel();
     save.skillsLearned = PlayerStats.instance.skills;
     save.strengthPoint = PlayerStats.instance.GetStrength();
     save.agilityPoint = PlayerStats.instance.GetAgility();
     save.endurancePoint = PlayerStats.instance.GetEndurance();
     save.intelligencePoint = PlayerStats.instance.GetIntelligence();

     save.playerLife = PlayerHPController.instance.GetHp();
     save.cityLoyalty = LoyaltySystem.instance.GetInfluencePointsCity();
     save.natureLoyalty = LoyaltySystem.instance.GetInfluencePointsNature();
     save.tutorialDone = WorldController.worldController.tutorialCompleted;
     save.currentHour = WorldController.worldController.currentHour.ToString();
     List<NPC> npcs = FindObjectsOfType<NPC>().ToList();
     save.npcsInteracted = new List<SaveNPC>();
     foreach (var npc in npcs) {
         if (npc.invoked) {
             save.npcsInteracted.Add(new SaveNPC() {
                 npcName = npc.gameObject.name,
                 hasQuest = npc.hasQuest,
                 currentDialogueIndex = npc.currentDialogueIndex
             });
         }
     }
     string json = JsonUtility.ToJson(save,true);
     File.WriteAllText(path, json);
    }
    public void LoadGame() {
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            Save save = JsonUtility.FromJson<Save>(json);
            //Player
            PlayerMovement.instance.TeleportPlayer(save.playerPosition);
            PlayerMovement.instance.gameObject.transform.rotation = save.playerRotation;
            PlayerHPController.instance.SetHP(save.playerLife);
            
            //TODO
            //SET STATS POINTS
            foreach (var skills in save.skillsLearned) {
                SkillTree.instance.ForceAcquireSkill(skills);
            }
            
            PlayerStats.instance.SetStrength(save.strengthPoint);
            PlayerStats.instance.SetAgility(save.agilityPoint);
            PlayerStats.instance.SetEndurance(save.endurancePoint);
            PlayerStats.instance.SetIntelligence(save.intelligencePoint);
            PlayerStats.instance.SetXp(save.xp);
            PlayerStats.instance.SetLevel(save.currentLevel);
            PlayerStats.instance.SetAvailablePoints(save.avaliablePoints);
            
            //Quests
            foreach (var quest in save.quests) {
                MissionManager.instance.AddMission(quest);
            }
            
            //Camera
            PlayerCameraMovement.instance.gameObject.transform.rotation = save.cameraRotation;
            //Bosses
            List<EnemyBehaviour> enemies = FindObjectsOfType<EnemyBehaviour>().ToList();
            foreach (var boss in enemies) {
                foreach (var defeatedBoss in save.bossesDefeated) {
                    if (boss.gameObject.name == defeatedBoss) {
                        boss.isDefeted = true;
                        if(boss.signal != null)
                            boss.signal.SetActive(false);
                        boss.gameObject.SetActive(false);
                    }
                }
            }
            //Loyalty
            LoyaltySystem.instance.AddPointsInfluenceCity(save.cityLoyalty);
            LoyaltySystem.instance.AddPointsInfluenceNature(save.natureLoyalty);
            //Tutorial
            WorldController.worldController.tutorialCompleted = save.tutorialDone;
            //NPCS
            List<NPC> npcs = FindObjectsOfType<NPC>().ToList();
            foreach (var npc in npcs) {
                foreach (var saveNpc in save.npcsInteracted) {
                    if (npc.gameObject.name == saveNpc.npcName) {
                        npc.invoked = true;
                        npc.hasQuest = saveNpc.hasQuest;
                        npc.currentDialogueIndex = saveNpc.currentDialogueIndex;
                    }
                }
            }
            //Time
            WorldController.worldController.currentHour = Convert.ToDateTime(save.currentHour);
        }
    }
    public void DeleteSave() {
        File.Delete(path);
    }
    public bool FileExists() {
        return File.Exists(path);
    }
}
