using UnityEngine;

public class BoitataDamageReceiver : MonoBehaviour
{
    public EnemyBehaviour user;
    
    public void TakeDamage(float damage, DamageElementManager.DamageElement damageElement) {
        user.TakeDamage(damage,DamageElementManager.DamageElement.Physical);
    }

}
