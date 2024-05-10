public class ChaseState : IState {
   public Enemy enemy;
   public ChaseState(Enemy enemy) {
      this.enemy = enemy;
   }
   public void Enter() {
      enemy.currentState = Enemy.EnemyState.Chase;
      enemy.agent.SetDestination(enemy.target.transform.position);
   }

   public void Update() {
      switch (enemy.myType) {
         case Enemy.EnemyType.ranged:
            if (enemy.TargetDistance() <= 10) {
               enemy.ChangeState(new RangedAttackState(enemy));
            }
            break;
         case Enemy.EnemyType.melee:
            if (enemy.TargetDistance() <= 2) {
               enemy.ChangeState(new MeleeAttackState(enemy));
            }
            break;
      }
     
   }
   public void Exit(){ }
}
