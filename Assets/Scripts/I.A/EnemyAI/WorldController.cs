using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
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
    
    [Header("Timer Settings")]
    private DateTime currentHour; 
    [SerializeField] private float timeMultiplier;
    [SerializeField] private TextMeshProUGUI timeText;
    
    [Header("LightSpecs")]
    [SerializeField]
    private SunLight sunLight;
    private List<GameObject> lightsObjects = new List<GameObject>();
    private Volume dayPostProcessing, nightPostProcessing;
    
    private void Start()
    {
        if (worldController == null) {
            worldController = this;
            DontDestroyOnLoad(gameObject);
        }else {
            Destroy(gameObject);
        }
        SceneManager.LoadSceneAsync("NewMenu", LoadSceneMode.Additive);
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
        InitiateTime();
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
    private void InitiateTime() {
        //Pegar o horario do save game
        currentHour = DateTime.Now.Date + TimeSpan.FromHours(13);
        sunLight.sunriseTime = TimeSpan.FromHours(sunLight.sunriseHour);
        sunLight.sunsetTime = TimeSpan.FromHours(sunLight.sunsetHour);
        lightsObjects.AddRange(GameObject.FindGameObjectsWithTag("LightObject"));
    }
    private void Update() {
        if (playingAnimation) {
            camera.transform.position = Vector3.Lerp(camera.transform.position, cameraTransform.position, Time.deltaTime * 2);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraTransform.rotation, Time.deltaTime * 2);
        }
        UpdateTimeOfDay();
        UpdateLightSettings();
        RotateSun();
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
    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.sunLight.transform.forward, Vector3.down);
        sunLight.sunLight.intensity = Mathf.Lerp(0, sunLight.maxSunLightIntesity, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
        sunLight.moonLight.intensity = Mathf.Lerp(sunLight.maxMoonLightIntesity, 0, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(sunLight.lightAnimation.nightAmbientLight, sunLight.lightAnimation.dayAmbientLight, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
        //changes the ambient intensity to the min in start hour and max in mid sunset hour
        RenderSettings.ambientIntensity = Mathf.Lerp(.32f, 2.74f, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
        //lerp the volume of the day post processing to night post proccessing between day and night
        if(dayPostProcessing !=null)
            dayPostProcessing.weight = Mathf.Lerp(1, 0, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
        if(nightPostProcessing !=null)
            nightPostProcessing.weight = Mathf.Lerp(0, 1, sunLight.lightAnimation.lightChangeCurve.Evaluate(dotProduct));
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
        yield return new WaitForSeconds(3);
        playingAnimation = true;
        yield return new WaitForSeconds(3);
        brain.enabled = true;
        playingAnimation = false;
        PlayerCamera.instance.ToggleAimLock(true);
        essentialsCanvas.SetActive(true);
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
