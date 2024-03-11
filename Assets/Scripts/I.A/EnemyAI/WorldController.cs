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
    
    private void Start()
    {
        if (worldController == null) {
            worldController = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }
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
            player.transform.position = Vector3.Lerp(player.transform.position, player.transform.position +new Vector3(0, 0, 5), Time.deltaTime * 2);
        }
    }

    public void StartGame() {
       Debug.Log("Game Started");
       isGameStarted = true;
       if (player.TryGetComponent(out Animator anim)) {
           anim.SetTrigger("Start");
       }
       SceneManager.UnloadSceneAsync("NewMenu");
       playingAnimation = true;
       StartCoroutine("PlayAnimation");
    }

    public IEnumerator PlayAnimation() {
        PlayerCamera.instance.ToggleAimLock(true);
        yield return new WaitForSeconds(5);
        brain.enabled = true;
        playingAnimation = false;
    }
}
