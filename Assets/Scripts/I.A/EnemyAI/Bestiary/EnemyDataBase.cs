using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "Spawner/Bestiary", order = 1)]
public class EnemyDataBase : ScriptableObject
{
  public GameObject[] enemys;
}
