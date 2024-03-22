using System;
using System.Collections;
using System.Collections.Generic;
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
    public DialogueDatabase dialogueDatabase;
    public bool hasQuest;
    public List<string> questToGive;
    public int atualQuestIndex;
    public string currentQuest;
    
    public bool isWriting,forcedStop;
    public float talkingSpeed = .8f;
    
    void Start() {
         if(npcType == NPCtYPE.CanPatrol) {
            npcAgent = this.GetComponent<NavMeshAgent>();
         } 
         npcReference.talkBox.SetActive(true);
         currentQuest = questToGive[atualQuestIndex];
        if (interactable) {
            npcReference.npcText.text = "";
            npcReference.npcNameReference.text = npcReference.npcName;
            npcReference.perfilImage.sprite = npcReference.perfilSprite;
        }
        npcReference.talkBox.SetActive(false);
    }
    public void Interact() {
        if (npcType == NPCtYPE.CanPatrol) { StopNpc(); }
        EnableChatBox();
        Talk();
    }
    #region ChatBox
    public void EnableChatBox() {
        talkBox.SetActive(true);
        PlayerCamera.instance.LockCamera(false);
        PlayerCamera.instance.ToggleAimLock(false);
        DefaultButtonConfiguration();
    }
    public void DisableChatBox() {
        npcReference.npcText.text = "";
        talkBox.SetActive(false);
        PlayerCamera.instance.LockCamera(true);
        PlayerCamera.instance.ToggleAimLock(true);
    }
    public void ResetDialogue() {
        npcReference.npcText.text = "";
    }
    private void DefaultButtonConfiguration() {
        npcReference.acceptButton.gameObject.SetActive(false);
        npcReference.refuseButton.gameObject.SetActive(false);
        npcReference.finishButton.gameObject.SetActive(true);
        npcReference.nextButton.gameObject.SetActive(true);
    }
    private void EnableLastBoxConfiguration() {
        npcReference.acceptButton.gameObject.SetActive(false);
        npcReference.refuseButton.gameObject.SetActive(false);
        npcReference.finishButton.gameObject.SetActive(true);
        npcReference.nextButton.gameObject.SetActive(false);
    }
    private void AcceptButtonConfiguration() {
        npcReference.acceptButton.gameObject.SetActive(true);
        npcReference.refuseButton.gameObject.SetActive(true);
        npcReference.finishButton.gameObject.SetActive(false);
        npcReference.nextButton.gameObject.SetActive(false);
    }
    #endregion
    public void Talk()
    {
        npcReference.npcText.text = "";
        if(isWriting) {
            forcedStop = true;
            return;
        }
        if (hasQuest) {
           //CALCULO CASO TENHA QUEST 
           if (QuestManager.instance.CheckIfIsComplete(currentQuest)) {
               //Minha quest em questao ja esta completa e nao tenho outra para dar
               if (questToGive.IndexOf(currentQuest) == questToGive.Count) {
                   int rnd = UnityEngine.Random.Range(0, dialogueDatabase.randomDialogues.Count);
                   Chat(dialogueDatabase.randomDialogues[rnd].dialogue);
                   EnableLastBoxConfiguration();
               }
           }else if (!QuestManager.instance.CheckIfIsActive(currentQuest)) {
               Quest quest = QuestManager.instance.FindQuestOnDatabase(currentQuest);
               if (currentDialogueIndex == quest.dialogue.Count) { 
                   Chat("Você deseja aceitar a missão?");
                   AcceptButtonConfiguration();
                   return;
               }
               if (!quest.dialogue[currentDialogueIndex].alreadySaid) {
                   Chat(quest.dialogue[currentDialogueIndex].dialogue);
                   quest.dialogue[currentDialogueIndex].alreadySaid = true;
                   currentDialogueIndex++;
               }
           }else if(!QuestManager.instance.CheckIfIsComplete(currentQuest)) {
               //Minha quest em questao nao esta completa
               if (QuestManager.instance.CheckIfIsActive(currentQuest)) {
                   //Minha quest em questao ja esta ativa
                   Quest myQuest = QuestManager.instance.FindActiveQuest(currentQuest);
                   Chat(myQuest.questAlreadyGiven[currentDialogueIndex].dialogue);
               }
           }
        }else {
            //CALCULO CASO NÃO TENHA QUEST
                //Pegar um diálogo aleatório
                int rnd = UnityEngine.Random.Range(0, dialogueDatabase.randomDialogues.Count);
                Chat(dialogueDatabase.randomDialogues[rnd].dialogue);
                EnableLastBoxConfiguration();
                
        }
    }
    /*public void TalkQuest()
    {
        Quest quest = QuestManager.instance.FindQuestOnDatabase(currentQuest);
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
    }*/
    public void AcceptQuest() {
        QuestManager.instance.AddQuest(currentQuest);
        atualQuestIndex++;
        DisableChatBox();
    }

    public void Chat(string text) { 
        StartCoroutine(WriteText(text,0));
    }
    private IEnumerator WriteText(string text, int aux) {
        int x = aux;
        npcReference.npcText.text += text[aux];
        yield return new WaitForSeconds(talkingSpeed);
        if (forcedStop) {
            StopCoroutine("WriteText");
            npcReference.npcText.text = text;
            forcedStop = false;
            yield break;
        }
        x++;
        if(aux < text.Length -1)
            StartCoroutine(WriteText(text, x));
    }
    private void StopNpc() {
        npcAgent.destination = this.transform.position;
        npcAgent.isStopped = true;
    }
}
[Serializable]
public class DialogueDatabase{
    public List<Dialogue> randomDialogues = new List<Dialogue>();
}

[Serializable]
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
        SerializedProperty dialogueDatabase = serializedObject.FindProperty("dialogueDatabase");
        SerializedProperty questToGive = serializedObject.FindProperty("questToGive");
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
        EditorGUILayout.PropertyField(dialogueDatabase, true);
        myTarget.npcCode = EditorGUILayout.TextField("NPC Code", myTarget.npcCode);
        myTarget.hasQuest = EditorGUILayout.Toggle("Has Quest", myTarget.hasQuest);
        if (myTarget.hasQuest) { 
            EditorGUILayout.PropertyField(serializedObject.FindProperty("questToGive"), true);   
        }
        myTarget.isWriting = EditorGUILayout.Toggle("Is Writing", myTarget.isWriting);
        myTarget.currentDialogueIndex = EditorGUILayout.IntField("Current Dialogue Index", myTarget.currentDialogueIndex);
        myTarget.currentQuest = EditorGUILayout.TextField("Current Quest", myTarget.currentQuest);
        myTarget.atualQuestIndex = EditorGUILayout.IntField("Atual Quest Index", myTarget.atualQuestIndex);
        EditorGUILayout.PropertyField(npcReferenceProp, true);
        serializedObject.ApplyModifiedProperties();
    }
} 
#endif