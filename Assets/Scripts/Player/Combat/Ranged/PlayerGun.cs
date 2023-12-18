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
    [SerializeField] private Image reloadImage;
    [SerializeField] private GameObject effect;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ammo = equipedWeapon.ammo;
        UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
    }

    private void Update()
    {
        Shoot();
        Reload();
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) == true && isReloading == false && DialogueManager.instance.isPlayingDialogue == false)
        {
            if (ammo > 0)
            {
                PlayerCamera.instance.playerAnimator.SetLayerWeight(1, 1);
                PlayerCamera.instance.playerAnimator.SetFloat("AimVertical", -PlayerCamera.instance.cameraBody.transform.eulerAngles.x / 60);
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
                            if (Input.GetMouseButton(0) == true)
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
        else if (PlayerCamera.instance.isAiming == false)
        {
            if(isReloading == false)
            {
                PlayerCamera.instance.playerAnimator.SetLayerWeight(1, 0);
            }
            PlayerCamera.instance.playerAnimator.SetFloat("AimVertical", 0);
        }
    }

    private void Reload()
    {
        if (Input.GetKeyDown(InputController.instance.reloadGun) == true && DialogueManager.instance.isPlayingDialogue == false)
        {
            StartCoroutine(ReloadAction());
        }
    }

    private void SummonBullets()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector3 target = Vector3.zero;
        Ray cameraRay = PlayerCamera.instance.cameraBody.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(cameraRay, out RaycastHit hitPos, 999, aimCollisionLayer) == true)
        {
            target = hitPos.point;
        }
        else
        {
            return;
        }

        for (int i = 0; i < equipedWeapon.projectileAmmount; i++)
        {
            float aux = Random.Range(-equipedWeapon.missfireRadius * 1.00f, (equipedWeapon.missfireRadius * 1.00f) + 0.01f);
            Vector3 startingPos = new Vector3(transform.position.x + aux, transform.position.y + aux, transform.position.z);
            Vector3 targetAim = (target - transform.position).normalized;

            Instantiate(equipedWeapon.projectile, startingPos, Quaternion.LookRotation(targetAim, Vector3.up), null);
            Instantiate(effect, startingPos, Quaternion.LookRotation(targetAim, Vector3.up), transform);
            //PlayerCamera.instance.ShakeCamera(equipedWeapon.recoil);
            ammo--;
        }


        shootCD = equipedWeapon.fireRate + Time.time;
        UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
    }

    private IEnumerator ReloadAction()
    {
        isReloading = true;
        PlayerCamera.instance.playerAnimator.SetLayerWeight(1, 1);
        PlayerCamera.instance.playerAnimator.Play("Reload");
        yield return new WaitForSeconds(equipedWeapon.reloadTime);
        PlayerCamera.instance.playerAnimator.SetLayerWeight(1, 0);

        isReloading = false;
        ammo = equipedWeapon.ammo;
        UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
    }
}
