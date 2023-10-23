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

    float tooltipScale = 1f;

    void Awake()
    {
        instance = this;
    }
    void LateUpdate()
    {
        if (!isOn) return;

        if (interactableObject != null)
        {
            target.rectTransform.position = WorldToScreenPosition(interactableObject.position);
            target.rectTransform.localScale = Vector3.one * tooltipScale;
        }
            
    }
    
    public void DisableTooltip()
    {
        interactableObject = null;
        isOn = false;
        target.gameObject.SetActive(false);
    }
    public void ToggleTooltip(Transform obj)
    {
        interactableObject = obj;
        isOn = !isOn;
        target.gameObject.SetActive(!target.gameObject.activeSelf);
    }
    public void SetScale(float scale)
    {
        tooltipScale = Mathf.Clamp(scale, .1f, 1f);
        SetAlpha();
    }
    void SetAlpha()
    {
        Color tooltipColor = target.color;
        tooltipColor.a = tooltipScale;
        target.color = tooltipColor;
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