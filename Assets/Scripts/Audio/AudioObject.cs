using System;
using System.Collections;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
   [SerializeField]
   private SphereCollider audioCollObject;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.G))
      {
         PlayAudio("Bell",30);
      }
   }

   void PlayAudio(string name, float maxRadius)
   {
      AudioBoard.instance.PlayAudio(name);
      if (transform.childCount != 0)
         return;
      InstantiateChildObject();
      StartCoroutine(SoundPropagation(maxRadius));
   }
   void InstantiateChildObject()
   {
      GameObject childObject = new GameObject();
      childObject.name = "TemporaryAudioTrigger";
      childObject.tag = "AudioWarning";
      childObject.transform.position = this.transform.position;
      childObject.transform.parent = this.transform;
      childObject.AddComponent<SphereCollider>();
      childObject.GetComponent<SphereCollider>().isTrigger = true;
      childObject.GetComponent<SphereCollider>().radius = .3f;
      audioCollObject = childObject.GetComponent<SphereCollider>();
   }
    private IEnumerator SoundPropagation(float maxRadius)
    {
       yield return new WaitForSeconds(.015f);
       if (audioCollObject.radius <= maxRadius){
          audioCollObject.radius = Mathf.Lerp(audioCollObject.radius, maxRadius + .3f , 3 * Time.deltaTime);
          StartCoroutine(SoundPropagation(maxRadius));
       }else{
          Destroy(audioCollObject.gameObject);
       }
    }
}

