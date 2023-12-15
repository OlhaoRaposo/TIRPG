using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    [SerializeField] float throwStrength = 10f;
    [SerializeField] float throwCooldown = 3f;

    //Posicao de onde o item é jogado (Substituir por um transform depois)
    [SerializeField] Vector3 throwPosition;

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
                //Mostrar aviso na tela: "O item está em cooldown"
            }
        }
    }
    void UseThrowable()
    {
        //Comecar cooldown
        nextThrow = Time.time + throwCooldown;

        //Jogar item
        GameObject throwable = Instantiate(PlayerInventory.instance.GetThrowable().prefab, transform.position + throwPosition, Quaternion.identity);
        throwable.GetComponent<Rigidbody>().AddForce(/*Camera.main.*/transform.forward * throwStrength, ForceMode.Impulse);
        throwable.GetComponent<Rigidbody>().AddTorque(Vector3.right * throwStrength, ForceMode.Impulse);
    }
}
