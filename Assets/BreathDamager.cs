using UnityEngine;

public class BreathDamager : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerHPController.instance.ChangeHP(damage, true);
            Destroy(gameObject);
        }
    }
}
