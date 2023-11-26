using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class PlayerAim : MonoBehaviour
{
    [Header("Settings")]
    public float MouseSens = 100f;
    public float minAngle, MaxAngle;
    [Header("References")]
    public Transform player;
    private float xRotation = 0f;

    [Header("Interactions")]
    public bool isLookANote;
    public GameObject atualUiNote;
    [SerializeField]
    private GameObject interactCanvas;
    [SerializeField]
    Vector3 endRotation;


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        AimMove();
        CheckInteractions();
    }

    private void AimMove()
    {
        if (isLookANote)
        {
            Cursor.lockState = CursorLockMode.None;
            GameObject childObject = atualUiNote.gameObject.transform.GetChild(1).gameObject;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                endRotation = childObject.transform.rotation.eulerAngles;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                endRotation.x += mouseY * -MouseSens / 3;
                endRotation.y += mouseX * MouseSens / 3;

                endRotation.y = Mathf.Clamp(endRotation.y, -45, 45);
                endRotation.x = Mathf.Clamp(endRotation.x, -45, 45);

                Quaternion targetRotation = Quaternion.Euler(endRotation);
                childObject.transform.rotation = Quaternion.Lerp(childObject.transform.rotation, targetRotation, Time.deltaTime * 2);
            }
        }else {
            Cursor.lockState = CursorLockMode.Locked;
            float mouseX = Input.GetAxis("Mouse X") * MouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * MouseSens * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, minAngle, MaxAngle);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }

    private void CheckInteractions()
    {
        if(Physics.Raycast(Camera.main.transform.position, transform.forward, out RaycastHit hit, 25)){
            
            if (hit.collider.gameObject.CompareTag("Interactable")) {
                interactCanvas.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) {
                    hit.collider.gameObject.SendMessage("Interact");
                }
            }else {
                interactCanvas.SetActive(false);
            }
        }
       
    }
}
