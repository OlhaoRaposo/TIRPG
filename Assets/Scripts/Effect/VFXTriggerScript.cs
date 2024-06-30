using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTriggerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] effectsParent;
    private void Start()
    {
        GameObject parentObject = this.gameObject;

        int childCount = parentObject.transform.childCount;

        effectsParent = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            effectsParent[i] = parentObject.transform.GetChild(i).gameObject;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entrou");
            for (int j = 0; j < effectsParent.Length; j++)
            {
                effectsParent[j].SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Saiu");
            for (int j = 0; j < effectsParent.Length; j++)
            {
                effectsParent[j].SetActive(false);
            }
        }
    }
}
