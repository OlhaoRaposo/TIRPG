using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InteractiveObject : MonoBehaviour
{
    public enum ObjectPossibilities {
        SkillCheck,Notes,Door,OnlyInteract
    }
    public GameObject QuestCanvas;
    public ObjectPossibilities objectType;
    //Notes Variables
    [SerializeField]
    public string noteText;
    public RenderTexture noteTexture;
    public Camera noteCamera;
    public GameObject noteInstance;
    public GameObject noteObject;
    //Skill Check Variables
    public enum objectRelated {None, Door, Quest,} public objectRelated objectRelatedTo;
    public string questRelataed;
    public string phaseRelated;
    public int skillCheckCount;
    public List<bool> validations = new List<bool>();
    public enum SkillType { CircleSkillCheck, BarSkillCheck, } public SkillType skillType;
    public GameObject circleSkillCheckObject, barSkillCheckObject;
    

    GameObject skillCheck;

    private void Start() {
        if (objectRelatedTo == objectRelated.Quest) {
           GameObject obj = Instantiate(QuestCanvas, transform.position + new Vector3(0,3,0), quaternion.identity);
           QuestCanvas = obj;
           QuestCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        CheckIconTime();
    }

    private void CheckIconTime()
    {
       
    }

    public void Interact()
    {
        switch (objectType)
        {
            case ObjectPossibilities.SkillCheck:
                StartSkilCheck();
                break;
            case ObjectPossibilities.Notes:
                    InstantiateNote();
                break;
            case ObjectPossibilities.Door:
                OpenDoor();
                break;
            case ObjectPossibilities.OnlyInteract:
                InteractWithQuest();
                break;
        }
    }
    #region SkillCheckRegion
    void StartSkilCheck()
    {
        
        if(skillCheckCount == 0)
            skillCheckCount = Random.Range(1, 5);
        
        if(skillCheckCount == 0)
            skillCheckCount = Random.Range(1, 5);

        if (skillType == SkillType.CircleSkillCheck) {
             skillCheck = Instantiate(circleSkillCheckObject, transform.position, Quaternion.identity);
            skillCheck.gameObject.GetComponent<SkillCheck>().interactiveObject = this;
        }else {
             skillCheck = Instantiate(barSkillCheckObject, transform.position, Quaternion.identity);
            skillCheck.gameObject.GetComponent<BarCheck>().interactiveObject = this;
        }
    }

    public void OpenDoor() {
        
    }
    public void InteractWithQuest() {
    }
    public void CheckValidations()
    {
        if(validations.Count == skillCheckCount) {
            if(validations.Contains(false)) {
                validations.Clear();
            }else {
                switch (objectRelatedTo) {
                    case objectRelated.Quest:
                        break;
                }
                validations.Clear();
            }
            skillCheckCount = Random.Range(1, 5);
        }else if (validations.Count < skillCheckCount) {
            StartSkilCheck();
        }else {
            validations.Clear();
            skillCheckCount = Random.Range(1, 5);
        }
    }
    #endregion
    #region NotesRegion
    public void InstantiateNote()
    {
        if (!Camera.main.GetComponent<PlayerAim>().isLookANote)
        {
            Vector3 pos = new Vector3(1000,1000,1000);
            Camera camera =  Instantiate(noteCamera, pos, Quaternion.identity);
            GameObject note = Instantiate(noteInstance, pos + new Vector3(-0.15f,-0.13f,0.5f), Quaternion.identity);
            GameObject noteCanvas = Instantiate(noteObject, transform.position, quaternion.identity);
            noteCanvas.transform.GetChild(0).GetComponent<RawImage>().texture = noteTexture;
            camera.targetTexture = noteTexture;
            Camera.main.GetComponent<PlayerAim>().isLookANote = true;
            Camera.main.GetComponent<PlayerAim>().atualUiNote = noteCanvas;
            //set al objects parent to noteCanvas
            note.transform.SetParent(noteCanvas.transform);
            camera.transform.SetParent(noteCanvas.transform);
            note.transform.Find("Arm").transform.SetParent(noteCanvas.transform);

        }else {
            Destroy(Camera.main.GetComponent<PlayerAim>().atualUiNote);
            Camera.main.GetComponent<PlayerAim>().isLookANote = false;
        }
    }
    #endregion
}
#if UNITY_EDITOR    
[CustomEditor(typeof(InteractiveObject))]
public class InteractiveObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InteractiveObject myTarget = (InteractiveObject)target;

        myTarget.objectType = (InteractiveObject.ObjectPossibilities)EditorGUILayout.EnumPopup("Object Type", myTarget.objectType);

        EditorGUILayout.Space();

        switch (myTarget.objectType)
        {
            case InteractiveObject.ObjectPossibilities.SkillCheck:
                myTarget.skillType = (InteractiveObject.SkillType)EditorGUILayout.EnumPopup("Skill Type", myTarget.skillType);
                switch (myTarget.skillType) {
                    case InteractiveObject.SkillType.CircleSkillCheck:
                        myTarget.circleSkillCheckObject = (GameObject)EditorGUILayout.ObjectField("Circle Skill Check Object", myTarget.circleSkillCheckObject, typeof(GameObject), true);
                        break;
                    case InteractiveObject.SkillType.BarSkillCheck:
                        myTarget.barSkillCheckObject = (GameObject)EditorGUILayout.ObjectField("Bar Skill Check Object", myTarget.barSkillCheckObject, typeof(GameObject), true);
                        break;
                }
                EditorGUILayout.Space();
                myTarget.objectRelatedTo = (InteractiveObject.objectRelated)EditorGUILayout.EnumPopup("Object Related To", myTarget.objectRelatedTo);
                if (myTarget.objectRelatedTo == InteractiveObject.objectRelated.Quest) {
                    myTarget.questRelataed = EditorGUILayout.TextField("Quest Related", myTarget.questRelataed);
                    myTarget.phaseRelated = EditorGUILayout.TextField("Phase Related", myTarget.phaseRelated);
                    myTarget.QuestCanvas = (GameObject)EditorGUILayout.ObjectField("ActiveQuest", myTarget.QuestCanvas, typeof(GameObject), true);
                }
                 
                break;
            case InteractiveObject.ObjectPossibilities.Notes:
                myTarget.noteText = EditorGUILayout.TextField("Note Text", myTarget.noteText);
                myTarget.noteTexture = (RenderTexture)EditorGUILayout.ObjectField("Note Texture", myTarget.noteTexture, typeof(RenderTexture), true);
                myTarget.noteCamera = (Camera)EditorGUILayout.ObjectField("Note Camera", myTarget.noteCamera, typeof(Camera), true);
                myTarget.noteObject = (GameObject)EditorGUILayout.ObjectField("Note Object", myTarget.noteObject, typeof(GameObject), true);
                myTarget.noteInstance = (GameObject)EditorGUILayout.ObjectField("Note Instance", myTarget.noteInstance, typeof(GameObject), true);
                break;
            case InteractiveObject.ObjectPossibilities.OnlyInteract:
                myTarget.objectRelatedTo = (InteractiveObject.objectRelated)EditorGUILayout.EnumPopup("Object Related To", myTarget.objectRelatedTo);
                if (myTarget.objectRelatedTo == InteractiveObject.objectRelated.Quest) {
                    myTarget.questRelataed = EditorGUILayout.TextField("Quest Related", myTarget.questRelataed);
                    myTarget.phaseRelated = EditorGUILayout.TextField("Phase Related", myTarget.phaseRelated);
                    myTarget.QuestCanvas = (GameObject)EditorGUILayout.ObjectField("ActiveQuest", myTarget.QuestCanvas, typeof(GameObject), true);
                }
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
