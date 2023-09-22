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

    private bool isAiming;

    [Header("References")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject aimObject;

    private CinemachineFreeLook myCinemachineCamera;
    [HideInInspector] public Camera cameraBody;

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
    }

    private void Aim()
    {
        if (Input.GetMouseButtonDown(1) == true)
        {
            SetCameraOrbit(true);
        }

        if (isAiming == true)
        {
            AlignRotation();
        }

        if (Input.GetMouseButtonUp(1) == true)
        {
            SetCameraOrbit(false);
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

    public void AlignRotation()
    {
        playerBody.transform.rotation = Quaternion.Euler(0, cameraBody.gameObject.transform.eulerAngles.y, 0);
        playerObject.transform.rotation = Quaternion.Euler(0, cameraBody.gameObject.transform.eulerAngles.y, 0);
        aimObject.transform.rotation = Quaternion.Euler(cameraBody.gameObject.transform.eulerAngles.x, cameraBody.gameObject.transform.eulerAngles.y, 0);
    }

    private void SetCurrentSense(float sense)
    {
        currentSense = sense;
        myCinemachineCamera.m_XAxis.m_MaxSpeed = currentSense * 600;
        myCinemachineCamera.m_YAxis.m_MaxSpeed = currentSense * 4;
    }

    public void ShakeCamera(float strength, float duration)
    {

    }
}
