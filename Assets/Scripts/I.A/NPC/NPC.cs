using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public enum CurrentState { Idle, Talking, Interacting, }
    public CurrentState currentState;
    public bool interactable;
    public enum NPCtYPE { Static, CanPatrol, }
    public NPCtYPE npcType;
    public string npcCode;
    public NavMeshAgent npcAgent;
    
    //TextArea
    [Header("Referencias do NPC")]
    public NPCReferenceData npcReference;
    public bool hasQuest;

    void Start() {
        if (npcType == NPCtYPE.Static) {
            return;
        }else if(npcType == NPCtYPE.CanPatrol) {
            npcAgent = this.GetComponent<NavMeshAgent>();
        }
    }
    public void Interact() {
        if (npcType == NPCtYPE.CanPatrol) { StopNpc(); }
        if (hasQuest) {
            
        }else {
            
        }
        
    }

    private void Talk() {
        
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
        EditorGUILayout.PropertyField(npcReferenceProp, true);
        serializedObject.ApplyModifiedProperties();
        if (myTarget.hasQuest) { 
            EditorGUILayout.PropertyField(serializedObject.FindProperty("questToGive"), true);   
            serializedObject.ApplyModifiedProperties();
        }
    }
} 
#endif