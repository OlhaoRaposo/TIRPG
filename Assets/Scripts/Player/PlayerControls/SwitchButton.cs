using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    /*Lista com as opções de ações possíveis durante o jogo para fazer a 
    diferenciação do botão de troca da tecla de comando no menu de controles.*/
    [SerializeField]enum ButtonAction
    {
        Jump,Dash,Run,StealthMode,Inventory,Map,Throwables,
        Consumables,Interaction,PrimaryWeapon,SecondaryWeapon,
        ReloadGun,SkillTree
    }
    [Header ("Action Identification")]
    //Variável que identifica a ação que deve ser alterada pelo botão.
    [SerializeField]ButtonAction action;

    //Descrição da ação de pular e texto com a tecla de comando definida.
    [Header ("Jump")]
	[SerializeField]Text descJump;
	[SerializeField]Text buttonJump;

    //Descrição da ação de esquivar e texto com a tecla de comando definida.
    [Header ("Dash")]
    [SerializeField]Text descDash;
	[SerializeField]Text buttonDash;

    //Descrição da ação de correr e texto com a tecla de comando definida.
    [Header ("Run")]
    [SerializeField]Text descRun;
	[SerializeField]Text buttonRun;

    //Descrição da ação do modo furtivo e texto com a tecla de comando definida.
    [Header ("Stealth Mode")]
    [SerializeField]Text descStealthMode;
	[SerializeField]Text buttonStealthMode;

    //Descrição da ação de abrir o inventário e texto com a tecla de comando definida.
    [Header ("Inventory")]
    [SerializeField]Text descInventory;
	[SerializeField]Text buttonInventory;

    //Descrição da ação de abrir o mapa e texto com a tecla de comando definida.
    [Header ("Map")]
    [SerializeField]Text descMap;
	[SerializeField]Text buttonMap;

    //Descrição da ação de arremessar itens e texto com a tecla de comando definida.
    [Header ("Throwables")]
    [SerializeField]Text descThrowables;
	[SerializeField]Text buttonThrowables;

    //Descrição da ação de utiliar consumíveis e texto com a tecla de comando definida.
    [Header ("Consumables")]
    [SerializeField]Text descConsumables;
	[SerializeField]Text buttonConsumables;

    //Descrição da ação de interagir com outro objetos, personagens, etc. e texto com a tecla de comando definida.
    [Header ("Interaction")]
    [SerializeField]Text descInteraction;
	[SerializeField]Text buttonInteraction;

    //Descrição da ação de equipar a arma principal e texto com a tecla de comando definida.
    [Header ("Primary Weapon")]
    [SerializeField]Text descPrimaryWeapon;
	[SerializeField]Text buttonPrimaryWeapon;

    //Descrição da ação de equipar a arma secundária e texto com a tecla de comando definida.
    [Header ("Secondary Weapon")]
    [SerializeField]Text descSecondaryWeapon;
	[SerializeField]Text buttonSecondaryWeapon;

    //Descrição da ação de recarregar a arma equipada e texto com a tecla de comando definida.
    [Header ("Reload Gun")]
    [SerializeField]Text descReloadGun;
	[SerializeField]Text buttonReloadGun;

    //Descrição da ação de abrir o painel de habilidades e texto com a tecla de comando definida.
    [Header ("Skill Tree")]
    [SerializeField]Text descSkillTree;
	[SerializeField]Text buttonSkillTree;

    void Awake()
    {
        //Inicializa os textos das descrições das ações.
        descJump.text = "Jump";
        descDash.text = "Dash";
        descRun.text = "Sprint";
        descStealthMode.text = "Stealth Mode";
        descInventory.text = "Inventory";
        descMap.text = "Map";
        descThrowables.text = "Throwables";
        descConsumables.text = "Consumables";
        descInteraction.text = "Interaction";
        descPrimaryWeapon.text = "Primary Weapon";
        descSecondaryWeapon.text = "Secondary Weapon";
        descReloadGun.text = "Reload";
        descSkillTree.text = "Skill Tree";
    }

    //Atualiza os textos que indica as tecla de comando para realizar as ações ao abrir o menu ou ao fazer a alteração de alguma tecla.
    void FixedUpdate()
    {
        //Faz a conversão de KeyCode para string e atribui para cada variável a tecla definida para cada ação.
        buttonJump.text = InputController.instance.jump.ToString();
        buttonDash.text = InputController.instance.dash.ToString();
        buttonRun.text = InputController.instance.run.ToString();
        buttonStealthMode.text = InputController.instance.stealth.ToString();
        buttonInventory.text = InputController.instance.inventory.ToString();
        buttonMap.text = InputController.instance.map.ToString();
        buttonThrowables.text = InputController.instance.throwables.ToString();
        buttonConsumables.text = InputController.instance.consumables.ToString();
        buttonInteraction.text = InputController.instance.interaction.ToString();
        buttonPrimaryWeapon.text = InputController.instance.primaryWeapon.ToString();
        buttonSecondaryWeapon.text = InputController.instance.secondaryWeapon.ToString();
        buttonReloadGun.text = InputController.instance.reloadGun.ToString();
        buttonSkillTree.text = InputController.instance.skillTree.ToString();
    }

    /*Método que chama a função do controlador de comandos informando qual das teclas de comando deve ser alterada.
    Passa em parâmetro para o controlador a atual tecla convertida em uma string.*/
    public void ChangeButtonAction()
    {
        //Verifica qual é a ação definida naquele botão.
        switch(action)
        {
            //Informa em cada caso a atual tecla responsável pela ação em forma de uma string para a função do controlador.
            case ButtonAction.Jump:
                InputController.instance.ChangeKey(buttonJump.text);
                break;
            case ButtonAction.Dash:
                InputController.instance.ChangeKey(buttonDash.text);
                break;
            case ButtonAction.Run:
                InputController.instance.ChangeKey(buttonRun.text);
                break;
            case ButtonAction.StealthMode:
                InputController.instance.ChangeKey(buttonStealthMode.text);
                break;
            case ButtonAction.Inventory:
                InputController.instance.ChangeKey(buttonInventory.text);
                break;
            case ButtonAction.Map:
                InputController.instance.ChangeKey(buttonMap.text);
                break;
            case ButtonAction.Throwables:
                InputController.instance.ChangeKey(buttonThrowables.text);
                break;
            case ButtonAction.Consumables:
                InputController.instance.ChangeKey(buttonConsumables.text);
                break;
            case ButtonAction.Interaction:
                InputController.instance.ChangeKey(buttonInteraction.text);
                break;
            case ButtonAction.PrimaryWeapon:
                InputController.instance.ChangeKey(buttonPrimaryWeapon.text);
                break;
            case ButtonAction.SecondaryWeapon:
                InputController.instance.ChangeKey(buttonSecondaryWeapon.text);
                break;
            case ButtonAction.ReloadGun:
                InputController.instance.ChangeKey(buttonReloadGun.text);
                break;
            case ButtonAction.SkillTree:
                InputController.instance.ChangeKey(buttonSkillTree.text);
                break;
        }
    }

    //Método que chama a redefinição das teclas do controlador de comandos.
    public void ResetKeys()
    {
        InputController.instance.ResetDefault();
    }
}