using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractTooltip : MonoBehaviour
{
    public static InteractTooltip instance;

    [SerializeField] Transform interactableObject;
    [SerializeField] Image target;

    [SerializeField] bool isOn = false;

    void Awake()
    {
        instance = this;
    }
    void LateUpdate()
    {
        if (!isOn) return;

        target.rectTransform.position = WorldToScreenPosition(interactableObject.position);
    }
    public void ToggleTooltip(Transform obj)
    {
        interactableObject = obj;
        isOn = !isOn;
        target.gameObject.SetActive(!target.gameObject.activeSelf);
    }

    public bool GetIsOn()
    {
        return isOn;
    }

    public Vector3 WorldToScreenPosition(Vector3 position)
    {
        Camera test = Camera.main;
        Vector3 screenPos = test.WorldToScreenPoint(position);
        screenPos.x = Mathf.Clamp(screenPos.x, Screen.width * .1f, Screen.width * .9f);
        screenPos.y = Mathf.Clamp(screenPos.y, Screen.height * .1f, Screen.height * .9f);

        return screenPos;
    }
}
