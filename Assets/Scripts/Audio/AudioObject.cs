using System.Collections;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
   [SerializeField]
   private SphereCollider audioCollObject;
   [SerializeField] 
   private AudioInfo thisInfo;
   void Start()
   {
      //Inicia o audio assim q o objeto for criado
      PlayAudio(thisInfo.name,thisInfo.maxRange);
   }
   void PlayAudio(string name, float maxRadius)
   {
      //Toca o audio
      AudioBoard.instance.PlayAudio(name);
      
      //So cria a propagação do audio se houver um objeto para progpagar
      if (transform.childCount != 0)
         return;
      //Cria o objeto de propagação
      InstantiateChildObject();
      //Inicia a propagação do som
      StartCoroutine(SoundPropagation(maxRadius));
   }
   void InstantiateChildObject()
   {
      GameObject childObject = new GameObject();
      childObject.name = "TemporaryAudioTrigger";
      childObject.tag = "AudioWarning";
      childObject.transform.position = this.transform.position;
      childObject.transform.parent = this.transform;
      childObject.gameObject.layer = thisInfo.audioLayer;
      childObject.AddComponent<SphereCollider>();
      childObject.GetComponent<SphereCollider>().isTrigger = true;
      childObject.GetComponent<SphereCollider>().radius = .3f;
      audioCollObject = childObject.GetComponent<SphereCollider>();
   }
   IEnumerator SoundPropagation(float maxRadius)
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
[System.Serializable]
 class AudioInfo
 {
    [SerializeField]
    public string name;
    [SerializeField]
    public float maxRange;
    [SerializeField]
    public LayerMask audioLayer;
 }

