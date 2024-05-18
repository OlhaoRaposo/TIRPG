using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraSettings : MonoBehaviour
{
    [SerializeField] private Slider cameraSense, aimSense, zoomStrength;
    [SerializeField]private bool isInverted;

    private void Awake()
    {
        GetCameraValues();
    }

    private void Start()
    {
        SetCameraValues();
    }

    private void GetCameraValues()
    {
        cameraSense.value = PlayerPrefs.GetFloat("cameraSense");
        aimSense.value = PlayerPrefs.GetFloat("aimSense");
        zoomStrength.value = PlayerPrefs.GetFloat("zoomStrength");
        int aux = PlayerPrefs.GetInt("isInverted");
        if (aux == 0)
        {
            isInverted = false;
        }
        else
        {
            isInverted = true;
        }
    }

    public void SetCameraValues()
    {
        PlayerCameraMovement.instance.SetValues(cameraSense.value, aimSense.value, /*zoomStrength.value, */ isInverted);
        SaveCameraValues();
    }

    private void SaveCameraValues()
    {
        PlayerPrefs.SetFloat("cameraSense", cameraSense.value);
        PlayerPrefs.SetFloat("aimSense", aimSense.value);
        PlayerPrefs.SetFloat("zoomStrength", zoomStrength.value);
        if (isInverted == false)
        {
            PlayerPrefs.SetInt("isInverted", 0);
        }
        else
        {
            PlayerPrefs.SetInt("isInverted", 1);
        }
    }

    public void InvertCamera()
    {
        if(isInverted == false)
        {
            isInverted = true;
        }
        else
        {
            isInverted = false;
        }
    }
}
