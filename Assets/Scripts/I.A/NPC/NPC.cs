using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable_Npc))]
public class NPC : MonoBehaviour
{
    public List<Dialogue> dialogues;
    public bool hasQuest;
    public List<Dialogue> questDialogue;
    public UnityEvent OnEndQuestDialogue;
    [SerializeField] public int currentDialogueIndex;
    [SerializeField] private bool isTalking;
    [SerializeField] private NPCReferenceData npcReference;
    [SerializeField] private GameObject hasDialogue;
    
    [SerializeField] public bool invoked;
    [SerializeField] private UnityEvent OnEndDialogue = new UnityEvent();
    public bool hasDrop;
    public ItemDropInfo[] dropInfo;
    [Header("Tutorial NPC")]
    public bool isTutorialNPC;
    [SerializeField] public TutorialNPC tutorialNPC;
    [SerializeField] public int tutorialIndex;
    private void Start() {
        npcReference.talkBox.SetActive(true);
        npcReference.npcText.text = "";
        npcReference.npcNameReference.text = npcReference.npcName;
        npcReference.talkBox.SetActive(false);
        if (isTutorialNPC) {
            tutorialNPC.Initiate(tutorialIndex);
            dialogues = tutorialNPC.dialogues;
        }
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
        //lock Player 
        PlayerCameraMovement.instance.ToggleAimLock(false);
        DefaultButtonConfiguration();
    }
    public void CompleteTutorial() {
        if (isTutorialNPC) {
            WorldController.worldController.tutorialCompleted = true;
        }
    }
    public void DisableChatBox() {
        npcReference.npcText.text = "";
        npcReference.talkBox.SetActive(false);
        //TODO
        //Unock Player Movement
        //Unlock Player Camera
        //Unlock Player Inputs
        PlayerCameraMovement.instance.ToggleAimLock(true);
    }
    private void Talk() {
        if (hasQuest) {
            TypeWritter.instance.Write(npcReference.npcText, dialogues[currentDialogueIndex].sentences[dialogues[currentDialogueIndex].sentenceIndex]);
            TypeWritter.instance.AttCurrentNPC(this);
        }
        TypeWritter.instance.Write(npcReference.npcText, dialogues[currentDialogueIndex].sentences[dialogues[currentDialogueIndex].sentenceIndex]);
        TypeWritter.instance.AttCurrentNPC(this);
    }
    public void EndedWriting() {
        if (hasQuest)
        {
            if (questDialogue[currentDialogueIndex].sentenceIndex < questDialogue[currentDialogueIndex].sentences.Count -1) {
                questDialogue[currentDialogueIndex].sentenceIndex++;
            }else {
                EnableLastBoxConfiguration();
                hasQuest = false;
                OnEndQuestDialogue.Invoke();
            }
        }else {
            if (dialogues[currentDialogueIndex].sentenceIndex < dialogues[currentDialogueIndex].sentences.Count -1) {
                dialogues[currentDialogueIndex].sentenceIndex++;
            }else {
                EnableLastBoxConfiguration();
                if(!invoked) {
                    if(hasDialogue != null)
                        hasDialogue.SetActive(false);
                    OnEndDialogue.Invoke();
                    if(hasDrop)
                        ItemDropManager.instance.DropItem(dropInfo, transform.position);
                    invoked = true;
                }
                if (currentDialogueIndex < dialogues.Count -1) {
                    currentDialogueIndex++;
                }else {
                    dialogues[currentDialogueIndex].sentenceIndex = 0;
                }
            }
        }
    }
    private void DefaultButtonConfiguration() {
        npcReference.finishButton.gameObject.SetActive(false);
        npcReference.nextButton.gameObject.SetActive(true);
    }
    private void EnableLastBoxConfiguration() {
        npcReference.finishButton.gameObject.SetActive(true);
        npcReference.nextButton.gameObject.SetActive(false);
    }

    public void AddQuest(string questCode) {
        QuestManager.instance.AddQuest(questCode);
    }
}
[Serializable]
public class NPCReferenceData :  PropertyAttribute
{
    [Header("Características do NPC")]
    public string npcName;
    public TextMeshProUGUI npcNameReference;
    [Header("Referência do NPC em cena")]
    public TextMeshProUGUI npcText;
    public GameObject talkBox;
    public Button nextButton;
    public Button finishButton;
}
[Serializable]
public class TutorialNPC  {
   
    public List<Dialogue> dialogues = new List<Dialogue>();
    public void Initiate(int index) {
        dialogues = new List<Dialogue>();
        dialogues.Add(new Dialogue());
        dialogues.Add(new Dialogue());
        dialogues[0].sentences = new List<string>();
        dialogues[1].sentences = new List<string>();
        switch (index) {
            case  0:
                dialogues[0].sentences.Add("Bem vindo ao tutorial de movimentação");
                dialogues[0].sentences.Add("Utilize as teclas W, A, S, D para se movimentar");
                dialogues[0].sentences.Add($"Utilize a tecla: '{InputController.instance.jump}' para pular");
                dialogues[0].sentences.Add($"Com a tecla: '{InputController.instance.dash}' você irar dar um impulso para o lado em que estiver andando");
                dialogues[0].sentences.Add($"Para correr use: '{InputController.instance.run}'");
                dialogues[0].sentences.Add($"Ah e para interagir com o proximo tutor aperte a tecla: '{InputController.instance.interaction}'");
                dialogues[0].sentences.Add("Agora siga em frente");
                dialogues[1].sentences.Add("Mais a frente o proximo robô ira te ensinar mais alguns truques");
                break;
            case 1:
                dialogues[0].sentences.Add("Agora para a parte de Interação");
                dialogues[0].sentences.Add($"Apertando a tecla: '{InputController.instance.inventory}' você ira abrir um menu de inventário contendo todos seus items");
                dialogues[0].sentences.Add("Pode prosseguir");
                dialogues[1].sentences.Add("Mais a frente o proximo robô ira te ensinar mais alguns truques");
                break;
            case 2:
                dialogues[0].sentences.Add("Neste momento estamos entrando na parte mais crucial do treinamento o combate");
                dialogues[0].sentences.Add($"Você tem 2 espaços para armas disponiveis no seu inventario, para alternalos utilize: '{InputController.instance.primaryWeapon} e {InputController.instance.secondaryWeapon}'");
                dialogues[0].sentences.Add($"Para atirar utilize o botão esquerdo do mouse, e para mirar utilize o botão direito do mouse");
                dialogues[0].sentences.Add($"Para recarregar sua arma utilize a tecla: '{InputController.instance.reloadGun}'");
                dialogues[0].sentences.Add($"Com a tecla: '{InputController.instance.throwables}' você ira arremessar um item");
                dialogues[0].sentences.Add($"e com a: '{InputController.instance.consumables}' você pode recuperar parte da sua vida com um item de cura");
                dialogues[1].sentences.Add("Você pode testar suas habilidades de combate com boneco de treinamento a frente");
                dialogues[1].sentences.Add("E para sair daqui use o Teleporte no canto da sala");
                dialogues[1].sentences.Add("Boa sorte na sua jornada, e lembre-se de sempre se manter forte");
                break;
        }
    }
}