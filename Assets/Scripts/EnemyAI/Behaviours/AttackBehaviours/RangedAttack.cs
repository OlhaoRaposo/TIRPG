using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : IState
{
    private EnemyBehaviour enemy;
    public RangedAttack(EnemyBehaviour enemy) {this.enemy = enemy;}
    public void Enter()
    {
        enemy.myWeapon.gun.SendMessage("RangedAttack");
    }
   public void Update() {}
   public void Exit() {}
}
