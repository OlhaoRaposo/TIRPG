using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWritter : MonoBehaviour
{
  public static TypeWritter instance;
  public Coroutine writter;
  

  public TextMeshProUGUI textToWrite;
  public string text;
  public bool attNpc;
  public NPC npc;
  
  [SerializeField]private float talkingSpeed = .4f;
  [SerializeField]private bool isWriting = false;

  private void Start() {
    instance = this;
    writter = StartCoroutine(WriteText());
  }
  public void Write(TextMeshProUGUI textToWrite, string stringToWrite) {
    if (isWriting) {
      talkingSpeed = .01f;
      Debug.Log("IsWriting");
    }
    if(!isWriting) {
      textToWrite.text = "";
      this.textToWrite = textToWrite;
      text = stringToWrite;
      StartCoroutine(WriteText());
    }
  }
  public void AttCurrentIndex(NPC npc) {
    attNpc = true;
    this.npc = npc;
  }
  IEnumerator WriteText() {
    isWriting = true;
    foreach (char letter in text) {
      yield return new WaitForSeconds(talkingSpeed);
      textToWrite.text += letter;
    }
    if (attNpc) {
      npc.currentDialogueIndex++;
      npc = null;
      attNpc = false;
    }
    isWriting = false;
    talkingSpeed = .05f;
  }
}
