using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Player/Melee Weapon")]
public class PlayerMeleeBase : ScriptableObject
{
    public string modelName;
    public float damage;
    public float comboExecutionWindowPercentage;

}
