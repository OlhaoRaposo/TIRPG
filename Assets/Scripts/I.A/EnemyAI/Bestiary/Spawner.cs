using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
 public ScriptableObject bestiary;
 public string prefabCode;
 public bool hasQuestOnCourse;
 public UnityEvent onKill = new UnityEvent();

  public void StartRespawnProcess()
  {
   StartCoroutine(Respawn(Random.Range(2,6)));
   WorldController.worldController.spawnWasCreated = true;
  }
  private GameObject SearchEntityOnBestiary(string code)
  {
   foreach(GameObject enemy in ((EnemyDataBase)bestiary).enemys)
   {
    if(enemy.GetComponent<EnemyBehaviour>().mySpawner.bestiaryCode == code)
    {
     return enemy;
    }
   }
   return null;
  }

  IEnumerator Respawn(float time)
  {
   yield return new WaitForSeconds(time);
   GameObject enemy = Instantiate(SearchEntityOnBestiary(prefabCode), transform.position, Quaternion.identity);
   CheckIfHasQuestOnCourse(enemy);
   Destroy(this.gameObject);
  }
  
  void CheckIfHasQuestOnCourse(GameObject enemy)
  {
   if(hasQuestOnCourse) {
    enemy.AddComponent<KillDetection>();
    enemy.GetComponent<KillDetection>().SetInvokes(onKill.Invoke,true);
   }
  }
}
