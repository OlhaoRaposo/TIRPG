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

    [SerializeField] float minimumSize = .2f;
    [SerializeField] float maximumSize = 1f;
    float tooltipScale = 1f;

    bool canHightlightInteraction = false;

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
            if (!canHightlightInteraction)
            {
                target.rectTransform.localScale = Vector3.one * tooltipScale;
            }
            else
            {
                target.rectTransform.localScale = Vector3.one;
                SetAlpha(1f);
            }
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
        tooltipScale += minimumSize;
        tooltipScale = Mathf.Clamp(scale, minimumSize, maximumSize);
        SetAlpha();
    }
    void SetAlpha()
    {
        Color tooltipColor = target.color;
        tooltipColor.a = tooltipScale;
        target.color = tooltipColor;
    }
    void SetAlpha(float alpha)
    {
        Color tooltipColor = target.color;
        tooltipColor.a = alpha;
        target.color = tooltipColor;
    }
    public bool GetIsOn()
    {
        return isOn;
    }
    public void SetHighlightInteraction(bool can)
    {
        canHightlightInteraction = can;
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