using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
 public ScriptableObject bestiary;
 public string prefabCode;

  public void StartRespawnProcess()
  {
   StartCoroutine(Respawn(Random.Range(2,6)));
   WorldController.worldController.spawnWasCreated = true;
  }
  private GameObject SearchEntityOnBestiary(string code)
  {
   foreach(GameObject enemy in ((EnemyDataBase)bestiary).enemys)
   {
    if(enemy.GetComponent<EnemyScript>().bestiaryCode == code)
    {
     return enemy;
    }
   }
   return null;
  }

  IEnumerator Respawn(float time)
  {
   yield return new WaitForSeconds(time);
   Instantiate(SearchEntityOnBestiary(prefabCode), transform.position, Quaternion.identity);
   Destroy(this.gameObject);
  }
}
