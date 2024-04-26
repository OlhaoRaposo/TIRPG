using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ProcedureKillMobs : StepModule
{
    public string mobCode;
    public int amountToKill;
    public int currentAmountKilled;
    private bool destroyOnKill = true;
    public string questCode;
    
   [SerializeField] private List<GameObject> enemys;
   [SerializeField] protected internal UnityEvent onKill = new UnityEvent();
   [SerializeField] private protected UnityEvent onComplete = new UnityEvent();

   private bool invoked = false;
   private bool async;
    public override void SetActive(bool async) {
       enemys = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
       onKill.AddListener(AddKill);
       this.async = async;
       foreach (var enemy in enemys) {
           if(enemy.TryGetComponent(out EnemyBehaviour eb)) {
               if(eb.mySpawner.bestiaryCode == mobCode) {
                  KillDetection enDetection = enemy.AddComponent<KillDetection>();
                   enDetection.SetInvokes(onKill.Invoke,destroyOnKill);
               }
           }
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
            Debug.LogWarning("KILL COMPLETE");
            OnCompleted();
        }else
            Debug.LogWarning("KILL ADDED");
    }
    public void OnCompleted() {
        QuestManager.instance.AddValidation(questCode);
        if(invoked) return;
        invoked = !async;
    }
}
