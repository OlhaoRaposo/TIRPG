using System.Collections.Generic;
using UnityEngine;

public class RangedAreaAttack : IState
{
    private EnemyBehaviour enemy;
    public RangedAreaAttack(EnemyBehaviour enemy) {this.enemy = enemy;}

    private Vector3[] targets;
  public void Enter()
  {
      for (int i = 0; i < 3; i++)
      {
          enemy.Test[i] = enemy.target.transform.position + Random.insideUnitSphere * 10;
          enemy.Test[i].y += 30;
      }
      enemy.myWeapon.gun.SendMessage("RangedAreaAttack",enemy.Test);
  }
  public void Update()
  {
        
  }
  public void Exit()
  {
        
  }
}
