using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CustomEditor(typeof(InteractiveObject))]
public class InteractiveObject : MonoBehaviour
{
    public enum ObjectPossibilities {
        SkillCheck,Notes,Radio,Door
    }
    public ObjectPossibilities objectType;
    //Radio Variables
    public string staticAudio;
    public string principalAudio;
    public string aditionalAudio;
    public bool isRadioOn;
    
    //Notes Variables
    [SerializeField]
    public string noteText;
    public RenderTexture noteTexture;
    public Camera noteCamera;
    public GameObject noteInstance;
    public GameObject noteObject;
    //Skill Check Variables
    public enum objectRelated { None, Door, Radio, } public objectRelated objectRelatedTo;
    public bool isSkilCheck;
    public int skillCheckCount;
    public List<bool> validations = new List<bool>();
    public enum SkillType { CircleSkillCheck, BarSkillCheck, } public SkillType skillType;
    public GameObject circleSkillCheckObject, barSkillCheckObject;
    
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
            case ObjectPossibilities.Radio:
                PlayRadio();
                break;
            case ObjectPossibilities.Door:
                break;
        }
    }
    #region SkillCheckRegion
    void StartSkilCheck()
    {
        if(skillCheckCount == 0)
            skillCheckCount = Random.Range(1, 5);

        if (skillType == SkillType.CircleSkillCheck) {
            GameObject skillCheck = Instantiate(circleSkillCheckObject, transform.position, Quaternion.identity);
            skillCheck.gameObject.GetComponent<SkillCheck>().interactiveObject = this;
        }else {
            GameObject skillCheck = Instantiate(barSkillCheckObject, transform.position, Quaternion.identity);
            skillCheck.gameObject.GetComponent<BarCheck>().interactiveObject = this;
        }
    }
    public void CheckValidations()
    {
        if(validations.Count == skillCheckCount) {
            if(validations.Contains(false)) {
                validations.Clear();
                switch (objectRelatedTo)
                {
                    case objectRelated.None:
                        break;
                    case objectRelated.Door:
                        break;
                }
               
            }else {
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
    #region RadioRegion
    public void PlayRadio()
    {
        if(isRadioOn)
            return;
        isRadioOn = true;
        AudioBoard.instance.PlayAudio(aditionalAudio);
        AudioBoard.instance.PlayAudio(principalAudio);
        AudioBoard.instance.PlayAudio(staticAudio);
        foreach (var audio in AudioBoard.instance.audios) { if (audio.name == principalAudio) { StartCoroutine(StopRadio(audio.clip.length)); } }
    }
   private IEnumerator StopRadio(float time)
   {
       yield return new WaitForSeconds(time);
       AudioBoard.instance.StopAudio(aditionalAudio);
       AudioBoard.instance.StopAudio(principalAudio);
       AudioBoard.instance.StopAudio(staticAudio);
       isRadioOn = false;
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
                myTarget.isSkilCheck = EditorGUILayout.Toggle("Is Skill Check", myTarget.isSkilCheck);
                myTarget.skillCheckCount = EditorGUILayout.IntField("Skill Check Count", myTarget.skillCheckCount);
                myTarget.skillType = (InteractiveObject.SkillType)EditorGUILayout.EnumPopup("Skill Type", myTarget.skillType);
                myTarget.circleSkillCheckObject = (GameObject)EditorGUILayout.ObjectField("Circle Skill Check Object", myTarget.circleSkillCheckObject, typeof(GameObject), true);
                myTarget.barSkillCheckObject = (GameObject)EditorGUILayout.ObjectField("Bar Skill Check Object", myTarget.barSkillCheckObject, typeof(GameObject), true);
                break;

            case InteractiveObject.ObjectPossibilities.Notes:
                myTarget.noteText = EditorGUILayout.TextField("Note Text", myTarget.noteText);
                myTarget.noteTexture = (RenderTexture)EditorGUILayout.ObjectField("Note Texture", myTarget.noteTexture, typeof(RenderTexture), true);
                myTarget.noteCamera = (Camera)EditorGUILayout.ObjectField("Note Camera", myTarget.noteCamera, typeof(Camera), true);
                myTarget.noteObject = (GameObject)EditorGUILayout.ObjectField("Note Object", myTarget.noteObject, typeof(GameObject), true);
                myTarget.noteInstance = (GameObject)EditorGUILayout.ObjectField("Note Instance", myTarget.noteInstance, typeof(GameObject), true);
                break;
            case InteractiveObject.ObjectPossibilities.Radio:
                myTarget.staticAudio = EditorGUILayout.TextField("Static Audio", myTarget.staticAudio);
                myTarget.principalAudio = EditorGUILayout.TextField("Principal Audio", myTarget.principalAudio);
                myTarget.aditionalAudio = EditorGUILayout.TextField("Aditional Audio", myTarget.aditionalAudio);
                myTarget.isRadioOn = EditorGUILayout.Toggle("Is Radio On", myTarget.isRadioOn);
                break;
            case InteractiveObject.ObjectPossibilities.Door:
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
