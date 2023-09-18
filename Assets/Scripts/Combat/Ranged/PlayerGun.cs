using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public static PlayerGun instance;

    [Header("Variables")]
    [SerializeField] private float cameraOffset;
    private float shootCD = 0;

    [Header("References")]
    [SerializeField] private PlayerGunBase equipedWeapon;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) == true /*E não está em diálogo ou pausado*/)
        {
            switch (equipedWeapon.triggerType)
            {

                case PlayerGunBase.TriggerType.Auto:
                    {
                        if (shootCD < Time.time)
                        {
                            SummonBullets();
                        }
                        break;
                    }
                case PlayerGunBase.TriggerType.Semi:
                    {
                        if (Input.GetMouseButtonDown(0) == true)
                        {
                            if (shootCD < Time.time)
                            {
                                SummonBullets();
                            }
                        }
                        break;
                    }
                case PlayerGunBase.TriggerType.Hold:
                    {
                        float aux = 0;
                        if (Input.GetMouseButtonUp(0) == true)
                        {
                            if (equipedWeapon.fireRate >= aux + Time.time)
                            {
                                SummonBullets();
                            }
                            else
                            {
                                SummonBullets();
                            }
                        }
                        break;
                    }
            }
        }
    }

    private void SummonBullets()
    {
        for (int i = 0; i < equipedWeapon.projectileAmmount; i++)
        {
            float aux = Random.Range(-equipedWeapon.missfireRadius * 1.00f, (equipedWeapon.missfireRadius * 1.00f) + 0.01f);
            Vector3 startinPos = new Vector3(transform.position.x + aux, transform.position.y + aux, transform.position.z);

            PlayerProjectile currentProjectile = Instantiate(equipedWeapon.projectile, startinPos, transform.rotation).GetComponent<PlayerProjectile>();
            currentProjectile.OverrideData(equipedWeapon.damage, equipedWeapon.range);
        }

        //Sacudir camera com recoil
        shootCD = equipedWeapon.fireRate + Time.time;
    }
}
