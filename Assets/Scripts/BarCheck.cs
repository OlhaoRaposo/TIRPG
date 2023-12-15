using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BarCheck : MonoBehaviour
{
    [SerializeField]
    private Vector2 minLeft;
    [SerializeField]
    private Vector2 maxRight;

    private Vector3 hitPosition;
    
    [SerializeField]
    private float skillCheckDifficulty;
    
    private float startTime;
    [SerializeField]
    private Image barPointer;

    [SerializeField]
    private Image barHit;
    [SerializeField]
    private float speed;
    
    public InteractiveObject interactiveObject;

    private void Start()
    {
        StartSkilLBar();
    }

    private void StartSkilLBar()
    {
        startTime = Time.time;
        skillCheckDifficulty = Mathf.Round(Random.Range(0.06f, 0.1f) * 100.0f) / 100.0f;
        barHit.rectTransform.localPosition = new Vector3(Random.Range(-450, 450), 0, 0);

        barHit.fillAmount = skillCheckDifficulty;
        barHit.rectTransform.pivot = new Vector2(1, .5f);
        minLeft = barHit.rectTransform.localPosition;
        maxRight = new Vector2(minLeft.x - ((1024 - (1- skillCheckDifficulty) * 1024)), .5f);
    }

    void Update()
    {
        float point1, point2;
        if (minLeft.x < maxRight.x) {
            point1 = minLeft.x;
            point2 = maxRight.x;
        }else {
            point1 = maxRight.x;
            point2 = minLeft.x;
        }
        MoveBarPointer();
        if (Input.GetMouseButtonDown(1))
        {
           if(barPointer.transform.localPosition.x <= minLeft.x && barPointer.transform.localPosition.x >= maxRight.x)
           {
               if (interactiveObject != null) {
                   interactiveObject.validations.Add(true);
                   Destroy(this.gameObject);
               }
           }else {
               if (interactiveObject != null) {
                   interactiveObject.validations.Add(false);
                   Destroy(this.gameObject);
               }
           }
           interactiveObject.CheckValidations();
        }
    }

    private void MoveBarPointer()
    {
        float pingPongValue = Mathf.PingPong((Time.time - startTime) * speed, 1000) - 500; 
        Transform transform = barPointer.transform; 
        Vector3 newPosition = barPointer.rectTransform.localPosition;
        newPosition.x = pingPongValue;
        barPointer.rectTransform.localPosition = newPosition;
    }
}
