using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Character/Character Dialogue")]
public class DialogueBase : ScriptableObject
{
    [Header("Dialogue Variables")]
    [TextArea(5, 5)] public string[] DialogueTexts;
    public int[] portraitIndex;
}
