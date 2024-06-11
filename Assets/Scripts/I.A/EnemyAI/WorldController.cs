using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class WorldController : MonoBehaviour
{
    public bool spawnWasCreated;
    public static WorldController worldController;
    public bool isGameStarted = false;
    private bool playingAnimation;
    
    private new GameObject camera;
    private Transform cameraTransform;
    private GameObject player;
    private GameObject essentialsCanvas;
    
    [Header("Timer Settings")]
    public DateTime currentHour;
    public float blend;
    [SerializeField] private float timeMultiplier;
    [SerializeField] private TextMeshProUGUI timeText;
    
    [Header("LightSpecs")]
    [SerializeField]
    private SunLight sunLight;
    private GameObject[] lightsObjects;
    private Volume dayPostProcessing, nightPostProcessing;
    [SerializeField] Material skyBoxMaterial;
    
    [Header("Save")]
    public bool tutorialCompleted;
    public List<string> bossesDefeated;

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
        player = GameObject.FindGameObjectWithTag("Player");
        InitiateTime();
    }
    public void StartGame() {
        Debug.Log("Game Started");
        isGameStarted = true;
        //Play Player out of stopChest(nao sei o nome de parapeito)
        //PlayerController.instance.PlayStartAnimation();
        //Unload NewMenu Scene
        SceneController.instance.UnloadMenu();
        //Load Gameplay scenes
        SceneController.instance.LoadGameplayScenes();
        //Começa as animaçoes do jogo
        StartCoroutine(PlayAnimation());
    }
    public void InitiateTime() {
        //Pegar o horario do save game
        currentHour = DateTime.Now.Date + TimeSpan.FromHours(13);
        sunLight.sunriseTime = TimeSpan.FromHours(sunLight.sunriseHour);
        sunLight.sunsetTime = TimeSpan.FromHours(sunLight.sunsetHour);
        lightsObjects = GameObject.FindGameObjectsWithTag("LightObject");
    }
    public void RestartGame()
    {
        isGameStarted = false;
        essentialsCanvas.SetActive(false);
        InitiateTime();

        //PlayerMovement.instance.CAMERADOPLAYER.SetActive(false);
        CameraFollow.follow.gameObject.SetActive(true);
    }
    private void Update() {
        UpdateTimeOfDay();
        UpdateLightSettings();
        RotateSun();
        GetTimeOfDayValue();
    }
    private void UpdateTimeOfDay() {
        currentHour = currentHour.AddSeconds(Time.deltaTime * timeMultiplier);
        if (currentHour.Hour == 18) {
            foreach (var light in lightsObjects) {
                light.SetActive(true);
            }
        }else if (currentHour.Hour >= 6 && currentHour.Hour <= 17) {
            foreach (var light in lightsObjects) {
                light.SetActive(false);
            }
        }
        if (currentHour.Hour >= 24) {
            currentHour = currentHour.Date + TimeSpan.FromHours(0);
        }
        timeText.text = currentHour.ToString("HH:mm");
    }
    private void RotateSun()
    {
        float sunLightRotation;
        if (currentHour.TimeOfDay > sunLight.sunriseTime && currentHour.TimeOfDay < sunLight.sunsetTime) {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDiference(sunLight.sunriseTime, sunLight.sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDiference(sunLight.sunriseTime, currentHour.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
            
        }else {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDiference(sunLight.sunsetTime, sunLight.sunriseTime);
            TimeSpan timeSinseSunset = CalculateTimeDiference(sunLight.sunsetTime, currentHour.TimeOfDay);

            double percentage = timeSinseSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }
        sunLight.sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation,Vector3.right);
    }

    public void GetTimeOfDayValue() {
       //set a float to total seconds of a day
       float totalSeconds = 79200;
       //set a float to current seconds of the day
       float currentSeconds = (float)currentHour.TimeOfDay.TotalSeconds;
       //set a float to the current seconds of the sunrise
       float sunriseSeconds = (float)sunLight.sunriseTime.TotalSeconds;
       float range = currentSeconds - sunriseSeconds;
       float diference = totalSeconds - sunriseSeconds;
       float percentage = range / diference;
       blend = percentage;
    }
    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.sunLight.transform.forward, Vector3.down);
        sunLight.sunLight.intensity = Mathf.Lerp(0, sunLight.maxSunLightIntesity, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
        sunLight.moonLight.intensity = Mathf.Lerp(sunLight.maxMoonLightIntesity, 0, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
        //changes the ambient light to the min in start hour and max in mid sunset hour
        RenderSettings.ambientIntensity = Mathf.Lerp(0.3f, 1, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
        skyBoxMaterial.SetFloat("_Blend", blend);
    }
    private TimeSpan CalculateTimeDiference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;
        if (difference.TotalSeconds < 0) {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }
    public IEnumerator PlayAnimation() {
        CameraFollow.follow.Started();
        yield return new WaitForSeconds(2);
        CameraFollow.follow.gameObject.transform.parent = PlayerCameraMovement.instance.transform;
        CameraFollow.follow.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        if (!tutorialCompleted) {
            PlayerMovement.instance.TeleportPlayer(GameObject.Find("TutorialTeleport").transform.position);
            yield return new WaitForSeconds(3);
            essentialsCanvas.SetActive(true);
            GameObject tonhao = GameObject.Find("Tonhao");
            tonhao.GetComponent<NPC>().Interact();
        }
        yield return new WaitForSeconds(3);
        Debug.LogWarning("FINALIZADO COROUTINE");
        essentialsCanvas.SetActive(true);
        if(!tutorialCompleted)
            PlayerCameraMovement.instance.ToggleAimLock(false);
        else
            PlayerCameraMovement.instance.ToggleAimLock(true);
        /*
        CameraFollow.follow.gameObject.transform.position = new Vector3(401, 153, -205);
        CameraFollow.follow.gameObject.transform.rotation = Quaternion.Euler(0, 21, 0);
        */
        CameraFollow.follow.gameObject.SetActive(false);
    }
}
[Serializable]
public class SunLight
{
    [Header("Light")]
    public Light sunLight;
    public float maxSunLightIntesity;

    [Header("Sunrise")]
    public float sunriseHour;
    public TimeSpan sunriseTime;
    [Header("Sunset")]
    public float sunsetHour;
    public TimeSpan sunsetTime;
    [Header("Moon")]
    public Light moonLight;
    public float maxMoonLightIntesity;
    [Header("Animation")]
    public LightAnimation lightAnimation;
    public GameObject clock;
}
[Serializable] 
public class LightAnimation
{
    [Header("LightAnimation")]
    public AnimationCurve lightChangeCurve;
    public Color dayAmbientLight;
    public Color nightAmbientLight;
}
