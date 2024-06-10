using UnityEngine;
using UnityEngine.AI;

public class TooCloseToAttackState : IState {
    public EnemyBehaviour enemy;
    private float distanceWanted;
    public GameObject target;
    private float timer;
    public TooCloseToAttackState(EnemyBehaviour enemy) {
        this.enemy = enemy;
    }
    public void Enter() {
        Debug.Log("Entered" + enemy.currentState);
        Vector3 offset = enemy.transform.position - enemy.target.transform.position;
        enemy.agent.SetDestination(enemy.target.transform.position + offset.normalized * 2);
    }
    public void Update() {
      timer += Time.deltaTime;
      if (timer >= 2) {
          enemy.ChangeState(new ChaseState(enemy));
      }
    }
    public void Exit(){ }
}
