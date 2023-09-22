using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour
{
    public static PlayerGun instance;

    [Header("Variables")]
    [SerializeField] private float cameraOffset;
    [SerializeField] private int ammo;
    private float shootCD = 0;
    private bool isReloading = false;
    [SerializeField] private LayerMask aimCollisionLayer = new LayerMask();

    [Header("References")]
    [SerializeField] private PlayerGunBase equipedWeapon;
    [SerializeField] private Text ammoText;
    [SerializeField] private Image reloadImage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ammo = equipedWeapon.ammo;
        ammoText.text = $"{ammo}/{equipedWeapon.ammo}";
    }

    private void Update()
    {
        Shoot();
        Reload();
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) == true && isReloading == false /*E não está em diálogo ou pausado*/)
        {
            if (ammo > 0)
            {
                if (Input.GetMouseButton(1) == false)
                {
                    PlayerCamera.instance.AlignRotation(PlayerCamera.instance.cameraBody.gameObject);
                }

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
            else
            {
                StartCoroutine(ReloadAction());
            }
        }
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) == true /*E não está em diálogo ou pausado*/)
        {
            StartCoroutine(ReloadAction());
        }
    }

    private void SummonBullets()
    {
        
        for (int i = 0; i < equipedWeapon.projectileAmmount; i++)
        {
            float aux = Random.Range(-equipedWeapon.missfireRadius * 1.00f, (equipedWeapon.missfireRadius * 1.00f) + 0.01f);
            Vector3 startinPos = new Vector3(transform.position.x + aux, transform.position.y + aux, transform.position.z);

            ammo--;
            //Instantiate(equipedWeapon.projectile, startinPos, PlayerCamera.instance.aimObject.transform.rotation);
        }


        //Sacudir camera com recoil
        shootCD = equipedWeapon.fireRate + Time.time;
        ammoText.text = $"{ammo}/{equipedWeapon.ammo}";
    }

    private IEnumerator ReloadAction()
    {
        isReloading = true;

        yield return new WaitForSeconds(equipedWeapon.reloadTime);

        isReloading = false;
        ammo = equipedWeapon.ammo;
        ammoText.text = $"{ammo}/{equipedWeapon.ammo}";
    }
}
