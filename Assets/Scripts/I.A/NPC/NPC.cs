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
        if (npcType == NPCtYPE.CanPatrol) {
            enemyAgent.SetDestination(transform.position);
        }
        if (isTalking) {
            Debug.Log("is Talking");
            timer = 0.0001f;
            isTalking = false;
        }else if (!isTalking){
            Debug.Log("Not Talking");
            if(TryGetComponent(out Animator animator))
                animator.SetTrigger("Interact");
            interactText.text = "";
            timer = 0.025f;
            SearchAndWrite(data);
        }
    }
    private void HandleInteractorDistance()
    {
        if(isInteractingWith == null)
            return;
        Vector3 distance = transform.position - isInteractingWith.transform.position;
        if (distance.magnitude >= 5)
        {
            textLineGuide = 0;
            interactText.enabled = false;
            timer = 0.000f;
            isTalking = false;
            interactText.text = "";
            isInteractingWith = null;
        }else
            interactText.enabled = true;
    }
    private void SearchAndWrite(NPCGuideDatabase data)
    {
        foreach (var obj in data.npcs) {
            if (obj.npcCode == npcCode + questToGive[questToGiveCount]) {
                if (textLineGuide == obj.text.Length) {
                    textLineGuide = 0;
                    interactText.text = "";
                    if (hasQuest) {
                        if (!QuestManager.instance.CheckIfALreadyHaveQuest(questToGive[0])) {
                            Debug.Log("AddQuest");
                            QuestManager.instance.AddQuest(npcCode + questToGive[0]);
                        }
                    }
                    return;
                }
            }
        }
        if (hasQuest) {
            if (QuestManager.instance.CheckIfALreadyHaveQuest(npcCode + questToGive[0])) {
                if (QuestManager.instance.CheckIfIsComplete(npcCode + questToGive[0])) {
                    textLineGuide = -1;
                    StartCoroutine(WriteOnCanvas(" ", 0));
                    StartCoroutine(WriteOnCanvas(data.npcs[0].questRewards, 0));
                    QuestManager.instance.CompleteQuest(npcCode + questToGive[0]);
                    hasQuest = false;
                    return;
                }
                foreach (var obj in data.npcs) {
                    if (obj.npcCode == npcCode + questToGive[questToGiveCount]) {
                        textLineGuide = -1;
                        StartCoroutine(WriteOnCanvas(obj.questAlreadyGiven, 0));
                        return;
                    }
                }
                Debug.Log("Contains");
            }else {
                Debug.Log("NotContains");
                foreach (var obj in data.npcs) {
                    if (obj.npcCode == npcCode + questToGive[questToGiveCount] ) {
                        if (textLineGuide == obj.text.Length) {
                            QuestManager.instance.AddQuest(npcCode + questToGive[questToGiveCount]);
                        }
                        if(textLineGuide <= obj.text.Length)
                            StartCoroutine(WriteOnCanvas(obj.text[textLineGuide].text, 0));
                    }
                }
            }
        }else {
            foreach (var obj in data.npcs) {
                if (obj.npcCode == npcCode) {
                    if(textLineGuide <= obj.text.Length)
                        StartCoroutine(WriteOnCanvas(obj.text[textLineGuide].text, 0));
                }
            }
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
        HandleInteractorDistance();
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
        switch (myTarget.npcType)
        {
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