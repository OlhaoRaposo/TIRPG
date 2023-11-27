using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStatesController : MonoBehaviour
{
    public static SceneStatesController instance;

    [Header("Lobby states")]
    bool generatorOn = false;

    void Awake()
    {
        instance = this;
    }

    public void SetGeneratorState(bool state)
    {
        generatorOn = state;
    }
    public bool GetGeneratorState()
    {
        return generatorOn;
    }
}
