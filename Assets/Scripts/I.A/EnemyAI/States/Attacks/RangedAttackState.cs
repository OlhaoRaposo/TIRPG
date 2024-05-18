using UnityEngine;

public class RangedAttackState : IState {
    public EnemyBehaviour enemy;
    public RangedAttackState(EnemyBehaviour enemy) {
        this.enemy = enemy;
    }
    public void Enter() {
        enemy.weapon.SendMessage("Attack",SelectAttack());
    }
    private void Aim() {
        Vector3 fowardPos = enemy.target.transform.position + (enemy.target.transform.forward * 2);
        enemy.transform.LookAt(fowardPos);
    }
    string SelectAttack() { 
        string attack = "";
        attack = enemy.triggers.rangedAttacks[Random.Range(0, enemy.triggers.rangedAttacks.Count)];
        return attack;
    }
    public void Update() {
    }
    public void Exit() {
    }
}