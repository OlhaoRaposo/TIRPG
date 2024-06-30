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

    private void Start() {
        LoadGame();
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
     save.playerPosition = PlayerMovement.instance.gameObject.transform.position;
     save.playerRotation = PlayerMovement.instance.gameObject.transform.rotation;
     save.cameraPosition = PlayerMovement.instance.gameObject.transform.position;
     save.cameraRotation = PlayerMovement.instance.gameObject.transform.rotation;
     save.bossesDefeated = WorldController.worldController.bossesDefeated;
     save.currentLevel = PlayerStats.instance.level;
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
            for (int i = 0; i < save.currentLevel; i++) {
                PlayerStats.instance.LevelUp();
            }
            //Camera
            PlayerCameraMovement.instance.gameObject.transform.rotation = save.cameraRotation;
            //Bosses
            List<EnemyBehaviour> enemies = FindObjectsOfType<EnemyBehaviour>().ToList();
            foreach (var boss in enemies) {
                foreach (var defeatedBoss in save.bossesDefeated) {
                    if (boss.gameObject.name == defeatedBoss) {
                        boss.isDefeted = true;
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
                        npc.currentDialogueIndex = saveNpc.currentDialogueIndex;
                    }
                }
            }
            //Time
            WorldController.worldController.currentHour = Convert.ToDateTime(save.currentHour);
        }else
            SaveGame();
    }
    void DeleteSave()
    {
        File.Delete(path);
    }
}
