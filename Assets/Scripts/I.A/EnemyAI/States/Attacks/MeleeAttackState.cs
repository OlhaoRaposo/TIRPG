
public class MeleeAttackState : IState
{
    public Enemy enemy;
    public MeleeAttackState(Enemy enemy) {
        this.enemy = enemy;
    }
    public void Enter()
    {
    }
   public void Update()
    {
    }
    public void Exit()
    {
    }
}
