using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ProcedureKillMobs : StepModule
{
    public string mobCode;
    public int amountToKill;
    public int currentAmountKilled;
    
   [SerializeField] private List<GameObject> enemys;
   [SerializeField] protected internal UnityEvent onKill = new UnityEvent();
   [SerializeField] private protected UnityEvent onComplete = new UnityEvent();

   private bool invoked = false;
   private bool async;
    public override void SetActive(bool async) {
       enemys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
       onKill.AddListener(AddKill);
       onComplete.AddListener(OnCompleted);
       this.async = async;
       foreach (var enemy in enemys) {
           enemy.AddComponent<KillDetection>();
           enemy.GetComponent<KillDetection>().SetKillDetection(onKill.Invoke);
          
       }
    }
    public override void SetInactive()
    {
        foreach (var enemy in enemys) {
            if (enemy.TryGetComponent(out KillDetection kd)) {
                kd.DestroyKillDetection();
            }
                
        }
    }
    public void AddKill()
    {
        currentAmountKilled++;
        if (currentAmountKilled >= amountToKill) {
            onComplete.Invoke();
        }
    }
    public void OnCompleted() {
        if(invoked) return;
        invoked = !async;
    }
}
