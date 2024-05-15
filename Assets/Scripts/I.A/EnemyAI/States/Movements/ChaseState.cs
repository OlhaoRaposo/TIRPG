using UnityEngine;

public class ChaseState : IState {
   public Enemy enemy;
   public ChaseState(Enemy enemy) {
      this.enemy = enemy;
   }
   public void Enter() {
      enemy.currentState = Enemy.EnemyState.Chase;
      enemy.agent.speed = 10;
      if(enemy.target == null){
         enemy.ChangeState(new PatrolState(enemy));
      }
   }
   public void Update() {
      enemy.agent.SetDestination(enemy.target.transform.position);
      if (enemy.TargetDistance() < 2){
         switch (enemy.myType) {
            case Enemy.EnemyType.ranged:
               enemy.ChangeState(new TooCloseToAttackState(enemy));
               break;
            case Enemy.EnemyType.melee:
               enemy.ChangeState(new MeleeAttackState(enemy));
               break;
            case Enemy.EnemyType.rangedAndMelee:
               enemy.ChangeState(new MeleeAttackState(enemy));
               //OrRun
               break;
         }
      }else if(enemy.TargetDistance() > 10 && enemy.TargetDistance() < 15) {
         switch (enemy.myType) {
            case Enemy.EnemyType.ranged:
               enemy.ChangeState(new RangedAttackState(enemy));
               break;
            case Enemy.EnemyType.melee:
               int rnd = Random.Range(0, 100);
               if(rnd <= 20)
                  enemy.ChangeState(new JumpAttackState(enemy));
               break;
            case Enemy.EnemyType.rangedAndMelee:
               int rnd2 = Random.Range(0, 100);
               if(rnd2 <= 20)
                  enemy.ChangeState(new JumpAttackState(enemy));
               else
                  enemy.ChangeState(new RangedAttackState(enemy));
               break;
         }
      }
   }
   public void Exit(){ }
}
