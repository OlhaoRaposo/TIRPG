using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public void CallQuitGame()
    {
        PlayerController.instance.Respawn();
        //SceneController.instance?.QuitGame();
    }
}
