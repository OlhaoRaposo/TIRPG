
public class AttackState : IState
{
    public EnemyBehaviour enemy;
    public AttackState(EnemyBehaviour enemy) {
        this.enemy = enemy;
    }
    public void Enter() {
        enemy.weapon.SendMessage("Attack", enemy.triggerCode);
    }
    public void Update()
    {
    }
    public void Exit(){ }

}
