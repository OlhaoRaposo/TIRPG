using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    public enum CurrentState { Idle, Talking, Interacting, }
    public CurrentState currentState;
    public bool interactable;
    public GameObject talkBox;
    public enum NPCtYPE { Static, CanPatrol, }
    public NPCtYPE npcType;
    public string npcCode;
    public NavMeshAgent npcAgent;
    public int currentDialogueIndex;
    
    //TextArea
    [Header("Referencias do NPC")]
    public NPCReferenceData npcReference;
    public bool hasQuest;
    public string questToGive;

    void Start() {
         if(npcType == NPCtYPE.CanPatrol) {
            npcAgent = this.GetComponent<NavMeshAgent>();
            
         } 
         npcReference.talkBox.SetActive(true);
        if (interactable) {
            npcReference.npcNameReference.text = npcReference.npcName;
            npcReference.perfilImage.sprite = npcReference.perfilSprite;
        }
        npcReference.talkBox.SetActive(false);
    }
    public void Interact() {
        if (npcType == NPCtYPE.CanPatrol) { StopNpc(); }
        if (hasQuest) { 
            EnableChatBox();
            TalkQuest();
        }else {
            
        }
    }
    #region ChatBox
    public void EnableChatBox() {
        talkBox.SetActive(true);
        PlayerCamera.instance.LockCamera(true);
        PlayerCamera.instance.ToggleAimLock(false);
        DefaultButtonConfiguration();
    }
    public void DisableChatBox() {
        npcReference.npcText.text = "";
        talkBox.SetActive(false);
        PlayerCamera.instance.LockCamera(false);
        PlayerCamera.instance.ToggleAimLock(true);
        ResetDialogue();
    }

    private void ResetDialogue() {
        Quest quest = QuestManager.instance.FindQuestOnDatabase(questToGive);
        StartCoroutine(WriteText(""));
        foreach (var dialogue in quest.dialogue) {
            dialogue.alreadySaid = false;
        }
        currentDialogueIndex = 0;
    }

    private void DefaultButtonConfiguration() {
        npcReference.acceptButton.gameObject.SetActive(false);
        npcReference.refuseButton.gameObject.SetActive(false);
        npcReference.finishButton.gameObject.SetActive(true);
        npcReference.nextButton.gameObject.SetActive(true);
    }
    private void AcceptButtonConfiguration() {
        npcReference.acceptButton.gameObject.SetActive(true);
        npcReference.refuseButton.gameObject.SetActive(true);
        npcReference.finishButton.gameObject.SetActive(false);
        npcReference.nextButton.gameObject.SetActive(false);
    }
    #endregion
    public void TalkQuest()
    {
        npcReference.npcText.text = "";
        Quest quest = QuestManager.instance.FindQuestOnDatabase(questToGive);
        if (currentDialogueIndex == quest.dialogue.Count) { 
            ResetDialogue();
            StartCoroutine(WriteText("Você deseja aceitar a missão?"));
            AcceptButtonConfiguration();
            return;
        }
        if (!quest.dialogue[currentDialogueIndex].alreadySaid) {
            StartCoroutine(WriteText(quest.dialogue[currentDialogueIndex].dialogue));
            quest.dialogue[currentDialogueIndex].alreadySaid = true;
            currentDialogueIndex++;
        }
    }
    public void AcceptQuest() {
        QuestManager.instance.AddQuest(questToGive);
        DisableChatBox();
    }
    private IEnumerator WriteText(string text) {
        foreach (char letter in text) {
            npcReference.npcText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void StopNpc() {
        npcAgent.destination = this.transform.position;
        npcAgent.isStopped = true;
    }
}
[System.Serializable]
public class NPCReferenceData :  PropertyAttribute
{
    [Header("Características do NPC")]
    public Sprite perfilSprite;
    public Image perfilImage;
    public string npcName;
    public TextMeshProUGUI npcNameReference;
    [Header("Referência do NPC em cena")]
    public TextMeshProUGUI npcText;
    public GameObject talkBox;
    public Button nextButton;
    public Button acceptButton;
    public Button refuseButton;
    public Button finishButton;
    [Header("Prefabs Utilizados")]
    public GameObject HasQuestIcon;
}
#if UNITY_EDITOR
[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    public SerializedProperty questArray;
    public SerializedProperty npcReference;
    public SerializedProperty npcStates;
    
    void OnEnable()
    {
        questArray = serializedObject.FindProperty("questToGive");
        npcReference = serializedObject.FindProperty("npcReference");
        npcStates = serializedObject.FindProperty("npcStates");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty npcReferenceProp = serializedObject.FindProperty("npcReference");
        NPC myTarget = (NPC)target;
        myTarget.currentState = (NPC.CurrentState)EditorGUILayout.EnumPopup("NPC State", myTarget.currentState);
        myTarget.interactable = EditorGUILayout.Toggle("Interactable", myTarget.interactable);
        if(!myTarget.interactable)
            return;
        myTarget.currentDialogueIndex = EditorGUILayout.IntField("Current Dialogue Index", myTarget.currentDialogueIndex);
        myTarget.talkBox = (GameObject)EditorGUILayout.ObjectField("Talk Box", myTarget.talkBox, typeof(GameObject), true);
        EditorGUILayout.Space();
        myTarget.npcType = (NPC.NPCtYPE)EditorGUILayout.EnumPopup("NPC Type", myTarget.npcType);
        switch (myTarget.npcType) {
            case NPC.NPCtYPE.Static:
                break;
            case NPC.NPCtYPE.CanPatrol:
                myTarget.npcAgent = (NavMeshAgent)EditorGUILayout.ObjectField("Enemy Agent", myTarget.npcAgent, typeof(NavMeshAgent), true);
                break;
        }
        myTarget.npcCode = EditorGUILayout.TextField("NPC Code", myTarget.npcCode);
        myTarget.hasQuest = EditorGUILayout.Toggle("Has Quest", myTarget.hasQuest);
        if (myTarget.hasQuest) { 
            EditorGUILayout.PropertyField(serializedObject.FindProperty("questToGive"), true);   
        }
        EditorGUILayout.PropertyField(npcReferenceProp, true);
        serializedObject.ApplyModifiedProperties();
    }
} 
#endif