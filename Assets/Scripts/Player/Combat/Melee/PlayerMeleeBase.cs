using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Player/Melee Weapon")]
public class PlayerMeleeBase : ScriptableObject
{
    public string modelName;
    public float reach;
    public float damage;
    public DamageElementManager.DamageElement damageElement = DamageElementManager.DamageElement.Physical;
    public float comboExecutionWindowPercentage;

}
