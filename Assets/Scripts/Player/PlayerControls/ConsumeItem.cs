using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : MonoBehaviour
{
    [SerializeField] float consumableCooldown = 5f;
    float nextConsumable = 0f;
    private void Update()
    {
        if (Input.GetKeyDown(InputController.instance.consumables))
        {
            UseConsumable(PlayerInventory.instance.GetConsumable());
        }
    }
    void UseConsumable(ItemData consumable)
    {
        if (consumable == null)
        {
            UIManager.instance.ShowTextFeedback("No consumable equipped");
            return;
        }

        if (Time.time > nextConsumable)
        {
            switch (consumable.buffType)
            {
                case BuffType.NONE:
                    PlayerHPController.instance.ChangeHP(PlayerInventory.instance.GetConsumable().healingAmount, false);
                    nextConsumable = Time.time + consumableCooldown;
                    break;
                case BuffType.STRENGTH:
                    PlayerStats.instance.BuffStrength(consumable.buffAttributeMultiplier, consumable.buffDuration);
                    nextConsumable = Time.time + consumable.buffDuration;
                    break;
                case BuffType.AGILITY:
                    PlayerStats.instance.BuffAgility(consumable.buffAttributeMultiplier, consumable.buffDuration);
                    nextConsumable = Time.time + consumable.buffDuration;
                    break;
                case BuffType.ENDURANCE:
                    PlayerStats.instance.BuffEndurance(consumable.buffAttributeMultiplier, consumable.buffDuration);
                    nextConsumable = Time.time + consumable.buffDuration;
                    break;
                case BuffType.INTELLIGENCE:
                    PlayerStats.instance.BuffIntelligence(consumable.buffAttributeMultiplier, consumable.buffDuration);
                    nextConsumable = Time.time + consumable.buffDuration;
                    break;
                case BuffType.STAMINA_REGENERATION:
                    PlayerStats.instance.BuffStaminaRegen(consumable.buffAttributeMultiplier, consumable.buffDuration);
                    nextConsumable = Time.time + consumable.buffDuration;
                    break;
            }

            //Remover consumivel do inventario
            if (consumable.name != "Cabaça") PlayerInventory.instance.UsedConsumable();
        }
        else
        {
            UIManager.instance.ShowTextFeedback("The consumable is on cooldown");
        }
    }

    //ADICIONAR METODOS DE TESTE
}
