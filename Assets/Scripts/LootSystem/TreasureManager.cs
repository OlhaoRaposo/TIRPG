using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager instance;

    void Awake()
    {
        instance = this;
    }

    public void OpenTreasure(TreasureChest treasure)
    {
        int controller = treasure.amountRewards;
        
        for(int index = 0; controller > 0; index++)
        {
            int quantityDrawn = Random.Range(0, controller + 1);
            Debug.Log($"Quantidade: {quantityDrawn}");
            
            ItemData itemDrawn = treasure.possibleRewards[Random.Range(0,treasure.possibleRewards.Count)];
            Debug.Log($"Item: {itemDrawn}");

            for(int aux = 0; aux < quantityDrawn; aux++)
            {
                /*if(controller > 0)
                {*/
                    //PlayerInventory.instance.AddItemToInventory(itemDrawn);
                    controller--;
                    Debug.Log($"Adicionei");
                /*}*/
            }
            /*if(controller < 0)
            {
                controller = 0;
            }*/
            Debug.Log($"Index: {controller}");
        }
    }
}
