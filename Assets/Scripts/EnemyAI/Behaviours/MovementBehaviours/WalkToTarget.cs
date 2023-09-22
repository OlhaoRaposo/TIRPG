using UnityEngine;

public class WalkToTarget : IState
{
    private EnemyBehaviour enemy;
    public WalkToTarget(EnemyBehaviour enemy) { this.enemy = enemy; }
    
    Vector3 destination = new Vector3();
    Vector3 runDir = new Vector3();

    public void Enter() {
        Debug.Log("WalkToTarget");
        float distanceFromPlayer = 1;
        switch (enemy.enemyData.enemyBehaviour)
        {
            case EnemyData.myBehaviour.ranged:
                distanceFromPlayer = 3;
                break;
            case EnemyData.myBehaviour.melee:
                distanceFromPlayer = .1f;
                break;
            case EnemyData.myBehaviour.rangedAndMelee:
                distanceFromPlayer = Random.Range(.1f,3);
                break;
        }
        runDir = enemy.target.transform.position - enemy.transform.position + Random.insideUnitSphere * distanceFromPlayer ;
        destination = runDir + (Random.insideUnitSphere * 3);
        enemy.enemyAgent.SetDestination(destination);
    }
    public void Update(){} 
    public void Exit(){}
    
}
