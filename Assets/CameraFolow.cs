using System;
using UnityEngine;

public class CameraFolow : MonoBehaviour {
    public static CameraFolow follow;

    private void Start() {
        follow = this;
    }

    public void Started() {
       if(this.TryGetComponent(out Animator anim))
            anim.SetTrigger("_started");
    }
}