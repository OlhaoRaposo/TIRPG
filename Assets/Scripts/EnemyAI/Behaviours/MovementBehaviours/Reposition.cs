using UnityEngine;
using UnityEngine.AI;

public class Reposition : IState
{
   private EnemyBehaviour enemy;
   public Reposition(EnemyBehaviour enemy) {this.enemy = enemy;}
   private Vector3 enemyVision;
   Vector3 repositionTransform;
   public void Enter()
   {
      NavMeshHit hit;

      Debug.Log("Reposition");
      repositionTransform = Random.insideUnitSphere * (enemy.enemyData.rangeDetection);
      repositionTransform.y += 300;
         if (Physics.Raycast(repositionTransform, Vector3.down, out RaycastHit hitInfo)) {
            repositionTransform = hitInfo.point;
            if (NavMesh.FindClosestEdge(repositionTransform, out hit, NavMesh.AllAreas))
            { 
               repositionTransform = hit.position;
               repositionTransform.y += .5f;
               enemyVision = enemy.target.transform.position - repositionTransform;
               enemy.enemyAgent.SetDestination(repositionTransform);
            }else {
               enemy.SetState(new Reposition(enemy));
            }
         }else {
            enemy.SetState(new Reposition(enemy));
         }
   }

   public void Update()
   {
      Debug.DrawRay(repositionTransform,enemyVision * 10,Color.green);
      
   }
   public void Exit() { }
}
