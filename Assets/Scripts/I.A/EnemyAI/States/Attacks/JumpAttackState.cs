public class JumpAttackState : IState {
    public Enemy enemy;
    public JumpAttackState (Enemy enemy) {
        this.enemy = enemy;
    }
    public void Enter() {
        enemy.transform.LookAt(enemy.target.transform.position);
        enemy.weapon.Attack("_Jump");
    }
    public void Update() {
      
    }
    public void Exit(){ }
}
