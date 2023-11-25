using UnityEditor.Animations;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimationData", menuName = "Character/AnimationData")]

public class AnimationData : ScriptableObject
{
    public Avatar armature;
    public AnimationClip[] idles, movements, attacks;
    [HideInInspector] public AnimatorOverrideController overrider;

    public void InstantiateOverrider()
    {
        overrider = new AnimatorOverrideController();
    }
}
