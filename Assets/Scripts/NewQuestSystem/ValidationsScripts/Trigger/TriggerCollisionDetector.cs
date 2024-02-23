using UnityEngine;
using UnityEngine.Events;

public class TriggerCollisionDetector : MonoBehaviour
{
    [SerializeField]
    private Collider objectToDetect = null;
    [SerializeField]
    private UnityEvent onTriggerEnter = new UnityEvent();
    [SerializeField]
    private UnityEvent onTriggerExit = new UnityEvent();
    [SerializeField]
    private bool destroyOnTrigger = false;
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("tRIGGERED" + col.name + " " + objectToDetect.name);
        if (col.gameObject == objectToDetect.gameObject)
        {
            Debug.Log("EnteredDetector");
            onTriggerEnter.Invoke();
            if (destroyOnTrigger) Destroy(this);
        }
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (col == objectToDetect)
        {
            onTriggerExit.Invoke();
            if (destroyOnTrigger) Destroy(this);
        }
    }
    
    public void DestroyTrigger()
    {
        Destroy(this);
    }
    
    public void SetTrigger (Collider col, bool destroy, UnityAction actionEnter, UnityAction actionExit)
    {
        objectToDetect = col;
        destroyOnTrigger = destroy;
        onTriggerEnter.AddListener(actionEnter);
        onTriggerExit.AddListener(actionExit);
    }
}