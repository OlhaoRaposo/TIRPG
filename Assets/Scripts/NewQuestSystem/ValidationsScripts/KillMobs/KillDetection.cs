using UnityEngine;
using UnityEngine.Events;

public class KillDetection : MonoBehaviour
{
    [SerializeField] public string mobCode;
    [SerializeField] private UnityEvent onKill = new UnityEvent();
    [SerializeField] private UnityEvent onComplete = new UnityEvent();
    
    public void SetKillDetection(UnityAction actionKill)
    {
        onKill.AddListener(actionKill);
    }
    
    public void DestroyKillDetection()
    {
        Destroy(this);
    }
}
