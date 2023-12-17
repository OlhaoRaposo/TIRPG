using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
public class NPCGuideDatabase
{
    public List<NpcTexts> npcs = new List<NpcTexts>();
}
public class NPC : MonoBehaviour
{
    public bool interactable;
    public enum NPCtYPE { Static, CanPatrol, }
    public NPCtYPE npcType;
    public string npcCode;
    public GameObject hasQuestIcon;
    public GameObject talkPanel;
    public int textLineGuide;
    //SerializeText
    private string jsonPath;
    public bool isTalking = false;
    public TextMeshProUGUI interactText;
    public NavMeshAgent enemyAgent;
    public bool hasQuest;
    public int questCount;
    public string[] questToGive;
    public int questToGiveCount;
    
    float timer = .02f;
    private bool isPatroling, isIddle;
    private Vector3 patrolDestination;
    private bool hasArrived;
    private GameObject isInteractingWith;
    NPCGuideDatabase data = new NPCGuideDatabase();
    private void Awake() {
        jsonPath = Application.dataPath + "/NPCJsonDatabase.json";
    }
    void Start()
    {
        if (File.Exists(jsonPath))
        {
            string s = File.ReadAllText(jsonPath);
            data = JsonUtility.FromJson<NPCGuideDatabase>(s);
        }

        if (npcType == NPCtYPE.Static) {
            return;
        }else if(npcType == NPCtYPE.CanPatrol) {
            enemyAgent = this.GetComponent<NavMeshAgent>();
            StartCoroutine(StartPatrolling());
            StartCoroutine(CheckIfIsStoped());
        }
    }
    
    public void Interact(GameObject interactor)
    {
        isInteractingWith = interactor; 
        if (npcType == NPCtYPE.CanPatrol) { StopTheNpc(); }
        
        if (isTalking) {
            timer = 0.0000001f;
            isTalking = false;
        }else if (!isTalking){
            HandleTalk();
        }
    } 
    private void HandleTalk() {
        interactText.text = "";
        timer = 0.015f;

        foreach (var quest in QuestManager.instance.activeQuests) {
            if (QuestManager.instance.CheckIfIsComplete(quest.questName)) {
                if (quest.npcToComplete == npcCode) {
                    RewardTalk();
                }
            }
        }
        
        if (hasQuest) {
            if(!isTalking)
                if(QuestManager.instance.CheckIfALreadyHaveQuest(npcCode + questToGive[0])){
                    if (QuestManager.instance.CheckIfIsComplete(npcCode + questToGive[0]))
                        HandleQuests();
                    else
                        QuestGivenTalk();
                }else
                 QuestTalk();
        }else
            NormalTalk();
    }
    private void QuestTalk() {
        foreach (var npc in data.npcs) {
            if(npc.npcCode == npcCode + questToGive[0]) {
                if (textLineGuide < npc.text.Length)
                    StartCoroutine(WriteOnCanvas(npc.text[textLineGuide].text, 0));
                else if(textLineGuide >= npc.text.Length) {
                    textLineGuide = 0;
                    HandleQuests();
                    return;
                }
            }
        }
    }
    private void NormalTalk() {
       
        foreach (var texts in data.npcs) {
            if (texts.npcCode == npcCode) {
                if (textLineGuide >= texts.text.Length) {
                    textLineGuide = 0;
                    return;
                }
                if (textLineGuide < texts.text.Length)
                    StartCoroutine(WriteOnCanvas(texts.text[textLineGuide].text, 0));
            }
        }
    }
    private void RewardTalk() {
        foreach (var npc in data.npcs) {
            if(npc.npcCode == npcCode + questToGive[0]) {
                QuestManager.instance.CompleteQuest(npcCode + questToGive[0]);
                StartCoroutine(DefaultTalk(npc.questRewards, 0));
            }
        }
    }
    private void QuestGivenTalk() {
        foreach (var npc in data.npcs) {
            if(npc.npcCode == npcCode + questToGive[0]) {
                StartCoroutine(DefaultTalk(npc.questAlreadyGiven, 0));
            }
        }
    }
    private void HandleQuests()
    {
        if (hasQuest) {
            if (QuestManager.instance.CheckIfALreadyHaveQuest(npcCode + questToGive[0])) {
                Debug.Log("Quest Was already Given by: " + npcCode );
                if (QuestManager.instance.CheckIfIsComplete(npcCode + questToGive[0])) {
                    Debug.Log("Quest Was Completed");
                    RewardTalk();
                    return;
                }
            }else {
                Debug.Log("Quest Was not Given ");
                QuestManager.instance.AddQuest(npcCode + questToGive[0]);
            }
        }
    }
    private void HandleInteractorDistance()
    {
        if (isInteractingWith == null) {
            talkPanel.SetActive(false);
            return; }
        
        Vector3 distance = transform.position - isInteractingWith.transform.position;
        if (distance.magnitude >= 6) {
            talkPanel.SetActive(false);
            textLineGuide = 0;
            interactText.enabled = false;
            timer = 0.000f;
            isTalking = false;
            interactText.text = "";
            isInteractingWith = null;
        }else {
            talkPanel.SetActive(true);
        }
    }
    IEnumerator DefaultTalk(string text,int stringAux)
    {
        isTalking = true;
        interactText.text += text[stringAux];
        yield return new WaitForSeconds(timer);
        if (stringAux < text.Length - 1) {
            int newint = stringAux + 1;
            StartCoroutine(WriteOnCanvas(text, newint));
        }else {
            isTalking = false;
        }
    }
   
