using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Player/Ranged Weapon")]
public class PlayerGunBase : ScriptableObject
{
    public int ammo;
    public float reloadTime;
    public float fireRate;
    public enum TriggerType { Auto, Semi, Hold }
    public TriggerType triggerType;
    public int projectileAmmount;
    public float missfireRadius;
    public float recoil;
    public GameObject projectile;
}
