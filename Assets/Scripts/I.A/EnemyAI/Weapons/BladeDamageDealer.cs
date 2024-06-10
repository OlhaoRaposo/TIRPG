using UnityEngine;

public class BladeDamageDealer : MonoBehaviour
{
   public float damage;
   private void OnTriggerEnter(Collider other) {
      
      if(other.gameObject.TryGetComponent(out PlayerHPController player)){
         player.ChangeHP(damage,true);
         Debug.Log("HIT");
      }
   }
}
