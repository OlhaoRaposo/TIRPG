using UnityEngine;
using UnityEngine.Events;

public class KillDetection : MonoBehaviour
{
    [SerializeField] public UnityEvent onKill = new UnityEvent();
    public bool destroyOnKill;
    public void SetInvokes(UnityAction killed, bool destroy) {
        onKill.AddListener(killed);
    }
    public void Die() {
        
        Debug.LogWarning("KILL CALLED");
        if(destroyOnKill) Destroy(this);
    }
    public void DestroyKillDetection()
    {
        Destroy(this);
    }
}
