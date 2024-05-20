using UnityEngine;

public class ChaseState : IState {
   public EnemyBehaviour enemy;
   public ChaseState(EnemyBehaviour enemy) {
      this.enemy = enemy;
   }
   public void Enter() {
      enemy.currentState = EnemyBehaviour.EnemyState.Chase;
      enemy.agent.speed = 10;
      if(enemy.target == null){
         enemy.ChangeState(new PatrolState(enemy));
      }
   }
   public void Update() {
      enemy.agent.SetDestination(enemy.target.transform.position);
      if (enemy.TargetDistance() < 5){
         switch (enemy.myType) {
            case EnemyBehaviour.EnemyType.ranged:
               enemy.ChangeState(new TooCloseToAttackState(enemy));
               break;
            case EnemyBehaviour.EnemyType.melee:
               enemy.ChangeState(new MeleeAttackState(enemy));
               break;
            case EnemyBehaviour.EnemyType.rangedAndMelee:
               int rnd2 = Random.Range(0, 100);
               if(rnd2 <= 50)
                  enemy.ChangeState(new MeleeAttackState(enemy));
               else
                  enemy.ChangeState(new RangedAttackState(enemy));
               break;
         }
      }else if(enemy.TargetDistance() > 6 && enemy.TargetDistance() < 10) {
         switch (enemy.myType) {
            case EnemyBehaviour.EnemyType.ranged:
               enemy.ChangeState(new RangedAttackState(enemy));
               break;
            case EnemyBehaviour.EnemyType.melee:
               int rnd = Random.Range(0, 100);
               if(rnd <= 60)
                  enemy.ChangeState(new JumpAttackState(enemy));
               break;
            case EnemyBehaviour.EnemyType.rangedAndMelee:
               int rnd2 = Random.Range(0, 100);
               if(rnd2 <= 30)
                  enemy.ChangeState(new JumpAttackState(enemy));
               else
                  enemy.ChangeState(new RangedAttackState(enemy));
               break;
         }
      }else if (enemy.TargetDistance() > 10 && enemy.TargetDistance() < 20) {
         int rnd2 = Random.Range(0, 100);
         if(rnd2 <= 20)
            enemy.ChangeState(new JumpAttackState(enemy));
         else 
            enemy.ChangeState(new ChaseState(enemy));
      }
   }
   public void Exit(){ }
}
