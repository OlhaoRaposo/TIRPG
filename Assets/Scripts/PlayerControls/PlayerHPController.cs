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
            hpImage.fillAmount = currentHP / hpMax;

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
                hpImage.fillAmount = hpMax / hpMax;
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
            staminaImage.fillAmount = currentStamina / staminaMax;
        }
        else
        {
            currentStamina += changeAmmount;

            if(currentStamina < staminaMax)
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

    public float GetMaxStamina()
    {
        return staminaMax;
    }
}
