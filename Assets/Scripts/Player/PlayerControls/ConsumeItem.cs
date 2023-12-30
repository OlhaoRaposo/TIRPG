using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeItem : MonoBehaviour
{
    [SerializeField] float consumableCooldown = 5f;
    float nextConsumable = 0f;
    private void Update()
    {
        if (Input.GetKeyDown(InputController.instance.consumables) && PlayerInventory.instance.GetConsumable() != null)
        {
            UseConsumable();
        }
    }
    void UseConsumable()
    {
        if (Time.time > nextConsumable)
        {
            PlayerHPController.instance.ChangeHP(PlayerInventory.instance.GetConsumable().healingAmount, false);
            nextConsumable = Time.time + consumableCooldown;
        }
    }
}
