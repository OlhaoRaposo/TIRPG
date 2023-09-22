using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RobotRangedWeapon : MonoBehaviour
{
    public EnemyBehaviour enemy;

    public void RangedAttack()
    {
        Instantiate(enemy.myWeapon.ammoPrefab, transform.position, enemy.transform.rotation);
    }
    public void RangedAreaAttack(Vector3[] targets)
    {
        foreach (var transform in targets)
        {
            StartCoroutine(IRangedAreaAttack(transform));
        }
    }
    IEnumerator IRangedAreaAttack(Vector3 target)
    {
        yield return new WaitForSeconds(Random.Range(.2f, .8f));
        Instantiate(enemy.myWeapon.areaAmmoPrefab, target, quaternion.identity);
        if (Physics.Raycast(target, Vector3.down, out RaycastHit hitInfo))
        {
            Instantiate(enemy.myWeapon.imageTarget, hitInfo.point + new Vector3(0,0.1f,0), Quaternion.Euler(new Vector3(90,0,0)));
        }
    }
}
