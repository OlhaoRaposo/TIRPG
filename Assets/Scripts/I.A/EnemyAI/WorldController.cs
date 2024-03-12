using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    public bool spawnWasCreated;
    public static WorldController worldController;
    public bool isGameStarted;
    private bool playingAnimation;
    
    private GameObject camera;
    private Transform cameraTransform;
    private GameObject player;
    private CinemachineBrain brain;
    private GameObject essentialsCanvas;
    
    private void Start()
    {
        if (worldController == null) {
            worldController = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }

        essentialsCanvas = GameObject.Find("====CANVAS====");
        essentialsCanvas.SetActive(false);
        camera = Camera.main.gameObject;
        cameraTransform = GameObject.Find("CameraPositionReference").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        brain.enabled = false;
        Camera.main.gameObject.transform.localPosition = new Vector3(-.5f, 0f,-5);
        Camera.main.gameObject.transform.localRotation = Quaternion.Euler(10.7f, -5, 0);
        PlayerCamera.instance.ToggleAimLock(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            PlayerCamera.instance.ToggleAimLock(!PlayerCamera.instance.locked);
        }
        if (playingAnimation) {
            camera.transform.position = Vector3.Lerp(camera.transform.position, cameraTransform.position, Time.deltaTime * 2);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraTransform.rotation, Time.deltaTime * 2);
        }
    }

    public void StartGame() {
       Debug.Log("Game Started");
       isGameStarted = true;
       //Play Player out of stopChest(nao sei o nome de parapeito)
       if (player.TryGetComponent(out Animator anim)) {
           anim.SetTrigger("Start"); 
       }
       //Unload NewMenu Scene
       SceneManager.UnloadSceneAsync("NewMenu");
       //Começa as animaçoes do jogo
       StartCoroutine("PlayAnimation");
    }

    public IEnumerator PlayAnimation() {
        yield return new WaitForSeconds(3);
        playingAnimation = true;
        yield return new WaitForSeconds(3);
        brain.enabled = true;
        playingAnimation = false;
        PlayerCamera.instance.ToggleAimLock(true);
        essentialsCanvas.SetActive(true);
    }
}
