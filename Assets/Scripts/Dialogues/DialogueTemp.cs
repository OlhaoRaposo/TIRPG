using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueOnQuest", menuName = "Quests/New Dialogue On Quest")]
public class DialogueTemp : ScriptableObject
{
    public string charname;
    public int dialogueIndex;
    public void Play()
    {
        DialogueManager.instance.StartDialogue(charname, dialogueIndex);
    }
}
