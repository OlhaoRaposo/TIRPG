using UnityEngine;

public class JailLocker : MonoBehaviour
{
   bool isLocked = false;
   [SerializeField] float timeToUnlock = 5f;

   private void Update() {
      if (isLocked) {
         PlayerMovement.instance.gameObject.transform.position = transform.position;
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (!isLocked) {
         if(other.gameObject.CompareTag("Player")) {
            isLocked = true;
         }
      }
   }

   public void Unlock() {
      isLocked = false;
   }
}
