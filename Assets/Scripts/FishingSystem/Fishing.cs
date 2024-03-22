using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    Vector3 centro;
    Vector3 pontoDeArraste;
    Vector3 PosicaoMouse;
    [SerializeField] GameObject visual;
    Camera cam;
    float tempoLimite;
    float timePesca;
    float timeArraste;
    int arraste = 2;
    bool pescando;
    [SerializeField] List<ItemData> peixes = new List<ItemData>();

    void Start()
    {
        cam = Camera.main;
        pontoDeArraste = transform.position;
    }

    void Update()
    {
        tempoLimite += Time.deltaTime;

        if(tempoLimite >= arraste)
        {
            Debug.Log("Mudei de Posição");
            pontoDeArraste = Random.insideUnitCircle * 5;
            tempoLimite = 0f;
            Arraste();
        }
        
        if(Input.GetMouseButton(0))
        {
            //Debug.Log("Segurando");
            PosicaoMouse = Input.mousePosition;

            Ray ray = cam.ScreenPointToRay(PosicaoMouse);
            if(Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if(Vector3.Distance(pontoDeArraste, raycastHit.point) <= 2)
                {
                    timePesca += Time.deltaTime;

                    timeArraste = 0f;

                    if (timePesca >= 3f)
                    {
                        Pescou();
                    }
                }
                else
                {
                    timeArraste += Time.deltaTime;

                    if(timeArraste >= 2f)
                    {
                        Debug.Log("Zerei");
                        timePesca = 0f;
                        timeArraste = 0f;
                    }
                }
            }
        }

        /*if(Input.GetMouseButtonDown(0))
        {
            pescando = true;

            PosicaoMouse = Input.mousePosition;

            if (Vector3.Distance(pontoDeArraste, PosicaoMouse) < 1)
            {
                timePesca += Time.deltaTime;

                if(timePesca >= 3f)
                {
                    Pescou();
                }
            }
            else
            {
                timePesca = 0;
            }
        }*/
    }

    void Arraste()
    {
        if(Vector3.Distance(centro, pontoDeArraste) > 5)
        {
            pontoDeArraste = centro;
            pontoDeArraste = Random.insideUnitCircle * 5;
            Arraste();
        }
        else
        {
            arraste = Random.Range(2,6);
            visual.transform.position = new Vector3(pontoDeArraste.x, visual.transform.position.y, pontoDeArraste.y);
        }
    }

    void Pescou()
    {
        Debug.Log("Pesquei");
        timePesca = 0f;
        visual.SetActive(false);
        int pescado = Random.Range(0, peixes.Count);
        if(peixes.Count > 0)
        {
            PlayerInventory.instance.AddItemToInventory(peixes[pescado]);
        }
    }
}
