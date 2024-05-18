using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitDestroy : MonoBehaviour
{
    [SerializeField] private float destroy;
    void Start() {
        StartCoroutine(DestroyOnSeconds());
    }

    private IEnumerator DestroyOnSeconds() {
        yield return new WaitForSeconds(destroy);
        Destroy(this);
    }
}
