using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    [Header("Settings")]
    [SerializeField] private float cameraSense;
    [SerializeField] private float aimSense;
    [SerializeField] private float zoomStrength;
    [SerializeField] private bool isInverted;

    [Header("Variables")]
    [SerializeField] private float currentSense;
    [SerializeField] private float[] cameraHeightNormal = new float[3];
    [SerializeField] private float[] cameraRadiusNormal = new float[3];
    [SerializeField] private float[] cameraHeightAim = new float[3];
    [SerializeField] private float[] cameraRadiusAim = new float[3];

    private bool isAiming;

    [Header("References")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerBody;

    private CinemachineFreeLook myCinemachineCamera;
    private Camera cameraBody;

    private void Awake()
    {
        instance = this;

        myCinemachineCamera = gameObject.GetComponent<CinemachineFreeLook>();
        cameraBody = gameObject.GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        myCinemachineCamera.Follow = playerObject.transform;
        myCinemachineCamera.LookAt = playerObject.transform;

        SetCameraOrbit(false);
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetValues(float cameraSense, float aimSense, float zoomStrength, bool isInverted)
    {
        this.cameraSense = cameraSense;
        this.aimSense = aimSense;
        this.zoomStrength = zoomStrength;
        this.isInverted = isInverted;

        SetCurrentSense(cameraSense);
        if(isInverted == true)
        {
            myCinemachineCamera.m_YAxis.m_InvertInput = true;
        }
        else
        {
            myCinemachineCamera.m_YAxis.m_InvertInput = false;
        }
    }

    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        if (Input.GetMouseButtonDown(1) == true)
        {
            SetCameraOrbit(true);
        }

        if(isAiming == true)
        {
            playerBody.transform.rotation = Quaternion.Euler(0, cameraBody.gameObject.transform.eulerAngles.y, 0);
            playerObject.transform.rotation = Quaternion.Euler(0, cameraBody.gameObject.transform.eulerAngles.y, 0);
        }

        if (Input.GetMouseButtonUp(1) == true)
        {
            SetCameraOrbit(false);
        }

    }

    private void SetCameraOrbit(bool isAiming)
    {
        for (int i = 0; i < 3; i++)
        {
            if(isAiming == false)
            {
                SetCurrentSense(cameraSense);
                myCinemachineCamera.m_Orbits[i].m_Height = cameraHeightNormal[i];
                myCinemachineCamera.m_Orbits[i].m_Radius = cameraRadiusNormal[i];
            }
            else
            {
                SetCurrentSense(aimSense);
                myCinemachineCamera.m_Orbits[i].m_Height = cameraHeightAim[i];
                myCinemachineCamera.m_Orbits[i].m_Radius = cameraRadiusAim[i];
            }
        }
        this.isAiming = isAiming;
    }

    private void SetCurrentSense(float sense)
    {
        currentSense = sense;
        myCinemachineCamera.m_XAxis.m_MaxSpeed = currentSense * 600;
        myCinemachineCamera.m_YAxis.m_MaxSpeed = currentSense * 4;
    }
}
