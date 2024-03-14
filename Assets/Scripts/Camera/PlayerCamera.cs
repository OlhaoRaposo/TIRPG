using System.Collections;
using Cinemachine;
using UnityEngine;

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
    [SerializeField] private float regularFov;
    [SerializeField] private float aimFov;
    public bool locked;

    public bool isAiming;
    private float camSpeedX, camSpeedY;

    [Header("References")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerAim;
    [SerializeField] private CinemachineBrain myCinemachineBrain;

    private CinemachineFreeLook myCinemachineCamera;
    private Vector3 startingPos, startingAimPos;
    [HideInInspector] public Camera cameraBody;
    [HideInInspector] public Animator playerAnimator;

    private void Awake()
    {
        instance = this;

        myCinemachineCamera = gameObject.GetComponent<CinemachineFreeLook>();
        cameraBody = gameObject.GetComponentInChildren<Camera>();
        playerAnimator = playerObject.GetComponent<Animator>();
    }

    private void Start()
    {
        myCinemachineCamera.Follow = playerObject.transform;
        myCinemachineCamera.LookAt = playerAim.transform;
        startingPos = transform.position;
        startingAimPos = playerAim.transform.localPosition;

        camSpeedX = myCinemachineCamera.m_XAxis.m_MaxSpeed;
        camSpeedY = myCinemachineCamera.m_YAxis.m_MaxSpeed;

        SetCameraOrbit(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetValues(float cameraSense, float aimSense, float zoomStrength, bool isInverted)
    {
        this.cameraSense = cameraSense;
        this.aimSense = aimSense;
        this.zoomStrength = zoomStrength;
        this.isInverted = isInverted;

        SetCurrentSense(cameraSense);
        if (isInverted == true)
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
        SetCameraZoom();
        //CursorLockControl();
    }

    public void LockCamera(bool lockCamera)
    {
        if (lockCamera == true)
        {
            myCinemachineCamera.m_XAxis.m_MaxSpeed = 0;
            myCinemachineCamera.m_YAxis.m_MaxSpeed = 0;
        }else {
            myCinemachineCamera.m_XAxis.m_MaxSpeed = camSpeedX;
            myCinemachineCamera.m_YAxis.m_MaxSpeed = camSpeedY;
        }
    }
    private void Aim()
    {
        if (Input.GetMouseButtonDown(1) == true)
        {
            playerAnimator.SetLayerWeight(1, 1);
            playerAnimator.SetFloat("AimVertical", -cameraBody.transform.eulerAngles.x / 60);

            SetCameraOrbit(true);
        }

        if (isAiming == true)
        {
            AlignRotation(cameraBody.gameObject);
        }

        if (Input.GetMouseButtonUp(1) == true)
        {
            playerAnimator.SetLayerWeight(1, 0);
            playerAnimator.SetFloat("AimVertical", 0);

            SetCameraOrbit(false);
        }

        if (Input.GetMouseButtonDown(2) == true)
        {
            transform.position = startingPos;
        }

    }

    private void SetCameraOrbit(bool isAiming)
    {
        if (isAiming == false)
        {
            SetCurrentSense(cameraSense);
            myCinemachineCamera.m_Lens.FieldOfView = regularFov;
        }
        else
        {
            SetCurrentSense(aimSense);
            myCinemachineCamera.m_Lens.FieldOfView = aimFov;
        }
        this.isAiming = isAiming;
    }

    public void AlignRotation(GameObject target)
    {
        playerObject.transform.rotation = Quaternion.Euler(0, target.transform.eulerAngles.y, 0);
    }

    public Vector3 GetCameraForward()
    {
        return new Vector3(cameraBody.transform.forward.x, 0, cameraBody.transform.forward.z).normalized;
    }

    public Vector3 GetCameraRight()
    {
        return new Vector3(cameraBody.transform.right.x, 0, cameraBody.transform.right.z).normalized;
    }

    private void SetCurrentSense(float sense)
    {
        currentSense = sense;
        myCinemachineCamera.m_XAxis.m_MaxSpeed = currentSense * 600;
        myCinemachineCamera.m_YAxis.m_MaxSpeed = currentSense * 4;
    }

    public void ShakeCamera(float strength)
    {

    }

    private void SetCameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if(myCinemachineCamera.m_Lens.FieldOfView > aimFov)
                {
                    myCinemachineCamera.m_Lens.FieldOfView -= zoomStrength;
                }
            }
            else
            {
                if(myCinemachineCamera.m_Lens.FieldOfView < regularFov)
                {
                    myCinemachineCamera.m_Lens.FieldOfView += zoomStrength;
                }
            }
        }
    }

    private void CursorLockControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) == true)
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                ToggleAimLock(true);
            }
            else
            {
                ToggleAimLock(false);
            }
        }
    }

    public void ToggleAimLock(bool toggle)
    {
        if (toggle == true)
        {
            myCinemachineCamera.m_XAxis.m_MaxSpeed = camSpeedX;
            myCinemachineCamera.m_YAxis.m_MaxSpeed = camSpeedY;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            myCinemachineCamera.m_XAxis.m_MaxSpeed = 0;
            myCinemachineCamera.m_YAxis.m_MaxSpeed = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ToggleMovement(bool toggle)
    {
        if (toggle == true)
        {
            myCinemachineCamera.m_XAxis.m_MaxSpeed = camSpeedX;
            myCinemachineCamera.m_YAxis.m_MaxSpeed = camSpeedY;
        }
        else
        {
            myCinemachineCamera.m_XAxis.m_MaxSpeed = 0;
            myCinemachineCamera.m_YAxis.m_MaxSpeed = 0;
        }
    }
}
