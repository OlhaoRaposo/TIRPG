using UnityEngine;

[CreateAssetMenu(fileName = "Character profile", menuName = "Character/Character profile")]
public class DialogueCharacterBase : ScriptableObject
{
    public string characterName;
    public Sprite[] charcterPortraits;
    public DialogueBase[] dialogues;
}
