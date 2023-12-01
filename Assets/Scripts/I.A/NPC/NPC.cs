using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using TMPro;
using UnityEngine.UI;
public class MyTexts
{
    public string npcCode;
    public string[] text;
}
[CustomEditor(typeof(NPC))]
public class NPC : MonoBehaviour
{
    public bool interactable;
    public enum NPCtYPE { Static, Patroling, }
    public NPCtYPE npcType;
    public string npcCode;
    //SerializeText
    private string jsonPath;
    
    public TextMeshProUGUI interactText;
    public NavMeshAgent enemyAgent;
    
    private bool isPatroling, isIddle;
    private Vector3 patrolDestination;
    private bool hasArrived;

    void Start()
    {
        if(interactable)
            ReadJson();
        if (npcType == NPCtYPE.Static) {
            return;
        }else {
            enemyAgent = this.GetComponent<NavMeshAgent>();
            StartCoroutine(StartPatrolling());
            StartCoroutine(CheckIfIsStoped());
        }
    }
    MyTexts ReadJson()
    {
        MyTexts myTexts = new MyTexts();
        if (File.Exists(jsonPath))
        {
            string s = File.ReadAllText(jsonPath);
           myTexts = JsonUtility.FromJson<MyTexts>(s);
        }
        return myTexts;
    }
    void Interact()
    {
        if (npcType == NPCtYPE.Static) {
            
        }else {
            
        }
        
        if (interactable) {
            string[] text = ReadJson().text;
            
            
        }
    }

    IEnumerator WriteOnCanvas(string text,int aux)
    {
        float timer = .2f;
        interactText.text += text[aux];
        yield return new WaitForSeconds(timer);
        if(aux <= text.Length)
            StartCoroutine(WriteOnCanvas(text, aux++));
    }
    private void Update() {
        DettectPatrolDistance();
    }
    IEnumerator StartPatrolling()
    {
        isPatroling = true;
        isIddle = false;
        NavMeshHit hit;
        if (isPatroling) {
            patrolDestination = transform.position + (Random.insideUnitSphere * 20) + new Vector3(0,5,0);
            if (Physics.Raycast(patrolDestination, Vector3.down, out RaycastHit hitInfo)) {
                enemyAgent.SetDestination(hitInfo.point);
                if (NavMesh.SamplePosition(hitInfo.point, out hit, .5f, NavMesh.AllAreas))
                {
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
}
#if UNITY_EDITOR
[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        NPC myTarget = (NPC)target;
        myTarget.interactable = EditorGUILayout.Toggle("Interactable", myTarget.interactable);
        myTarget.npcType = (NPC.NPCtYPE)EditorGUILayout.EnumPopup("NPC Type", myTarget.npcType);
        EditorGUILayout.Space();
        myTarget.npcCode = EditorGUILayout.TextField("NPC Code", myTarget.npcCode);
        EditorGUILayout.Space();
        switch (myTarget.npcType)
        {
            case NPC.NPCtYPE.Static:
               myTarget.interactText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Interact Text", myTarget.interactText, typeof(TextMeshProUGUI), true);
                break;
            case NPC.NPCtYPE.Patroling:
                myTarget.interactText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Interact Text", myTarget.interactText, typeof(TextMeshProUGUI), true);
                myTarget.enemyAgent = (NavMeshAgent)EditorGUILayout.ObjectField("Enemy Agent", myTarget.enemyAgent, typeof(NavMeshAgent), true);
                break;
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    } 
}
#endif