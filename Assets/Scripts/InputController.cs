using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    //Variável de acesso global para o controlador de comandos.
    public static InputController instance;

    [Header ("Action Keys")]
    //Variável que guarda a tecla de ação de pular.
    public KeyCode jump = KeyCode.Space;
    //Variável que guarda a tecla de ação de esquivar.
    public KeyCode dash = KeyCode.F;
    //Variável que guarda a tecla de ação de correr.
    public KeyCode run = KeyCode.LeftShift;
    //Variável que guarda a tecla de ação do modo furtivo.
    public KeyCode stealth = KeyCode.LeftControl;
    //Variável que guarda a tecla de ação de abrir o inventário.
    public KeyCode inventory = KeyCode.Tab;
    //Variável que guarda a tecla de ação  de abrir o mapa.
    public KeyCode map = KeyCode.M;
    //Variável que guarda a tecla de ação de arremessar itens.
    public KeyCode throwables = KeyCode.G;
    //Variável que guarda a tecla de ação de utilizar os consumíveis.
    public KeyCode consumables = KeyCode.Q;
    //Variável que guarda a tecla de ação de interagir com itens. personagens, etc.
    public KeyCode interaction = KeyCode.E;
    //Variável que guarda a tecla de ação de selecionar a arma principal.
    public KeyCode primaryWeapon = KeyCode.Alpha1;
    //Variável que guarda a tecla de ação de selecionar a arma secundária.
    public KeyCode secondaryWeapon = KeyCode.Alpha2;
    //Variável que guarda a tecla de ação de recarregar a arma atual.
    public KeyCode reloadGun = KeyCode.R;
    //Variável que guarda a tecla de ação de abrir o painel de habilidades.
    public KeyCode skillTree = KeyCode.C;

    [Header ("Key Change Variables")]
    //Variável de controle que verifica se está em alteração de tecla.
    [SerializeField]bool keyCustomization = false;
    //Variável que guarda a nova tecla, tecla que será atribuída a ação.
    [SerializeField]KeyCode newKey;
    //Variável que guarda a última tecla, tecla que era responsável pela ação para casos de a nova tecla já estar definida para outra ação.
    [SerializeField]KeyCode oldKey;
    //Variável que guarda a tecla que deve ser alterada passada pelo botão no menu de controles.
    [SerializeField]string selectedKey;
    //Lista que guarda todas as teclas de acordo com a declaração acima para auxiliar na hora de alterar e/ou substituir teclas já definidas.
	[SerializeField]List<KeyCode> commandKeys;
    //Guarda as teclas padrões para cada ação.
    [SerializeField]List<KeyCode> standardCommandKeys;

    void Awake()
    {
        //Garante que exista apenas um controlador na cena.
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        //Inicializa as lista com as teclas padrões.
        commandKeys = new List<KeyCode>(){jump,dash,run,stealth,inventory,map,throwables,consumables,
                                                        interaction,primaryWeapon,secondaryWeapon,reloadGun,skillTree};

        standardCommandKeys = new List<KeyCode>(){jump,dash,run,stealth,inventory,map,throwables,consumables,
                                                        interaction,primaryWeapon,secondaryWeapon,reloadGun,skillTree};
    }

    void Update()
    {
        //Verifica se a customização de comandos está ativa.
        if(keyCustomization == true)
        {
            //Fica em loop pela lista de teclas disponíveis da unity quando está em customização para saber qual será a nova tecla.
            foreach(KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                //Verifica qual foi a primeira tecla pressionada depois de iniciar a customização, sendo mouse, teclado ou joystick, não sendo possível as teclas de movimentação por AWSD.
                if(Input.GetKeyDown(key) && key != KeyCode.A && key != KeyCode.W && key != KeyCode.S && key != KeyCode.D)
                {
                    //Atribui para a variável a tecla pressionada.
                    newKey = key;
                    //Desativa a customização.
                    keyCustomization = false;
                    //Chama o método para verificar se a nova tecla já está definida para outra ação.
                    CheckKey();
                    //Altera a tecla selecionada para a nova e atualiza as demais.
                    UpdateKey();
                    //Em caso de a nova tecla já está definida em outra ação inverte as teclas.
                    ReplaceKey();
                }  
            }
        }
    }
    
    //Método que inicializa a customização.
    public void ChangeKey(string keyName)
    {
        //Atibui para a variável a tecla que deve ser alterada.
        selectedKey = keyName;
        //Ativa a customização.
        keyCustomization = true;
    }

    //Método que verifica se já há ação atribuída com a nova tecla, e a remove da antiga ação em caso positivo.
	void CheckKey()
	{
        //Percorre a lista auxiliar para verificar se a nova tecla já está definida para outra ação.
		for(int index = 0; index < commandKeys.Count; index++)
		{
            //Verifica se a nova tecla já estava na lista e em caso positivo o index informará qual é a ação que ela estava atribuída.
			if(commandKeys[index] == newKey)
			{
                //Remove a atual tecla da antiga ação que ela era responsável.
                switch(index)
                {
                    case 0:
                        jump = KeyCode.None;
                        break;
                    case 1:
                        dash = KeyCode.None;
                        break;
                    case 2:
                        run = KeyCode.None;
                        break;
                    case 3:
                        stealth = KeyCode.None;
                        break;
                    case 4:
                        inventory = KeyCode.None;
                        break;
                    case 5:
                        map = KeyCode.None;
                        break;
                    case 6:
                        throwables = KeyCode.None;
                        break;
                    case 7:
                        consumables = KeyCode.None;
                        break;
                    case 8:
                        interaction = KeyCode.None;
                        break;
                    case 9:
                        primaryWeapon = KeyCode.None;
                        break;
                    case 10:
                        secondaryWeapon = KeyCode.None;
                        break;
                    case 11:
                        reloadGun = KeyCode.None;
                        break;
                    case 12:
                        skillTree = KeyCode.None;
                        break;
                }
			}
		}
	}

    //Método que atribui na variável "oldKey" qual era a antiga tecla da ação e chama a função que altera para a nova tecla.
    void UpdateKey()
    {
        //Para cada verificação guarda a antiga tecla e altera para a nova.
        if(jump.ToString() == selectedKey)
        {
            oldKey = jump;
            NewKey(ref jump, newKey);
        }
        else if (dash.ToString() == selectedKey)
        {
            oldKey = dash;
            NewKey(ref dash, newKey);
        }
        else if (run.ToString() == selectedKey)
        {
            oldKey = run;
            NewKey(ref run, newKey);
        } 
        else if (stealth.ToString() == selectedKey)
        {
            oldKey = stealth;
            NewKey(ref stealth, newKey);
        }
        else if (inventory.ToString() == selectedKey)
        {
            oldKey = inventory;
            NewKey(ref inventory, newKey);
        } 
        else if (map.ToString() == selectedKey)
        {
            oldKey = map;
            NewKey(ref map, newKey);
        }  
        else if (throwables.ToString() == selectedKey)
        {
            oldKey = throwables;
            NewKey(ref throwables, newKey);
        }  
        else if (consumables.ToString() == selectedKey)
        {
            oldKey = consumables;
            NewKey(ref consumables, newKey);
        }   
        else if (interaction.ToString() == selectedKey)
        {
            oldKey = interaction;
            NewKey(ref interaction, newKey);
        }   
        else if (primaryWeapon.ToString() == selectedKey)
        {
            oldKey = primaryWeapon;
            NewKey(ref primaryWeapon, newKey);
        }  
        else if (secondaryWeapon.ToString() == selectedKey)
        {
            oldKey = secondaryWeapon;
            NewKey(ref secondaryWeapon, newKey);
        }   
        else if (reloadGun.ToString() == selectedKey)
        {
            oldKey = reloadGun;
            NewKey(ref reloadGun, newKey);
        }  
        else if (skillTree.ToString() == selectedKey)
        {
            oldKey = skillTree;
            NewKey(ref skillTree, newKey);
        }

        //Atualiza a lista auxiliar com a atual configuração de teclas.
        commandKeys = new List<KeyCode>(){jump,dash,run,stealth,inventory,map,throwables,consumables,
                                                        interaction,primaryWeapon,secondaryWeapon,reloadGun,skillTree};
    }

    //Altera a tecla da ação para a nova tecla.
    void NewKey(ref KeyCode key1, KeyCode key2)
    {
        //Atibui a nova tecla a ação.
        key1 = key2;
    }

    //Retorna para ação que detinha a nova tecla a tecla que era da ação que foi alterada. Faz a substituição das teclas.
    void ReplaceKey()
    {
        //Percorre a lista auxiliar para verificar se houve ação que teve a tecla removida na atribuição da nova tecla.
        for(int index = 0; index < commandKeys.Count; index++)
		{
            //Verifica a cada loop se a atual variável há tecla definida.
			if(commandKeys[index] == KeyCode.None)
			{
                //Para cada verificação atribui para a ação a antiga tecla, isso ocorre quando há a inversão de comandos.
                switch(index)
                {
                    case 0:
                        jump = oldKey;
                        break;
                    case 1:
                        dash = oldKey;
                        break;
                    case 2:
                        run = oldKey;
                        break;
                    case 3:
                        stealth = oldKey;
                        break;
                    case 4:
                        inventory = oldKey;
                        break;
                    case 5:
                        map = oldKey;
                        break;
                    case 6:
                        throwables = oldKey;
                        break;
                    case 7:
                        consumables = oldKey;
                        break;
                    case 8:
                        interaction = oldKey;
                        break;
                    case 9:
                        primaryWeapon = oldKey;
                        break;
                    case 10:
                        secondaryWeapon = oldKey;
                        break;
                    case 11:
                        reloadGun = oldKey;
                        break;
                    case 12:
                        skillTree = oldKey;
                        break;
                }

                //Anula a antiga tecla depois da verificação de inversão.
                oldKey = KeyCode.None;
                //Atualiza a lista auxiliar com a atual configuração de teclas.
                commandKeys = new List<KeyCode>(){jump,dash,run,stealth,inventory,map,throwables,consumables,
                                                        interaction,primaryWeapon,secondaryWeapon,reloadGun,skillTree};
			}
		}
    }

    //Método que redefine todas as teclas ao padrão.
    public void ResetDefault()
    {
        //Retorna a tecla padrão para cada variável.
        jump = standardCommandKeys[0];
        dash = standardCommandKeys[1];
        run = standardCommandKeys[2];
        stealth = standardCommandKeys[3];
        inventory = standardCommandKeys[4];
        map = standardCommandKeys[5];
        throwables = standardCommandKeys[6];
        consumables = standardCommandKeys[7];
        interaction = standardCommandKeys[8];
        primaryWeapon = standardCommandKeys[9];
        secondaryWeapon = standardCommandKeys[10];
        reloadGun = standardCommandKeys[11];
        skillTree = standardCommandKeys[12];

        //Atualiza a lista auxiliar com a atual configuração de teclas
        commandKeys = standardCommandKeys;
    }
}