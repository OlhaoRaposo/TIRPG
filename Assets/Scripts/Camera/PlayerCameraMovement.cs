using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    public static PlayerCameraMovement instance;

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

    private Vector3 startingPos, startingAimPos;
    [HideInInspector] public Camera cameraBody;
    [HideInInspector] public Animator playerAnimator;

    private void Awake()
    {
        instance = this;

        cameraBody = gameObject.GetComponentInChildren<Camera>();
        playerAnimator = playerObject.GetComponent<Animator>();
    }

    private void Start()
    {
        startingPos = transform.position;
        startingAimPos = playerAim.transform.localPosition;

        camSpeedX = currentSense;
        camSpeedY = currentSense * 0.7f;
    }

    public void SetValues(float cameraSense, float aimSense, float zoomStrength, bool isInverted)
    {
        this.cameraSense = cameraSense;
        this.aimSense = aimSense;
        this.zoomStrength = zoomStrength;
        this.isInverted = isInverted;

        SetCurrentSense(cameraSense);
    }

    private void Update()
    {
        if (true /*BIRIGUINAIT DE TER INICIADO O JOGO*/)
        {
            CameraMove();
            PlayerAim();
            SetCameraZoom();
            CursorLockControl();
        }
    }

    //POSIÇÃO E MOVIMENTO
    private void CameraMove()
    {

    }

    private void PlayerAim()
    {
        
    }
    public void AlignTargetWithCamera(GameObject target)
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

    //EFEITOS, SENSIBILIDADE & UTILIDADE
    private void SetCurrentSense(float sense)
    {
        
    }

    public void ShakeCamera(float strength)
    {

    }

    private void SetCameraZoom()
    {
        
    }

    //TRAVAR CÂMERA POR QUALQUER MOTIVO QUE SEJA E LIBERAR O CURSOR TAMBÉM POR QUALQUER MOTIVO Q SEJA
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
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        ToggleCameraMovement(toggle);
    }

    public void ToggleCameraMovement(bool toggle)
    {
        
    }
}
