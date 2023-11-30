using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionCollision : MonoBehaviour
{
    public string sceneToLoad;
    [SerializeField]
    Vector3 playerPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EssentialObjects.instance.player.transform.position = playerPosition;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}