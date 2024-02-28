using UnityEngine;
using UnityEngine.Events;

public class InteractDetection : MonoBehaviour
{
    
    [SerializeField]
    private UnityEvent onInteracted = new UnityEvent();
    [SerializeField]
    public bool destroyOnTrigger = false;
    public void OnInteract()
    {
        onInteracted.Invoke();
        if (destroyOnTrigger) Destroy(this);
    }
    public void SetNpc(UnityAction actionEnter)
    {
        onInteracted.AddListener(actionEnter);
    }
}
