using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public static CameraFollow follow;

    private void Start() {

        if (follow != null) return;

        follow = this;
    }

    public void Started() {
      
    }
}