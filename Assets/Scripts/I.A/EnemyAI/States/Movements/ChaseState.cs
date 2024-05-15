using UnityEngine;

public class ChaseState : IState {
   public Enemy enemy;
   public ChaseState(Enemy enemy) {
      this.enemy = enemy;
   }
   public void Enter() {
      enemy.currentState = Enemy.EnemyState.Chase;
      enemy.agent.SetDestination(enemy.target.transform.position);
      
      if(enemy.target == null){
         enemy.ChangeState(new PatrolState(enemy));
      }
      switch (enemy.myType) {
         case Enemy.EnemyType.ranged:
            if (enemy.TargetDistance() <= 10) {
               enemy.ChangeState(new RangedAttackState(enemy));
               enemy.currentState = Enemy.EnemyState.Attack;
            }else {
               //enemy.ChangeState(new TooCloseToAttackState(enemy));
            }
            break;
         case Enemy.EnemyType.melee:
            if(enemy.currentState != Enemy.EnemyState.Attack)
               if (!enemy.isAtacking) {
                  enemy.currentState = Enemy.EnemyState.Attack;
                  if (enemy.TargetDistance() <= 2) {
                     enemy.ChangeState(new MeleeAttackState(enemy));
                  }else {
                     int random = Random.Range(0, 100);
                     if (random <= 70) {
                        enemy.ChangeState(new JumpAttackState(enemy));
                     }else {
                        //if(enemy.currentState != Enemy.EnemyState.Chase)
                        // enemy.ChangeState(new ChaseState(enemy));
                     }
                  }
               }
            break;
      }
   }
   public void Update() { }
   public void Exit(){ }
}
