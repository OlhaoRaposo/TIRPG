using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Quest
{
    [Header("Info")]
    public string code;
    public bool isComplete;
    [Header("Accessibility")]
    public List<Dialogue> dialogue;
    public List<Dialogue> questAlreadyGiven;
    [Header("Steps")]
    [Space()]
    public List<bool> validationCount;
    public QuestStepModule step;
    public string Name {set => code = value; get => code;}
    

    public void SetActive()
    {
        step.SetActive(true);
    }
    public void SetInactive()
    {
        step.SetInactive();
     
    }
   
}
