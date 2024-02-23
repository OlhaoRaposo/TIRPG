using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class ProcedureTrigger : StepModule
{
    [SerializeField]
    private protected GameObject _colliderObject = null;
    [SerializeField]
    private GameObject _triggerObject = null;
    
    [Space]
    [SerializeField]
    private bool destroyOnTrigger = true;
    [SerializeField] protected internal UnityEvent onTriggerEnter = new UnityEvent();
    [SerializeField]
    private protected UnityEvent onTriggerExit = new UnityEvent();
    
    private bool invoked = false;
    private bool async;
    
    public override void SetActive(bool async)
    {
        onTriggerEnter.AddListener(Enter);
        onTriggerExit.AddListener(Exit);
        
        
        this.async = async;
    }
    
    public override void SetInactive()
    {
        onTriggerEnter.RemoveListener(Enter);
        onTriggerExit.RemoveListener(Exit);
        
    }

    private void Enter()
    {
        if (invoked) return;
        invoked = !async;
    }
    
    private void Exit()
    {
    }
}