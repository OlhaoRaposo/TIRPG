using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour
{
    public static PlayerGun instance;

    [Header("Variables")]
    [SerializeField] private int ammo;
    [SerializeField] private string gunName;
    [SerializeField] private string gunMag;
    private float shootCD = 0;
    public bool isReloading = false, canShoot = true;
    [SerializeField] private LayerMask aimCollisionLayer = new LayerMask();
    private float holdTime;
    [HideInInspector] public bool isHolding = false;

    [Header("References")]
    public PlayerGunBase equipedWeapon;
    public AudioBoard localBoard;
    [SerializeField] private Image reloadImage;

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
        if (WorldController.worldController.isGameStarted == true)
        {
            Shoot();
            Reload();
        }
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
        if (isReloading == false && canShoot == true && UIManager.instance.GetIsInMenus() == false)
        {
            if (ammo > 0)
            {
                switch (equipedWeapon.triggerType)
                {

                    case PlayerGunBase.TriggerType.Auto:
                        {
                            if (Input.GetMouseButton(0) == true && shootCD < Time.time)
                            {
                                PlayerCameraMovement.instance.playerAnimator.SetLayerWeight(1, 1);
                                PlayerCameraMovement.instance.playerAnimator.Play($"{gunName} Aim Tree");

                                if (PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x > 180)
                                {
                                    PlayerCameraMovement.instance.playerAnimator.SetFloat("AimVertical", (360 - PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x) / PlayerCameraMovement.instance.cameraVerticalClamping);
                                }
                                else if (PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x < 180)
                                {
                                    PlayerCameraMovement.instance.playerAnimator.SetFloat("AimVertical", -PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x / PlayerCameraMovement.instance.cameraVerticalClamping);
                                }

                                SummonBullets();
                            }
                            break;
                        }
                    case PlayerGunBase.TriggerType.Semi:
                        {
                            if (Input.GetMouseButtonDown(0) == true && shootCD < Time.time)
                            {
                                PlayerCameraMovement.instance.playerAnimator.SetLayerWeight(1, 1);
                                PlayerCameraMovement.instance.playerAnimator.Play($"{gunName} Aim Tree");

                                if (PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x > 180)
                                {
                                    PlayerCameraMovement.instance.playerAnimator.SetFloat("AimVertical", (360 - PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x) / PlayerCameraMovement.instance.cameraVerticalClamping);
                                }
                                else if (PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x < 180)
                                {
                                    PlayerCameraMovement.instance.playerAnimator.SetFloat("AimVertical", -PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x / PlayerCameraMovement.instance.cameraVerticalClamping);
                                }

                                SummonBullets();
                            }
                            break;
                        }
                    case PlayerGunBase.TriggerType.Hold:
                        {
                            if (Input.GetMouseButton(0) == true)
                            {
                                PlayerCameraMovement.instance.playerAnimator.SetLayerWeight(1, 1);
                                if (isHolding == false)
                                {
                                    PlayerCameraMovement.instance.playerAnimator.Play($"{gunName} Pull Tree");
                                    isHolding = true;
                                }

                                if (PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x > 180)
                                {
                                    PlayerCameraMovement.instance.playerAnimator.SetFloat("AimVertical", (360 - PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x) / PlayerCameraMovement.instance.cameraVerticalClamping);
                                }
                                else if (PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x < 180)
                                {
                                    PlayerCameraMovement.instance.playerAnimator.SetFloat("AimVertical", -PlayerCameraMovement.instance.cameraBody.transform.eulerAngles.x / PlayerCameraMovement.instance.cameraVerticalClamping);
                                }

                                holdTime += Time.deltaTime;
                                if (holdTime >= shootCD)
                                {
                                    PlayerCameraMovement.instance.playerAnimator.Play($"{gunName} Hold Tree");
                                }
                            }
                            //Soltar
                            if (Input.GetMouseButtonUp(0) == true)
                            {
                                PlayerCameraMovement.instance.playerAnimator.Play($"{gunName} Release Tree");
                                SummonBullets();
                            }
                            break;
                        }
                }
                if (Input.GetMouseButtonUp(0) == true)
                {
                    CancelInvoke();
                    Invoke("TurnWeaponDown", 1.75f);
                }
            }
            else
            {
                StartCoroutine(ReloadAction());
            }
        }
    }

    private void TurnWeaponDown()
    {
        PlayerCameraMovement.instance.playerAnimator.SetLayerWeight(1, 0);
        PlayerCameraMovement.instance.playerAnimator.SetFloat("AimVertical", 0);
    }

    private void Reload()
    {
        if (Input.GetKeyDown(InputController.instance.reloadGun) == true)
        {
            StartCoroutine(ReloadAction());
        }
    }

    private void SummonBullets()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector3 target = Vector3.zero;
        Ray cameraRay = PlayerCameraMovement.instance.cameraBody.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(cameraRay, out RaycastHit hitPos, float.MaxValue, aimCollisionLayer) == true)
        {
            target = hitPos.point;
            PlayerCameraMovement.instance.AlignTargetWithCamera(PlayerCameraMovement.instance.playerObject);
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

            if (equipedWeapon.triggerType == PlayerGunBase.TriggerType.Hold)
            {
                if (Physics.Raycast(startingPos, targetAim, out RaycastHit hitEnemy, equipedWeapon.maxRange, aimCollisionLayer) == true)
                {
                    if (hitEnemy.transform.gameObject.tag == "Enemy")
                    {
                        if (hitEnemy.transform.gameObject.TryGetComponent(out EnemyBehaviour en))
                        {
                            en.TakeDamage(equipedWeapon.damage, equipedWeapon.bulletElement);
                        }
                        else if (hitEnemy.transform.gameObject.TryGetComponent(out BoitataDamageReceiver bt))
                        {
                            bt.TakeDamage(equipedWeapon.damage, equipedWeapon.bulletElement);
                        }
                        GameObject hitParticle = Instantiate(equipedWeapon.damageParticle, hitEnemy.transform.position, equipedWeapon.damageParticle.transform.rotation);
                        Destroy(hitParticle, 3);
                        Hitmark.instance.ToggleHitmark();
                    }
                }
                isHolding = false;
            }
            else
            {
                if (Physics.Raycast(startingPos, targetAim, out RaycastHit hitEnemy, equipedWeapon.maxRange, aimCollisionLayer) == true)
                {
                    if (hitEnemy.transform.gameObject.tag == "Enemy")
                    {
                        if (hitEnemy.transform.gameObject.TryGetComponent(out EnemyBehaviour en))
                        {
                            en.TakeDamage(equipedWeapon.damage * PlayerStats.instance.GetRangedDamageMultiplier(), equipedWeapon.bulletElement);
                        }
                        else if (hitEnemy.transform.gameObject.TryGetComponent(out BoitataDamageReceiver bt))
                        {
                            bt.TakeDamage(equipedWeapon.damage * PlayerStats.instance.GetRangedDamageMultiplier(), equipedWeapon.bulletElement);
                        }
                        GameObject hitParticle = Instantiate(equipedWeapon.damageParticle, hitEnemy.transform.position, equipedWeapon.damageParticle.transform.rotation);
                        Destroy(hitParticle, 3);
                        Hitmark.instance.ToggleHitmark();
                    }
                }
            }
            localBoard.PlayAudio($"{gunName}_Shot");
            PlayerCameraMovement.instance.ShakeCamera(equipedWeapon.recoil);
            ammo--;
        }


        shootCD = equipedWeapon.fireRate + Time.time;
        UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
    }

    private IEnumerator ReloadAction()
    {
        isReloading = true;
        PlayerCameraMovement.instance.playerAnimator.SetLayerWeight(1, 1);
        switch (equipedWeapon.triggerType)
        {
            case PlayerGunBase.TriggerType.Auto:
                {
                    PlayerCameraMovement.instance.playerAnimator.Play("Reload");
                    yield return new WaitForSeconds(equipedWeapon.reloadTime);

                    ammo = equipedWeapon.ammo;
                    UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
                    break;
                }
            case PlayerGunBase.TriggerType.Semi:
                {
                    while (ammo < equipedWeapon.ammo && Input.GetMouseButton(0) == false)
                    {
                        PlayerCameraMovement.instance.playerAnimator.Play("Reload Round");
                        yield return new WaitForSeconds(equipedWeapon.reloadTime);

                        ammo++;
                        UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
                    }

                    if (ammo > equipedWeapon.ammo)
                    {
                        ammo = equipedWeapon.ammo;
                        UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
                    }
                    break;
                }
            case PlayerGunBase.TriggerType.Hold:
                {
                    yield return new WaitForSeconds(equipedWeapon.reloadTime);

                    ammo = equipedWeapon.ammo;
                    UIManager.instance.UpdateAmmo($"{ammo}/{equipedWeapon.ammo}");
                    break;
                }
        }
        if (PlayerCameraMovement.instance.isAiming == false)
        {
            PlayerCameraMovement.instance.playerAnimator.SetLayerWeight(1, 0);
        }
        isReloading = false;
    }

    public void ShootToggle(bool toggle)
    {
        canShoot = toggle;
    }
}
