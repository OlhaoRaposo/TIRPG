using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    bool isInMainMenu = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetIsInMainMenu(bool b)
    {
        isInMainMenu = b;
    }
    public bool GetIsInMainMenu()
    {
        return isInMainMenu;
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
    public void UnloadSceneByIndex(string sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
    public void LoadAdditiveSceneByName(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public void UnloadSceneByName(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(sceneName);
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}