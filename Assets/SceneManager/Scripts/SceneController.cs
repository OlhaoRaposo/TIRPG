using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    void Awake()
    {
        instance = this;
    }
    public void LoadSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadAdditiveSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
    }
    public void LoadSceneByName(int sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    public void LoadAdditiveSceneByName(int sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}