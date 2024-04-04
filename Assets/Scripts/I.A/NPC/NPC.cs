using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    public enum CurrentState { Idle, Talking, Interacting, }
    public CurrentState currentState;
    public bool canInteractWithOtherNPC;
    public bool interactable;
    public GameObject talkBox;
    public enum NPCtYPE { Static, CanPatrol, }
    public NPCtYPE npcType;
    public string npcCode;
    public NavMeshAgent npcAgent;
    public int currentDialogueIndex;
    public bool currentQuestIsCompleted;

    public Vector3 patrolDestination;
    public bool hasArrived;
    private Coroutine patrol;
    public bool canInteractAgain;
    
    //TextArea
    [Header("Referencias do NPC")]
    public NPCReferenceData npcReference;
    public DialogueDatabase dialogueDatabase;
    public bool hasQuest;
    public List<string> questToGive;
    public int atualQuestIndex;
    public string currentQuest;
    void Start()
    {
        patrol = StartCoroutine(Patrol());
         if(npcType == NPCtYPE.CanPatrol) {
            npcAgent = GetComponent<NavMeshAgent>();
            patrolDestination = ReturnARandomPoint(transform.position);
            StartCoroutine(Patrol());
         } 
         npcReference.talkBox.SetActive(true);
         currentQuest = questToGive[atualQuestIndex];
        if (interactable) {
            npcReference.npcText.text = "";
            npcReference.npcNameReference.text = npcReference.npcName;
            npcReference.perfilImage.sprite = npcReference.perfilSprite;
        }
        npcReference.talkBox.SetActive(false);
        if(hasQuest)
            HandleQuestIcon();
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
    public void Talk() {
        if (currentQuestIsCompleted) {
            npcReference.questIcon.SetActive(true);
            Quest quest = QuestManager.instance.FindQuestOnDatabase(currentQuest);
            if(currentDialogueIndex == quest.questReward.rewardDialogue.Count) {
                currentDialogueIndex = 0;
                EnableLastBoxConfiguration();
                QuestManager.instance.CompleteQuest(currentQuest);
                npcReference.questIcon.SetActive(false);
                atualQuestIndex++;
                currentQuest = questToGive[atualQuestIndex];
            }else {
                TypeWritter.instance.Write(npcReference.npcText, quest.questReward.rewardDialogue[currentDialogueIndex].dialogue);
                TypeWritter.instance.AttCurrentIndex(this);
                DefaultButtonConfiguration();
            }
        }
        if (!hasQuest) {
            int rnd = Random.Range(0, dialogueDatabase.randomDialogues.Count);
            TypeWritter.instance.Write(npcReference.npcText, dialogueDatabase.randomDialogues[rnd].dialogue);
        }else if (hasQuest) {
            //Quest ja esta concluida
            if (QuestManager.instance.CheckIfIsComplete(currentQuest)) {
                Quest quest = QuestManager.instance.FindQuestOnDatabase(currentQuest);
                if(currentDialogueIndex == quest.questReward.rewardDialogue.Count) {
                    currentDialogueIndex = 0;
                    EnableLastBoxConfiguration();
                }
                TypeWritter.instance.Write(npcReference.npcText, quest.questReward.rewardDialogue[currentDialogueIndex].dialogue);
                TypeWritter.instance.AttCurrentIndex(this);
            }else if(!QuestManager.instance.CheckIfIsComplete(currentQuest) && QuestManager.instance.CheckIfIsActive(currentQuest)) {
                //Quests não esta completa mas esta ativa
                Quest quest = QuestManager.instance.FindQuestOnDatabase(currentQuest);
                TypeWritter.instance.Write(npcReference.npcText, quest.questAlreadyGiven[currentDialogueIndex].dialogue);
                TypeWritter.instance.AttCurrentIndex(this);
                if (currentDialogueIndex == quest.questAlreadyGiven.Count) {
                    currentDialogueIndex = 0;
                    EnableLastBoxConfiguration(); 
                }
            }else if (!QuestManager.instance.CheckIfIsComplete(currentQuest) && !QuestManager.instance.CheckIfIsActive(currentQuest)) {
                //Quest nao esta nem completa nem ativa
                Quest quest = QuestManager.instance.FindQuestOnDatabase(currentQuest);
                if (currentDialogueIndex == quest.dialogue.Count) {
                    TypeWritter.instance.Write(npcReference.npcText, "Você aceita essa missão?");
                    AcceptButtonConfiguration();
                    currentDialogueIndex = 0;
                }else {
                    TypeWritter.instance.Write(npcReference.npcText, quest.dialogue[currentDialogueIndex].dialogue);
                    TypeWritter.instance.AttCurrentIndex(this);
                }
            }
        }
        HandleQuestIcon();
    }
    public void QuestIsCompleted() {
        currentQuestIsCompleted = true;
    }
    public void HandleQuestIcon(){
        Debug.Log("HandleQuestIcon");
        if (currentQuestIsCompleted) {
            npcReference.questIcon.SetActive(true);
            Debug.Log("Active True");
        }else if(hasQuest && !QuestManager.instance.CheckIfIsActive(currentQuest)) {
            npcReference.questIcon.SetActive(true);
            Debug.Log("Active True");
        }else if (QuestManager.instance.CheckIfIsActive(currentQuest)) {
            npcReference.questIcon.SetActive(false);
            Debug.Log("Active False");
        }
    }
    public void AcceptQuest() {
        QuestManager.instance.AddQuest(currentQuest);
        atualQuestIndex++;
        HandleQuestIcon();
        DisableChatBox();
    }
    private void StopNpc() {
        npcAgent.destination = this.transform.position;
        npcAgent.isStopped = true;
    }
    private void Update() {
        CheckRemainingDistance();
        //HandleNpcInteraction();
    }
    private void HandleNpcInteraction() {
        if (canInteractWithOtherNPC) {
            Collider[] objects;
            objects = Physics.OverlapSphere(transform.position, 10);
            foreach (var detections in objects) {
                if (detections.TryGetComponent(out NPC npc)) {
                    if (npc.canInteractAgain) {
                        canInteractAgain = false;                     
                        StopCoroutine(patrol);                        
                        StopNpc();                                    
                        currentState = CurrentState.Interacting;      
                        StartCoroutine(BackToPatrol());               
                        StartCoroutine(ResetInteraction());           
                                              
                        npc.canInteractAgain = false;                 
                        npc.StopCoroutine(npc.patrol);                
                        npc.StopNpc();                                
                        npc.currentState = CurrentState.Interacting;  
                        npc.StartCoroutine(BackToPatrol());           
                        npc.StartCoroutine(ResetInteraction());       
                    }
                }
            }
        }                
    }

    IEnumerator BackToPatrol() {
        yield return new WaitForSeconds(Random.Range(5, 8));
        StartCoroutine(Patrol());
    }                
    IEnumerator ResetInteraction() {
        yield return new WaitForSeconds(Random.Range(6, 12));
        canInteractAgain = true;
    }

    private void CheckRemainingDistance() {
        Vector3 distance = transform.position - patrolDestination;
        if (distance.magnitude <= 4) {
            hasArrived = true;
        }else {
            hasArrived = false;
        }
        Debug.Log(distance.magnitude);
    }

    IEnumerator Patrol() {
        StartCoroutine(ImStuckStepBro(patrolDestination));
        if(hasArrived)    
            npcAgent.SetDestination(ReturnARandomPoint(transform.position));
        Debug.Log("Patroling");
        yield return new WaitForSeconds(3);
        StartCoroutine(Patrol());
    }

    IEnumerator ImStuckStepBro(Vector3 basePosition) {
        Vector3 pos = basePosition;
        yield return new WaitForSeconds(10);
        if(pos == patrolDestination) {
            npcAgent.SetDestination(ReturnARandomPoint(transform.position));
        }
    }
    private Vector3 ReturnARandomPoint(Vector3 referentialPoint) {
      Vector3 point;  
      point = referentialPoint + Random.insideUnitSphere * 12;
      point.y += 3;
      if(Physics.Raycast(point, Vector3.down, out RaycastHit hit, 20)) {
        point = hit.point;
      }
      patrolDestination = point;
      return point;
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
    public GameObject questIcon;
    public Button nextButton;
    public Button acceptButton;
    public Button refuseButton;
    public Button finishButton;
}
#if UNITY_EDITOR
[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    public SerializedProperty questArray;
    public SerializedProperty npcReference;
    public SerializedProperty npcStates;
    
    void OnEnable() {
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
        myTarget.canInteractWithOtherNPC = EditorGUILayout.Toggle("Can Interact With Other NPC", myTarget.canInteractWithOtherNPC);
        if (myTarget.canInteractWithOtherNPC) {
            myTarget.canInteractAgain = EditorGUILayout.Toggle("Can Interact Again", myTarget.canInteractAgain);
        }
        myTarget.npcType = (NPC.NPCtYPE)EditorGUILayout.EnumPopup("NPC Type", myTarget.npcType);
        switch (myTarget.npcType) {
            case NPC.NPCtYPE.Static:
                break;
            case NPC.NPCtYPE.CanPatrol:
                myTarget.npcAgent = (NavMeshAgent)EditorGUILayout.ObjectField("Enemy Agent", myTarget.npcAgent, typeof(NavMeshAgent), true);
                myTarget.patrolDestination = EditorGUILayout.Vector3Field("Patrol Destination", myTarget.patrolDestination);
                myTarget.hasArrived = EditorGUILayout.Toggle("Has Arrived", myTarget.hasArrived);
                break;
        }
        EditorGUILayout.PropertyField(dialogueDatabase, true);
        myTarget.npcCode = EditorGUILayout.TextField("NPC Code", myTarget.npcCode);
        myTarget.hasQuest = EditorGUILayout.Toggle("Has Quest", myTarget.hasQuest);
        if (myTarget.hasQuest) { 
            EditorGUILayout.PropertyField(serializedObject.FindProperty("questToGive"), true);   
        }
        myTarget.currentDialogueIndex = EditorGUILayout.IntField("Current Dialogue Index", myTarget.currentDialogueIndex);
        myTarget.currentQuest = EditorGUILayout.TextField("Current Quest", myTarget.currentQuest);
        myTarget.atualQuestIndex = EditorGUILayout.IntField("Atual Quest Index", myTarget.atualQuestIndex);
        EditorGUILayout.PropertyField(npcReferenceProp, true);
        serializedObject.ApplyModifiedProperties();
    }
} 
#endif