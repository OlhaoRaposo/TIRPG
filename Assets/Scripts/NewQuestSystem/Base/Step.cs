using UnityEngine;
[System.Serializable]
public class Step
{
    public bool isComplete;
    public bool isActive;
    public string stepSubtitle;
    [Header("Steps")]
    public QuestStepModule validation;
    

    public void SetActive()
    {
        Debug.Log("Active");
        validation.SetActive(true);
        isActive = true;
    }
    public void SetInactive()
    {
        validation.SetInactive();
     
    }
   
}
