using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EssentialObjects : MonoBehaviour
{
    public static EssentialObjects instance;
  
    public GameObject player;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "MountainScene" || SceneManager.GetActiveScene().name == "Forest" )
        {
            player.transform.position = GameObject.Find("PlayerPoint").transform.position;
        }
    }
    
}