    IEnumerator WriteOnCanvas(string text,int stringAux)
    {
        isTalking = true;
        interactText.text += text[stringAux];
        yield return new WaitForSeconds(timer);
        if (stringAux < text.Length - 1) {
            int newint = stringAux + 1;
            StartCoroutine(WriteOnCanvas(text, newint));
        }else {
            isTalking = false;
            textLineGuide++;
        }
    }
    private void Update() {
        DettectPatrolDistance();
        if(!interactable)
            return;
        HandleInteractorDistance();
        if(hasQuest)
            CheckIfQuestIsActive();
    }

    void CheckIfQuestIsActive() {
       if(!QuestManager.instance.CheckIfALreadyHaveQuest(npcCode + questToGive[0])) {
           foreach (var quest in QuestManager.instance.database.allQuestsInDatabase) {
               if (quest.questName == npcCode + questToGive[0]) {
                   hasQuestIcon.SetActive(true);
               }else {
                   return;
               }
           }
       }else if (QuestManager.instance.CheckIfIsComplete(npcCode + questToGive[0]) && QuestManager.instance.Read(npcCode + questToGive[0]).npcToComplete == npcCode) {
           hasQuestIcon.SetActive(true);
       }else {
           hasQuestIcon.SetActive(false);
       }
    }
    private void StopTheNpc()
    {
        enemyAgent.SetDestination(transform.position);
    }
    #region PatrolRegion
    IEnumerator StartPatrolling()
    {
        isPatroling = true;
        isIddle = false;
        NavMeshHit hit;
        if (isPatroling) {
            patrolDestination = transform.position + (Random.insideUnitSphere * 20) + new Vector3(0,5,0);
            if (Physics.Raycast(patrolDestination, Vector3.down, out RaycastHit hitInfo)) {
                enemyAgent.SetDestination(hitInfo.point);
                if (NavMesh.SamplePosition(hitInfo.point, out hit, .5f, NavMesh.AllAreas)) {
                    patrolDestination = hit.position + new Vector3( 0,0,0);
                    enemyAgent.SetDestination(patrolDestination);
                    hasArrived = false;
                }
            }
        }
        yield return new WaitUntil(() => hasArrived);
        int odds = Random.Range(0, 100);
        if (odds <= 25) {
            StartCoroutine(Iddle());
        }else
            StartCoroutine(StartPatrolling());
    }
    IEnumerator Iddle()
    {
        isIddle = true;
        isPatroling = false;
        enemyAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(Random.Range(3, 8));
        StartCoroutine(StartPatrolling());
    }
    IEnumerator CheckIfIsStoped()
    {
        Vector3 pos0 = transform.position;
        yield return new WaitForSeconds(8);
        Vector3 pos1 = transform.position;
        Vector3 distance = pos1 - pos0;
        if (distance.magnitude <= 1) 
            StartCoroutine(StartPatrolling());
        StartCoroutine(CheckIfIsStoped());
    }
    private void DettectPatrolDistance()
    {
        Vector3 distance = transform.position - patrolDestination;
        if (distance.magnitude <= 2) {
            hasArrived = true;
        }
    }
        #endregion
}
#if UNITY_EDITOR
[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    private SerializedProperty stringArray;
    void OnEnable()
    {
        stringArray = serializedObject.FindProperty("questToGive");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        NPC myTarget = (NPC)target;
        myTarget.interactable = EditorGUILayout.Toggle("Interactable", myTarget.interactable);
        if(!myTarget.interactable)
            return;
        myTarget.npcType = (NPC.NPCtYPE)EditorGUILayout.EnumPopup("NPC Type", myTarget.npcType);
        myTarget.talkPanel = (GameObject)EditorGUILayout.ObjectField("Talk Panel", myTarget.talkPanel, typeof(GameObject), true);
        switch (myTarget.npcType) {
            case NPC.NPCtYPE.Static:
                myTarget.interactText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Interact Text", myTarget.interactText, typeof(TextMeshProUGUI), true);
                break;
            case NPC.NPCtYPE.CanPatrol:
                myTarget.interactText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Interact Text", myTarget.interactText, typeof(TextMeshProUGUI), true);
                myTarget.enemyAgent = (NavMeshAgent)EditorGUILayout.ObjectField("Enemy Agent", myTarget.enemyAgent, typeof(NavMeshAgent), true);
                break;
        }
        myTarget.npcCode = EditorGUILayout.TextField("NPC Code", myTarget.npcCode);
        myTarget.hasQuest = EditorGUILayout.Toggle("Has Quest", myTarget.hasQuest);
        if (myTarget.hasQuest) { 
            myTarget.hasQuestIcon = (GameObject)EditorGUILayout.ObjectField("Has Quest Icon", myTarget.hasQuestIcon, typeof(GameObject), true);
            myTarget.questCount = EditorGUILayout.IntField("Quest Count", myTarget.questCount);
            if (stringArray != null) {
                if (GUILayout.Button("Adicionar Nova Quest")) {
                    AddNewQuest();
                }
                if (stringArray.arraySize > 0 && GUILayout.Button("Remover Última Quest")) {
                    RemoveLastQuest();
                }
                EditQuests();
            }
        }
        void EditQuests()
        {
            EditorGUILayout.Space();

            for (int i = 0; i < stringArray.arraySize; i++)
            {
                SerializedProperty quest = stringArray.GetArrayElementAtIndex(i);
                EditorGUI.BeginChangeCheck();
                string newQuest = EditorGUILayout.TextField("Quest " + i, quest.stringValue);
                if (EditorGUI.EndChangeCheck())
                {
                    quest.stringValue = newQuest;
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
        void AddNewQuest()
        {
            stringArray.arraySize++;
            serializedObject.ApplyModifiedProperties();
        }
        void RemoveLastQuest() {
            stringArray.arraySize--;
            serializedObject.ApplyModifiedProperties();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    } 
}
#endif