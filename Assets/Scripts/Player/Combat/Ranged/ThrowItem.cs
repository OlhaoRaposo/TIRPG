using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    [SerializeField] float throwStrength = 20f;
    [SerializeField] float throwCooldown = 3f;

    //Posicao de onde o item é jogado (Substituir por um transform depois)
    [SerializeField] Vector3 throwPosition;

    [SerializeField] LayerMask throwableLayer;

    float nextThrow = 0f;
    void Update()
    {
        if (Input.GetKeyDown(InputController.instance.throwables))
        {
            if (Time.time >= nextThrow)
            {
                UseThrowable();
            }
            else
            {
                UIManager.instance.ShowTextFeedback("The item is on cooldown");
            }
        }
    }
    void UseThrowable()
    {
        if (PlayerInventory.instance.GetThrowable() == null)
        {
            UIManager.instance.ShowTextFeedback("No throwable item equipped");
            return;
        }

        //Comecar cooldown
        nextThrow = Time.time + throwCooldown;

        //Jogar item
        GameObject throwable = Instantiate(PlayerInventory.instance.GetThrowable().prefab, transform.position + throwPosition, Quaternion.identity);
        throwable.layer = LayerMask.NameToLayer("ThrownItems");
        throwable.GetComponent<Rigidbody>().AddForce(/*Camera.main.*/(transform.forward + transform.up).normalized * throwStrength, ForceMode.Impulse);
        throwable.GetComponent<Rigidbody>().AddTorque(Vector3.right * throwStrength/100f, ForceMode.Impulse);

        //Tirar um arremessavel do inventario
        PlayerInventory.instance.ThrowedItem();
    }
}
