using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Tekoha.Database
{
    [Serializable]
    public class Save {
        //PlayerPosition
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        //CameraPosition
        public Vector3 cameraPosition;
        public Quaternion cameraRotation;
        
        //BossAlreadyDefeated
        public List<string> bossesDefeated;
        
        //PlayerInventory
            //nem ideia vey
            
        //Loyalty points
        public float cityLoyalty;
        public float natureLoyalty;
            
        //Tutorial 
        public bool tutorialDone;
        
        //NPCS
        public List<SaveNPC> npcsInteracted;
        
        //Time of the day
        public string currentHour;
    }
    
    [Serializable]
    public class SaveNPC {
        public string npcName;
        public int currentDialogueIndex;
    }
}
