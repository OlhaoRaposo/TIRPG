using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Interactable_Generator : MonoBehaviour, IInteractable
{
    AudioSource audioGenerator;
    AudioSource audioHelice;

    [SerializeField] Transform axis;
    bool isRotating = false;
    float t = 0;
    [SerializeField] float duration = 3f;
    [SerializeField] float speed = 1;

    void Start()
    {
        audioGenerator = GetComponent<AudioSource>();
        audioHelice = axis.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (isRotating){
            t += Time.fixedDeltaTime / duration;
            Mathf.Clamp01(t);
            axis.Rotate(0, 0, t * speed);
        }
    }
    public void Interact(PlayerInteractions player)
    {
        isRotating = true;
        audioGenerator?.Play();
        audioHelice?.Play();
        SceneStatesController.instance.SetGeneratorState(true);
    }
}
