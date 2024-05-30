using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

[RequireComponent(typeof(Interactable_Npc))]
public class NPC : MonoBehaviour
{
    public List<Dialogue> dialogues;
    [SerializeField] public int currentDialogueIndex;
    [SerializeField] private bool isTalking;
    [SerializeField] private NPCReferenceData npcReference;
    private void Start() {
        npcReference.talkBox.SetActive(true);
        npcReference.npcText.text = "";
        npcReference.npcNameReference.text = npcReference.npcName;
        npcReference.talkBox.SetActive(false);
    }
    public void Interact() {
        EnableChatBox();
        Talk();
        Debug.Log("Npc is Talking " + gameObject.name);
    }
    public void EnableChatBox() {
        npcReference.talkBox.SetActive(true);
        //TODO
        //Lock Player Movement
        //lock Player Camera
        //lock Player Inputs
        DefaultButtonConfiguration();
    }
    public void DisableChatBox() {
        npcReference.npcText.text = "";
        npcReference.talkBox.SetActive(false);
        //TODO
        //Unock Player Movement
        //Unlock Player Camera
        //Unlock Player Inputs
    }
    private void Talk() {
        TypeWritter.instance.Write(npcReference.npcText,
            dialogues[currentDialogueIndex].sentences[dialogues[currentDialogueIndex].sentenceIndex]);
        TypeWritter.instance.AttCurrentNPC(this);
    }
    public void EndedWriting() {
        if (dialogues[currentDialogueIndex].sentenceIndex < dialogues[currentDialogueIndex].sentences.Count -1) {
            dialogues[currentDialogueIndex].sentenceIndex++;
        }else {
            EnableLastBoxConfiguration();
            if (currentDialogueIndex < dialogues.Count -1) {
                currentDialogueIndex++;
            }else {
                dialogues[currentDialogueIndex].sentenceIndex = 0;
            }
        }
    }
    private void DefaultButtonConfiguration() {
        npcReference.acceptButton.gameObject.SetActive(false);
        npcReference.refuseButton.gameObject.SetActive(false);
        npcReference.finishButton.gameObject.SetActive(true);
        npcReference.nextButton.gameObject.SetActive(true);
    }
    private void EnableLastBoxConfiguration() {
        npcReference.acceptButton.gameObject.SetActive(false);
        npcReference.refuseButton.gameObject.SetActive(false);
        npcReference.finishButton.gameObject.SetActive(true);
        npcReference.nextButton.gameObject.SetActive(false);
    }
}
[Serializable]
public class DialogueDatabase{
    public List<Dialogue> randomDialogues = new List<Dialogue>();
}
[Serializable]
public class NPCReferenceData :  PropertyAttribute
{
    [Header("Características do NPC")]
    public Sprite perfilSprite;
    public Image perfilImage;
    public string npcName;
    public TextMeshProUGUI npcNameReference;
    [Header("Referência do NPC em cena")]
    public TextMeshProUGUI npcText;
    public GameObject talkBox;
    public GameObject questIcon;
    public GameObject interactIcon;
    public Button nextButton;
    public Button acceptButton;
    public Button refuseButton;
    public Button finishButton;
}
