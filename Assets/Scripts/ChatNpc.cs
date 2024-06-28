using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Interactable_Npc))]
public class ChatNpc : MonoBehaviour 
{
    public List<Dialogue> dialogues;
    [SerializeField] private int currentDialogueIndex;
    [SerializeField] private int currentSentenceIndex;
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
    }
    private void DefaultButtonConfiguration() {
        npcReference.finishButton.gameObject.SetActive(false);
        npcReference.nextButton.gameObject.SetActive(true);
    }
    private void EnableLastBoxConfiguration() {
        npcReference.finishButton.gameObject.SetActive(true);
        npcReference.nextButton.gameObject.SetActive(false);
    }
}
[Serializable]
public class NPCReferenceDataa :  PropertyAttribute
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