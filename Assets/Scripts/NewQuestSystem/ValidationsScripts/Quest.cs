using UnityEngine;
[System.Serializable]
public class Quest
{
    [Header("Info")]
    [SerializeField] private string code;
    [Header("Accessibility")]
    [SerializeField] private Dialogue dialogue;
    [Header("List")]
    [Space()]
    [SerializeField] public QuestStepModule step;
    
    public string Name {set => code = value; get => code;}
    
    
    public void SetActive()
    {
        step.SetActive(false);
   
    }
    
    public void SetInactive()
    {
        step.SetInactive();
     
    }
}
