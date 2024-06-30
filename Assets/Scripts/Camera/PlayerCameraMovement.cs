using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    public static PlayerCameraMovement instance;

    [Header("Settings")]
    [SerializeField] private float cameraSense;
    [SerializeField] private float aimSense;
    [SerializeField] private bool isInverted;

    [Header("Variables")]
    [SerializeField] private float currentSense;
    [SerializeField] private float regularFov;
    [SerializeField] private float aimFov;
    public float cameraVerticalClamping;
    public bool locked;
    public bool isAiming;
    private float camSpeedX, camSpeedY;
    private Vector3 previousCamRotation, previousCamPosition;

    [Header("References")]
    public GameObject playerObject;
    public GameObject playerModel;
    private Vector3 startingPos;
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
        startingPos = transform.localPosition;
        previousCamPosition = startingPos;
        transform.position = startingPos;
    }

    private void OnEnable()
    {

    }

    public void SetValues(float cameraSense, float aimSense, bool isInverted)
    {
        this.cameraSense = cameraSense;
        this.aimSense = aimSense;
        this.isInverted = isInverted;

        SetCurrentSense(cameraSense);
    }

    private void Update()
    {
        if (WorldController.worldController.isGameStarted)
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
        float x = Input.GetAxis("Mouse X") * camSpeedX;
        float y = Input.GetAxis("Mouse Y") * camSpeedY;

        if (x != 0 || y != 0)
        {
            transform.RotateAround(playerModel.transform.position, Vector3.up, x);
            transform.RotateAround(playerModel.transform.position, -transform.right, y);
        }

        if (transform.localEulerAngles != previousCamRotation && transform.localPosition != previousCamPosition)
        {
            //IMPEDIR QUE A CÂMERA FIQUE INVERTIDA, GAMBIARRA POR QUE ESSA MERDA DE ÂNGULO NN FUNCIONA DIREITO POR NADA
            if (transform.eulerAngles.x < 180)
            {
                if (Mathf.Abs(transform.eulerAngles.x) > cameraVerticalClamping * 2)
                {
                    transform.localPosition = previousCamPosition;
                    transform.localEulerAngles = previousCamRotation;
                }
                else
                {
                    previousCamPosition = transform.localPosition;
                    previousCamRotation = transform.localEulerAngles;
                }
            }
            else if (transform.eulerAngles.x > 180)
            {
                if (360 - Mathf.Abs(transform.eulerAngles.x) > cameraVerticalClamping)
                {
                    transform.localPosition = previousCamPosition;
                    transform.localEulerAngles = previousCamRotation;
                }
                else
                {
                    previousCamPosition = transform.localPosition;
                    previousCamRotation = transform.localEulerAngles;
                }
            }

        }
    }

    private void PlayerAim()
    {
        if (Input.GetMouseButton(1) == true && UIManager.instance.GetIsInMenus() == false)
        {
            AlignTargetWithCamera(playerObject);
            SetCurrentSense(aimSense);
            cameraBody.fieldOfView = aimFov;

            if (PlayerGun.instance.enabled == true && PlayerGun.instance.isReloading == false)
            {
                playerAnimator.SetLayerWeight(1, 1);
                if (PlayerGun.instance.equipedWeapon.triggerType != PlayerGunBase.TriggerType.Hold)
                {
                    playerAnimator.Play($"{PlayerGun.instance.GetGunName()} Aim Tree");
                }
            }
            isAiming = true;
        }
        else
        {
            SetCurrentSense(cameraSense);
            cameraBody.fieldOfView = regularFov;
        }

        if (Input.GetMouseButtonUp(1) == true && PlayerGun.instance.enabled == true && PlayerGun.instance.isReloading == false)
        {
            playerAnimator.SetLayerWeight(1, 0);
        }
        isAiming = false;
    }

    public void AlignTargetWithCamera(GameObject target)
    {
        transform.SetParent(null);
        target.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        transform.SetParent(playerObject.transform);
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
        currentSense = sense;
    }

    public void ShakeCamera(float strength)
    {

    }

    private void SetCameraZoom()
    {
        if (Vector3.Distance(cameraBody.transform.position, playerObject.transform.position) > 3)
        {
            transform.localPosition = startingPos;
            transform.localEulerAngles = Vector3.zero;
        }
    }

    //TRAVAR CÂMERA POR QUALQUER MOTIVO QUE SEJA E LIBERAR O CURSOR TAMBÉM POR QUALQUER MOTIVO Q SEJA INGAME
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

    //TRAVAR CÂMERA POR QUALQUER MOTIVO QUE SEJA E LIBERAR O CURSOR TAMBÉM POR QUALQUER MOTIVO Q SEJA, CHAME ESSA FUNÇÃO NA TELA INICIAL DANIEL -->
    public void ToggleAimLock(bool toggle)
    {
        if (toggle == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            camSpeedX = currentSense;
            camSpeedY = currentSense * 0.7f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            camSpeedX = 0;
            camSpeedY = 0;
        }
        UIManager.instance.SetCursorLockState(toggle);
    }
}


