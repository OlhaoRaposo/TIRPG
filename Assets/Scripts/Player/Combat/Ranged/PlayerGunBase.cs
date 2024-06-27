using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Player/Ranged Weapon")]
public class PlayerGunBase : ScriptableObject
{
    public string modelName;
    public string ammoName;
    public int ammo;
    public int projectileAmmount;
    public float reloadTime;
    public float damage;
    public DamageElementManager.DamageElement bulletElement = DamageElementManager.DamageElement.Physical;
    public float fireRate;
    public float maxRange;
    public enum TriggerType { Auto, Semi, Hold }
    public TriggerType triggerType;
    public float missfireRadius;
    public float recoil;
    public GameObject damageParticle;
}
