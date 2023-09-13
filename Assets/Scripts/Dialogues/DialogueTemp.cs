using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTemp : MonoBehaviour
{
    public string charname;
    public int dialogueIndex;
    public void Play()
    {
        DialogueManager.instance.StartDialogue(charname, dialogueIndex);
    }
}
