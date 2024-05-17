using UnityEngine;
using UnityEngine.AI;

public class TooCloseToAttackState : IState {
    public Enemy enemy;
    private float distanceWanted;
    public GameObject target;
    public TooCloseToAttackState(Enemy enemy) {
        this.enemy = enemy;
    }
    public void Enter() {
        Debug.Log("Entered" + enemy.currentState);
        enemy.agent.SetDestination(newPositionFromPlayer());
    }

    private Vector3 newPositionFromPlayer() {
        Vector3 newPosition = new Vector3();
        NavMeshHit hit;
        newPosition = enemy.transform.position - enemy.target.transform.position;
        newPosition = newPosition.normalized;
        newPosition *= distanceWanted;
        newPosition += Random.insideUnitSphere * 3;
        newPosition += new Vector3(0, 15, 0);
        if (Physics.Raycast(newPosition, Vector3.down, out RaycastHit hitInfo)) {
            if (NavMesh.SamplePosition(hitInfo.point, out hit, .5f, NavMesh.AllAreas)) {
                newPosition = hit.position + new Vector3( 0,0.5f,0);
            }
        }
        return newPosition;
    }
    private float CalculateDistance() {
        float distance = 0;
        Vector3 targetDistance = enemy.transform.position - enemy.target.transform.position;
        distance = 1- targetDistance.magnitude;
        return distance;
    }
    public void Update() {
        if (CalculateDistance() >= 10)
            enemy.ChangeState(new ChaseState(enemy));
    }
    public void Exit(){ }
}
