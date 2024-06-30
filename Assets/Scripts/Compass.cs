using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
   [SerializeField] private GameObject playerObj;
    public RawImage compassDirection;
    public float lerpSpeed;

    void Update() {
        do {
            playerObj = PlayerCameraMovement.instance.gameObject;
        } while (playerObj == null);
        compassDirection.uvRect = new Rect(playerObj.transform.eulerAngles.y / 360f, 0, 1f, 1f);
    }
}