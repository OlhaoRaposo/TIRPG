using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Player/Melee Weapon")]
public class PlayerMeleeBase : ScriptableObject
{
    public float damage;
    public float comboExecutionWindowPercentage;
    public float[] comboWindowTime;
    public AnimatorOverrideController animations;

}
