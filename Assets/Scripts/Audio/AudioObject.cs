using System.Collections;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
   [SerializeField] 
   private AudioInfo thisInfo;
   void Start()
   {
      //Inicia o audio assim q o objeto for criado
      PlayAudio(thisInfo.name);
   }
   void PlayAudio(string name)
   {
      //Toca o audio
      AudioBoard.instance.PlayAudio(name);
   }
}
[System.Serializable]
 class AudioInfo
 {
    [SerializeField]
    public string name;
 }

