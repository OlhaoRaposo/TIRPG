using UnityEngine;

public class TransitionCollision : MonoBehaviour
{
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //SceneController.instance.LoadSceneByName(sceneToLoad);
        }
    }
}