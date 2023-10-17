using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class PlayerHPController : MonoBehaviour
{
    [Header("Variables")]
    public static PlayerHPController instance;
    [SerializeField] private float hpMax, staminaMax;
    [SerializeField] private float currentHP, currentStamina;

    [Header("References")]
    [SerializeField] private Image hpImage, staminaImage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHP = hpMax;
        currentStamina = staminaMax;
    }

    public void ChangeHP(float changeAmmount, bool isDecrease)
    {
        if(isDecrease == true)
        {
            currentHP -= changeAmmount;
            hpImage.fillAmount = (currentHP - changeAmmount) / hpMax;

            if(currentHP <= 0)
            {
                //Game Over
            } 
        }
        else
        {
            currentHP += changeAmmount;

            if(currentHP < hpMax)
            {
                hpImage.fillAmount = (hpMax + changeAmmount) / hpMax;
            }
            else
            {
                hpImage.fillAmount = 1;
                currentHP = hpMax;
            }
        }
    }

    public void ChangeStamina(float changeAmmount, bool isDecrease)
    {
        if(isDecrease == true)
        {
            currentStamina -= changeAmmount;
            staminaImage.fillAmount = (currentStamina - changeAmmount) / staminaMax;
        }
        else
        {
            currentStamina += changeAmmount;
            Debug.Log("Ganhou stamina");

            if(currentStamina < staminaMax)
            {
                staminaImage.fillAmount = (currentStamina + changeAmmount) / staminaMax;
            }
            else
            {
                staminaImage.fillAmount = 1;
                currentStamina = staminaMax;
            }
        }
    }

    public float GetMaxStamina()
    {
        return staminaMax;
    }
}
