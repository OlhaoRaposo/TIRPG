using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
    #endif

[CreateAssetMenu(fileName = "QuestSystem", menuName = "QuestSystem/New Quest", order = 1)]
public class Quest : ScriptableObject
{
    public string questCode;
    public bool hasAReward;
    public Rewards Reward;
    public QuestDialogue questDialogue;
    public QuestDialogue questAlreadyGiven;
    public List<Validation> steps;

}
[CreateAssetMenu(fileName = "QuestValidation", menuName = "QuestSystem/New Validation", order = 1)]
[Serializable]
public class Validation : ScriptableObject { 
    public enum ValidationType { killMobs,GetItems,WalkToAPlace, }
    public bool isComplete;
    public ValidationType typeOfValidation;
    public string bestiaryCode;
    public int mobsToKill;
    public int mobsKilled;
    public int itensToGet;
    public int itensGotten;
    public Collider placeToWalk;
    public Collider player;

    public void Calculate() {
        if (typeOfValidation == ValidationType.killMobs) {
            if (mobsKilled == mobsToKill) {
                isComplete = true;
            }
        }else if (typeOfValidation == ValidationType.GetItems){
            if (itensGotten == itensToGet) {
                isComplete = true;
            }
        }else if (typeOfValidation == ValidationType.WalkToAPlace) {
            if (player.bounds.Intersects(placeToWalk.bounds)) {
                isComplete = true;
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Validation))]
    public class ValidationEditor : Editor
    {
        public Validation target;

        public void OnEnable()
        {
            target = (Validation)serializedObject.targetObject;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            target.isComplete = EditorGUILayout.Toggle("Is Complete", target.isComplete);
            target.typeOfValidation = (ValidationType)EditorGUILayout.EnumPopup("Type of Validation", target.typeOfValidation);
            if (target.typeOfValidation == ValidationType.killMobs) { 
                target.bestiaryCode = EditorGUILayout.TextField("Bestiary Code", target.bestiaryCode);
                target.mobsToKill = EditorGUILayout.IntField("Mobs to Kill", target.mobsToKill);
                target.mobsKilled = EditorGUILayout.IntField("Mobs Killed", target.mobsKilled);
            }else if (target.typeOfValidation == ValidationType.GetItems){
                target.itensToGet = EditorGUILayout.IntField("Itens to Get", target.itensToGet);
                target.itensGotten = EditorGUILayout.IntField("Itens Gotten", target.itensGotten);
            }else if (target.typeOfValidation == ValidationType.WalkToAPlace) {
                target.player = (Collider)EditorGUILayout.ObjectField("Player", target.player, typeof(Collider), true);
                target.placeToWalk = (Collider)EditorGUILayout.ObjectField("Place to Walk", target.placeToWalk, typeof(Collider), true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
[Serializable]
public class Rewards {
    public enum LoyaltyType{None,City, Nature}
    [Header("Loyalty Type")]
    public LoyaltyType loyaltyType;
    public float influence;
    [Header("XP")]
    public float xp;
}
[Serializable]
public class QuestDialogue {
    public List<Dialogue> dialogues = new List<Dialogue>();
}

