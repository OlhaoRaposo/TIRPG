using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour
{
    public static PlayerGun instance;

    [Header("Variables")]
    [SerializeField] private float cameraOffset;
    [SerializeField] private int ammo;
    [SerializeField] private string gunName;
    [SerializeField] private string gunMag;
    private float shootCD = 0;
    private bool isReloading = false, canShoot = true;
    [SerializeField] private LayerMask aimCollisionLayer = new LayerMask();

    [Header("References")]
    [SerializeField] private PlayerGunBase equipedWeapon;
    [SerializeField] private Image reloadImage;
    private float holdTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetNewGunWeapon(equipedWeapon);
    }

    private void Update()
    {
        Shoot();
        Reload();
    }

    public void SetNewGunWeapon(PlayerGunBase newWeapon)
    {
        equipedWeapon = newWeapon;
        ammo = equipedWeapon.ammo;
        gunName = newWeapon.modelName;
        gunMag = newWeapon.ammoName;
        UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
    }

    public string GetGunName()
    {
        return gunName;
    }

    private void Shoot()
    {
        if (isReloading == false && DialogueManager.instance.isPlayingDialogue == false && canShoot == true)
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
                            if (Input.GetMouseButton(0) == true && shootCD < Time.time)
                            {
                                PlayerCamera.instance.playerAnimator.SetLayerWeight(1, 1);
                                PlayerCamera.instance.playerAnimator.SetFloat("AimVertical", -PlayerCamera.instance.cameraBody.transform.eulerAngles.x / 60);
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
                                    PlayerCamera.instance.playerAnimator.SetLayerWeight(1, 1);
                                    PlayerCamera.instance.playerAnimator.SetFloat("AimVertical", -PlayerCamera.instance.cameraBody.transform.eulerAngles.x / 60);
                                    SummonBullets();
                                }
                            }
                            break;
                        }
                    case PlayerGunBase.TriggerType.Hold:
                        {
                            if (Input.GetMouseButton(0) == true)
                            {
                                PlayerCamera.instance.playerAnimator.SetLayerWeight(1, 1);
                                PlayerCamera.instance.playerAnimator.SetFloat("AimVertical", -PlayerCamera.instance.cameraBody.transform.eulerAngles.x / 60);
                                holdTime += Time.deltaTime;
                            }
                            //Soltar
                            if (Input.GetMouseButtonUp(0) == true)
                            {
                                SummonBullets();
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
            if (isReloading == false)
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
        if (Physics.Raycast(cameraRay, out RaycastHit hitPos, float.MaxValue, aimCollisionLayer) == true)
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

            GameObject projectile = Instantiate(equipedWeapon.projectile, startingPos, Quaternion.LookRotation(targetAim, Vector3.up), null);
            if (equipedWeapon.triggerType == PlayerGunBase.TriggerType.Hold)
            {
                if (holdTime / shootCD < 1)
                {
                    Debug.Log("Atirou mais fraco");
                    projectile.GetComponent<PlayerProjectile>().SetSpeed(holdTime);
                    holdTime = 0;
                }
            }
            Instantiate(equipedWeapon.effect, startingPos, Quaternion.LookRotation(targetAim, Vector3.up), transform);
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

    public void ShootToggle(bool toggle)
    {
        canShoot = toggle;
    }
}
