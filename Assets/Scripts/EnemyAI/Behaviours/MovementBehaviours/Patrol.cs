using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    private EnemyBehaviour enemy;
    public Patrol(EnemyBehaviour enemy) {this.enemy = enemy;}
    private NavMeshHit hit;
    Vector3 distance;
    public void Enter()
    {
        Debug.Log(" Enter Patroll");
        
        distance = enemy.transform.position - enemy.patrolDestination;
        if (enemy.isPatroling) {
            if (distance.magnitude < 3 || enemy.patrolDestination == new Vector3()) {
                enemy.patrolDestination = Random.insideUnitSphere * (enemy.enemyData.rangeDetection * 4);
                enemy.patrolDestination.y += 150;
                if (Physics.Raycast(enemy.patrolDestination, Vector3.down, out RaycastHit hitInfo)) {
                    enemy.patrolDestination = hitInfo.point;
                    if (NavMesh.FindClosestEdge(enemy.patrolDestination, out hit, 1))
                    {
                        enemy.patrolDestination = hit.position;
                        enemy.patrolDestination.y += .5f;
                        enemy.enemyAgent.SetDestination(enemy.patrolDestination);
                        enemy.point1.transform.position = enemy.patrolDestination;
                    }
                }
            }
        }
        Debug.LogWarning(distance.magnitude);
    }
   public void Update() {}
   public void Exit() { }
}
