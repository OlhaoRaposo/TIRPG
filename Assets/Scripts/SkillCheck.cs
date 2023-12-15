using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class SkillCheck : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    private float skillCheckDifficulty;
    
    [SerializeField]
    private Image hitcheckImage;
    
    [SerializeField]
    private Vector3 minHit, maxHit;
    private Vector3 hitRotation;
    private Vector3 maxhitRotation;

    [SerializeField]
    private Image skilCheckPointer;
    [SerializeField]
    private float rotationSpeed;
    
    [SerializeField]
    float minNumber;
    [SerializeField]
    float maxNumber;
    [SerializeField]
    float atualHit;

    public InteractiveObject interactiveObject;
    
    private void Start()
    {
        RollSkillCheck();
    }

    public void RollSkillCheck()
    {
        float aux = Random.Range(0, 1);
        if (aux < 0.5f)
        {
            rotationSpeed *= -1;

            hitRotation = new Vector3(0, 0, Mathf.Round(Random.Range(0, 360)));
            Quaternion rotationQuaternion = Quaternion.Euler(hitRotation);
            hitcheckImage.transform.rotation = rotationQuaternion;

            skillCheckDifficulty = Mathf.Round(Random.Range(0.05f, 0.15f) * 100.0f) / 100.0f;
            hitcheckImage.fillAmount = skillCheckDifficulty;

            minHit = hitRotation;

            maxHit = minHit - new Vector3(0, 0, skillCheckDifficulty * 360);

            if (minHit.z < maxHit.z)
            {
                minNumber = minHit.z;
                maxNumber = maxHit.z;
            }
            else
            {
                maxNumber = minHit.z;
                minNumber = maxHit.z;
            }

        }
    }

    private void Update()
    {
        skilCheckPointer.rectTransform.transform.Rotate(new Vector3(0,0,1) * Time.deltaTime * (rotationSpeed ));
        atualHit = skilCheckPointer.rectTransform.eulerAngles.z;
        if (Input.GetMouseButtonDown(1))
        {
            if(atualHit > minNumber && atualHit < maxNumber)
            {
                if (interactiveObject != null) {
                    interactiveObject.validations.Add(true);
                    Destroy(this.gameObject);
                }
            }else {
                if (interactiveObject != null) {
                    interactiveObject.validations.Add(false);
                    Destroy(this.gameObject);
                }
            }
            interactiveObject.CheckValidations();
        }
    }
}
