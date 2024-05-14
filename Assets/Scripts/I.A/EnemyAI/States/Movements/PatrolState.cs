using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState {
   public Enemy enemy;
   private Animator enemyAnimator;

   public PatrolState(Enemy enemy) {
      this.enemy = enemy;
   }
   public void Enter() {
      Debug.Log("Entered" + enemy.currentState);
      enemyAnimator = enemy.enemyAnimator;
      if(enemy != null && enemy.currentState != Enemy.EnemyState.Patrol){
         enemy.currentState = Enemy.EnemyState.Patrol;
      }
      enemy.agent.SetDestination(SelectRandomLocation(enemy.transform.position));
   }
   private Vector3 SelectRandomLocation(Vector3 reference) {
      Vector3 randomLocation;
      NavMeshHit hit;
      randomLocation = reference + Random.insideUnitSphere * 30 + new Vector3(0, 300, 0);
      if (Physics.Raycast(randomLocation, Vector3.down, out RaycastHit hitInfo)) {
         if (NavMesh.SamplePosition(hitInfo.point, out hit, .5f, NavMesh.AllAreas)) {
            randomLocation = hit.position + new Vector3( 0,0.5f,0);
         }
      }
      return randomLocation;
   }
   
   public void Update() {
      CheckRemainingDistance();
      if(enemy.TargetDistance() <= 10){
         enemy.ChangeState(new ChaseState(enemy));
      }
   }
   
   private void CheckRemainingDistance() {
      if (enemy.agent.remainingDistance <= 0.5f) {
         enemy.ChangeState(new PatrolState(enemy));
      }
   }
   
   public void Exit(){ }
}
