using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionCollision : MonoBehaviour
{
    public string sceneToLoad;

    //[SerializeField] Vector3 playerPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
            //EssentialObjects.instance.player.transform.position = playerPosition;
        }
    }
}