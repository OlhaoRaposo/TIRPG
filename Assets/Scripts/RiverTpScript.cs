using UnityEngine;

public class RiverTpScript : MonoBehaviour
{
    public NPC deathTalk;
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out PlayerMovement pm)) {
            pm.TeleportPlayer(new Vector3(695.79f, 62.19f, 180.25f));
            deathTalk.Interact();
        }
        
    }
}
