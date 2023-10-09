using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHPController : MonoBehaviour
{
    [Header("Variables")]
    public static PlayerHPController instance;
    [SerializeField] private float hpMax;
    [SerializeField] private float currentHP;

    [Header("References")]
    [SerializeField] private Image hpImage;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHP = hpMax;
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
            }
        }
    }
}
