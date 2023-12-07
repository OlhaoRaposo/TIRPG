using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hitmark : MonoBehaviour
{
    public static Hitmark instance;

    Image hitmark;
    float t = 0f;
    [SerializeField] float lerpDuration = 1f;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        hitmark = GetComponent<Image>();
    }
    void FixedUpdate()
    {
        t = Mathf.Clamp01(t - (Time.fixedDeltaTime / lerpDuration));

        Color hitmarkColor = hitmark.color;
        hitmarkColor.a = t;
        hitmark.color = hitmarkColor;
    }

    public void ToggleHitmark()
    {
        t = 1f;
    }
}
