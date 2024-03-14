using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject interactCanvas;
    [SerializeField]
    private GameObject cameraReference;
    
    public bool canInteract = true;
    
    private void Update() {
        CheckInteractions();
    }

    private void CheckInteractions()
    {
        
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray cameraRay = PlayerCamera.instance.cameraBody.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(cameraRay, out RaycastHit hit, 5)) {
            if (hit.collider.gameObject.CompareTag("Interactable")) {
                if (hit.distance <= 15) {
                    interactCanvas.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E)) {
                        if (canInteract) {
                            hit.collider.gameObject.SendMessage("Interact");
                            if (hit.collider.gameObject.TryGetComponent(out InteractDetection interact)){
                               interact.OnInteract();
                            }
                        }
                        canInteract = false;
                        Debug.Log("Interacted");
                    }
                    if (Input.GetKeyUp(KeyCode.E)) {
                        canInteract = true;
                    }
                }else {
                    interactCanvas.SetActive(false);
                }
            }else {
                interactCanvas.SetActive(false);
            }
        }
       
    }
}
