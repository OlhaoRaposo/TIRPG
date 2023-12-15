using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData",menuName = "Scriptables/Enemy")]
[System.Serializable]
public class EnemyData : ScriptableObject
{ public enum myBehaviour
  {
    ranged,
    melee,
    rangedAndMelee
  }
  [SerializeField]
  public myBehaviour enemyBehaviour;
  [SerializeField] 
  [Range(0, 30)] 
  public float rangeDetection;
  [Range(.5f, 4)] 
  public float reactionTime;
 
}

