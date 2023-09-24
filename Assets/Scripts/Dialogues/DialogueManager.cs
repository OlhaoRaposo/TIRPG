using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Variables")]
    [SerializeField] private DialogueCharacterBase[] allCharacters;
    [SerializeField] private float dialogueSpeed;
    public bool isPlayingDialogue = false;

    private bool isBuildingDialogueText = false;

    [Header("References")]
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private Image portrait;
    [SerializeField] private Text characterName;
    [SerializeField] private Text dialogueText;

    private DialogueCharacterBase activeCharacter;
    private int dialogueIndex, inDialogueIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        AdvanceDialogue();
    }

    public void StartDialogue(string characterName, int dialogueIndex)
    {
        if (isPlayingDialogue == false)
        {
            activeCharacter = null;

            foreach (DialogueCharacterBase character in allCharacters)
            {
                if (character.characterName == characterName)
                {
                    activeCharacter = character;
                    break;
                }
            }

            if (activeCharacter == null)
            {
                Debug.Log($"Não há personagens com o nome de {characterName}.");
                return;
            }
            if (dialogueIndex > activeCharacter.dialogues.GetLength(0) - 1)
            {
                Debug.Log($"Não há nehum diálogo ocupando o espaço de valor {dialogueIndex}.");
                return;
            }

            this.dialogueIndex = dialogueIndex;
            this.characterName.text = characterName;
            isPlayingDialogue = true;
            dialogueWindow.SetActive(true);

            StartCoroutine(DialogueProcess());
        }
        else
        {
            Debug.Log("Está tocando um diálogo atualmente.");
        }
    }

    public void AdvanceDialogue() //Pegar input do sistema
    {
        if (isPlayingDialogue == true && (Input.GetKeyDown(KeyCode.Return) == true || Input.GetMouseButtonDown(0)))
        {
            if (isBuildingDialogueText == true)
            {
                dialogueText.text = activeCharacter.dialogues[dialogueIndex].DialogueTexts[inDialogueIndex];
            }
            else
            {
                if (inDialogueIndex < activeCharacter.dialogues[dialogueIndex].DialogueTexts.GetLength(0))
                {
                    StartCoroutine(DialogueProcess());
                }
                else
                {
                    StopDialogue();
                }
            }
        }
    }

    public void SkipDialogue()
    {
        StopDialogue();
    }

    private void StopDialogue()
    {
        StopAllCoroutines();
        inDialogueIndex = 0;
        characterName.text = "";
        isPlayingDialogue = false;
        isBuildingDialogueText = false;
        dialogueWindow.SetActive(false);
        activeCharacter = null;
        QuestController.instance.ChangeDialog(QuestController.instance.activeMissions[0], QuestController.instance.activeMissions[0].indexDialogue);

    }

    private IEnumerator DialogueProcess()
    {
        char[] currentDialogue = activeCharacter.dialogues[dialogueIndex].DialogueTexts[inDialogueIndex].ToCharArray();
        dialogueText.text = "";
        portrait.sprite = activeCharacter.charcterPortraits[activeCharacter.dialogues[dialogueIndex].portraitIndex[inDialogueIndex]];

        isBuildingDialogueText = true;
        for (int i = 0; dialogueText.text != activeCharacter.dialogues[dialogueIndex].DialogueTexts[inDialogueIndex]; i++)
        {
            dialogueText.text += currentDialogue[i];
            //Tocar som de beep a cada letra.
            yield return new WaitForSeconds(1 / (dialogueSpeed * 10));
        }
        isBuildingDialogueText = false;

        inDialogueIndex++;
    }
}
