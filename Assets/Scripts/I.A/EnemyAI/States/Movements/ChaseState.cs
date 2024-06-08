using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ChaseState : IState {
   public EnemyBehaviour enemy;
   public ChaseState(EnemyBehaviour enemy) {
      this.enemy = enemy;
   }

   private float time;
   float interval = 1.5f;
   public void Enter() {
      enemy.attacksAvailable.Clear();
      enemy.currentState = EnemyBehaviour.EnemyState.Chase;
      enemy.agent.speed = 6;
   }
   public void Update()
   {
      Vector3 offset = enemy.transform.position - enemy.target.transform.position;
      enemy.agent.SetDestination(enemy.target.transform.position + offset.normalized * 2);
      
      time += Time.deltaTime;
      if (time <= interval) {
         time = 0;
         interval = Random.Range(1, 3);
      }else
         return;
      
      float distance = enemy.TargetDistance();

      foreach (var atacks in enemy.attacks) {
         if (distance <= atacks.maxRange && distance >= atacks.minRange) {
           enemy.attacksAvailable.Add(atacks.attackCode);
         }
      }

      foreach (var str in enemy.attacksAvailable) {
         Debug.Log(str);
      }
      enemy.triggerCode = ChooseAttack();
      enemy.ChangeState(new AttackState(enemy));
   }
   private string ChooseAttack()
   {
      string chosenAttack = "None";
      if (enemy.attacksAvailable.Count > 0) {
         chosenAttack = enemy.attacksAvailable[Random.Range(0, enemy.attacksAvailable.Count)];
         if(enemy.attacksAvailable.Count > 1)
            chosenAttack = enemy.attacksAvailable[Random.Range(0, enemy.attacksAvailable.Count)];
         else
         if (chosenAttack == enemy.triggerCode)
            chosenAttack = ChooseAttack();
      }else
         enemy.ChangeState(new ChaseState(enemy));
        
      return chosenAttack;
   }
   public void Exit(){ }
}
