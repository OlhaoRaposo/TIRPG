using UnityEngine;

public class PlayerTesting : MonoBehaviour
{
    public GameObject ranged, aim, melee;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ranged.SetActive(true);
            aim.SetActive(true);
            melee.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ranged.SetActive(false);
            aim.SetActive(false);
            melee.SetActive(true);
        }
    }
}
