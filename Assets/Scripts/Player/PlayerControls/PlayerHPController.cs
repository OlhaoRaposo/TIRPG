using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Net.Http;
using System.Collections;

public class PlayerHPController : MonoBehaviour
{
    [Header("Variables")]
    public static PlayerHPController instance;
    [SerializeField] private float hpMax, staminaMax;
    [SerializeField] private float currentHP, currentStamina;

    [Header("References")]
    [SerializeField] private Image hpImage, staminaImage;

    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        currentHP = hpMax;
        currentStamina = staminaMax;

        SetHP(hpMax);
        SetStamina(staminaMax);
    }

    private void OnLevelWasLoaded()
    {
        Start();
    }

    public void SetHP(float amount)
    {
        if (amount > hpMax)
        {
            hpMax = amount;
        }
        hpImage.fillAmount = amount / hpMax;
    }

    public void ChangeHP(float changeAmmount, bool isDecrease)
    {
        if (CheatMenu.instance.GetHasInfiniteHP()) return;
        if (PlayerMovement.instance.isDashing) return;

            if (isDecrease == true)
            {
                currentHP -= changeAmmount;
                hpImage.fillAmount = currentHP / hpMax;

                if (currentHP <= 0)
                {
                    Die();
                }
            }
            else
            {
                currentHP += changeAmmount;

                if (currentHP < hpMax)
                {
                    hpImage.fillAmount = hpMax / hpMax;
                }
                else
                {
                    hpImage.fillAmount = 1;
                    currentHP = hpMax;
                }
            }
    }
    [ContextMenu("Die")]
    void Die() {
        PlayerCameraMovement.instance.ToggleAimLock(false);
        ItemDropManager.instance.DestroyDroppedItems();
        SceneController.instance.GameOver();
    }
    public void SetStamina(float ammount)
    {
        if (ammount > staminaMax)
        {
            staminaMax = ammount;
        }
        staminaImage.fillAmount = ammount / staminaMax;
    }

    public void ChangeStamina(float changeAmmount, bool isDecrease)
    {
        if (CheatMenu.instance.GetHasInfiniteStamina()) return;

        if (isDecrease == true)
        {
            currentStamina -= changeAmmount;
            staminaImage.fillAmount = currentStamina / staminaMax;
        }
        else
        {
            currentStamina += changeAmmount;

            if (currentStamina < staminaMax)
            {
                staminaImage.fillAmount = currentStamina / staminaMax;
            }
            else
            {
                staminaImage.fillAmount = 1;
                currentStamina = staminaMax;
            }
        }
    }
    public void IncreaseMaxHP(float hp)
    {
        hpMax += hp;
        currentHP = hpMax;
    }
    public void BuffHp(float multiplier, float duration)
    {
        float startHp = hpMax;
        hpMax *= multiplier;

        StartCoroutine(ResetBuffHp(startHp, duration));
    }
    IEnumerator ResetBuffHp(float startHp, float duration)
    {
        yield return new WaitForSeconds(duration);

        hpMax = startHp;
    }

    public void IncreaseStamina(float stam)
    {
        staminaMax += stam;
        currentStamina = staminaMax;
    }
    public void BuffStamina(float multiplier, float duration)
    {
        float startStamina = staminaMax;
        staminaMax *= multiplier;

        StartCoroutine(ResetBuffStamina(startStamina, duration));
    }
    IEnumerator ResetBuffStamina(float startStamina, float duration)
    {
        yield return new WaitForSeconds(duration);

        staminaMax = startStamina;
    }

    public float GetStamina()
    {
        return currentStamina;
    }
    public float GetMaxStamina()
    {
        return staminaMax;
    }
    public float GetHp()
    {
        return currentHP;
    }
    public float GetMaxHp()
    {
        return hpMax;
    }
}
