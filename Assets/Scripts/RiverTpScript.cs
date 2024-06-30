using UnityEngine;

public class RiverTpScript : MonoBehaviour
{
    public NPC deathTalk;
    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.transform.parent != null)
            if (col.gameObject.transform.parent.TryGetComponent(out PlayerMovement pm)) {
                PlayerMovement.instance.TeleportPlayer(new Vector3(695.79f, 62.19f, 180.25f));
                deathTalk.Interact();
            }
            
    }
}
