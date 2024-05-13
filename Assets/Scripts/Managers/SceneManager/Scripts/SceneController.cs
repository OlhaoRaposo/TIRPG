using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    bool isInMainMenu = false;

    [SerializeField] string[] gameplayScenes;

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

        //SceneManager.sceneUnloaded += scene => Debug.Log($"Unloaded {scene.name}");

        LoadAdditiveSceneByName("PlayerScene");
    }
    void Start()
    {
        LoadMenu();
    }
    public void LoadMenu()
    {
        LoadAdditiveSceneAsyncByName("NewMenu");
        LoadAdditiveSceneAsyncByName("LobbyScene");
        isInMainMenu = true;
    }
    public void UnloadMenu()
    {
        UnloadSceneByName("NewMenu");
        isInMainMenu = false;
    }
    public void LoadGameplayScenes()
    {
        foreach(string s in gameplayScenes)
        {
            LoadAdditiveSceneAsyncByName(s);
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

    public bool CheckIfSceneIsLoaded(string sceneName)
    {
        foreach (Scene scene in SceneManager.GetAllScenes())
        {
            if (scene.name == sceneName)
            {
                return scene.isLoaded;
            }
        }

        return false;
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
    public void LoadAdditiveSceneAsyncByIndex(int sceneIndex)
    {
        Time.timeScale = 1;
        
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
    }
    public void UnloadSceneByIndex(string sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(sceneIndex);
    }

    public void LoadSceneByName(string sceneName)
    {
        if (CheckIfSceneIsLoaded(sceneName)) return;

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
    public void LoadAdditiveSceneByName(string sceneName)
    {
        if (CheckIfSceneIsLoaded(sceneName)) return;

        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    public void LoadAdditiveSceneAsyncByName(string sceneName)
    {
        if (CheckIfSceneIsLoaded(sceneName)) return;

        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
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