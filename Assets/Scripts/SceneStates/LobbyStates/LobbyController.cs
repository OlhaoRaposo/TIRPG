using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    [SerializeField] GameObject[] lightObjects;
    void Start()
    {
        if (SceneStatesController.instance.GetGeneratorState())
        {
            TurnOnLights();
        }
    }
    public void TurnOnLights()
    {
        foreach (GameObject go in lightObjects)
        {
            go.SetActive(true);
        }
    }
}
