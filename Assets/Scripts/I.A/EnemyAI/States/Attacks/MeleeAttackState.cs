using UnityEngine;
public class MeleeAttackState : IState {
    public EnemyBehaviour enemy;
    public MeleeAttackState(EnemyBehaviour enemy) {
        this.enemy = enemy;
    }
    public void Enter() {
        enemy.weapon.SendMessage("Attack",SelectAttack());
    }
    string SelectAttack() { 
        string attack = "";
        attack = enemy.triggers.meleeAttacks[Random.Range(0, enemy.triggers.meleeAttacks.Count)];
        return attack;
    }
   public void Update() { }
    public void Exit() { }
}
