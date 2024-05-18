using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ProcedureTrigger : StepModule
{
    [SerializeField]
    private protected GameObject _colliderObject = null;
    [SerializeField]
    private GameObject player = null;
    
    [Space]
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
        _colliderObject.AddComponent<TriggerCollisionDetector>().SetTrigger(PlayerController.instance.GetComponent<Collider>(), !async && destroyOnTrigger, onTriggerEnter.Invoke, onTriggerExit.Invoke);
        _colliderObject.GetComponent<TriggerCollisionDetector>().destroyOnTrigger = destroyOnTrigger;
        this.async = async;
    }
    public override void SetInactive()
    {
        onTriggerEnter.RemoveListener(Enter);
        onTriggerExit.RemoveListener(Exit);
        
        if(_colliderObject.TryGetComponent(out TriggerCollisionDetector tcd)) tcd.DestroyTrigger();
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