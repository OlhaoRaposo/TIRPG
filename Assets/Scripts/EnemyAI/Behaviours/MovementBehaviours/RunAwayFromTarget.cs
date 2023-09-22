using UnityEngine;

public class RunAwayFromTarget : IState
{ 
    private EnemyBehaviour enemy;
    public RunAwayFromTarget(EnemyBehaviour enemy) {this.enemy = enemy;}
    
    Vector3 destination = new Vector3();
    Vector3 runDir= new Vector3();
    float runDistance;
    public void Enter()
    { 
        Debug.Log("Run from targer");
        runDir = enemy.transform.position = enemy.target.transform.position;
        runDistance = Random.Range(12, 30);
        runDir = runDir.normalized * runDistance + (Random.insideUnitSphere * 2);
    
        destination = runDir;

        enemy.enemyAgent.SetDestination(destination);
    } 
    public void Update() { }
    public void Exit() { }
}
