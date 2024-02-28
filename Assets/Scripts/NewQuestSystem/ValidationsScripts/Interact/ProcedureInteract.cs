using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class ProcedureInteract : StepModule
{
    public GameObject npc;
    
    [SerializeField]
    private bool destroyOnTrigger = true;
    
    [SerializeField] protected internal UnityEvent onTriggerEnter = new UnityEvent();
    [SerializeField] private protected UnityEvent onTriggerExit = new UnityEvent();
    
    private bool invoked = false;
    private bool async;

    public override void SetActive(bool async)
    {
        onTriggerEnter.AddListener(Enter);
        onTriggerExit.AddListener(Exit);
        npc.AddComponent<InteractDetection>();
        npc.GetComponent<InteractDetection>().SetNpc(onTriggerEnter.Invoke);
        npc.GetComponent<InteractDetection>().destroyOnTrigger = destroyOnTrigger;
        this.async = this.async;
    }
    public override void SetInactive()
    {
        onTriggerEnter.RemoveListener(Enter);
        onTriggerExit.RemoveListener(Exit);
    }
    
   
   
    public void Enter()
    {
        if (invoked) return;
        invoked = !async;
    }

    public void Exit()
    {
        
    }
}
