using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public bool spawnWasCreated;
    public static WorldController worldController;
    private void Start()
    {
        if (worldController == null) {
            worldController = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }
    }
}
