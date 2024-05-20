using UnityEngine;

public class JumpAttackState : IState {
    public EnemyBehaviour enemy;
    public JumpAttackState (EnemyBehaviour enemy) {
        this.enemy = enemy;
    }
    public void Enter() {
        enemy.transform.LookAt(jumpPosition());
        enemy.weapon.SendMessage("Attack",SelectAttack());
    }
    
    string SelectAttack() { 
        string attack = "";
        attack = enemy.triggers.jumpAttacks[Random.Range(0, enemy.triggers.jumpAttacks.Count)];
        return attack;
    }
    private Vector3 jumpPosition() {
        Vector3 pos = new Vector3();
        pos = enemy.target.transform.position;
        pos += Random.insideUnitSphere * 1.5f;
        pos.y = enemy.target.transform.position.y;
        return pos;
    }
    public void Update() { }
    public void Exit(){ }
}
