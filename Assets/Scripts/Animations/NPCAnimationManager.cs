using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnimationData animations;
    private Animator myAnimator;

    private void Start()
    {
        myAnimator = gameObject.GetComponent<Animator>();
        animations.InstantiateOverrider();
        animations.overrider.runtimeAnimatorController = myAnimator.runtimeAnimatorController;
        myAnimator.runtimeAnimatorController = animations.overrider;

        if (myAnimator != null && animations.armature != null)
        {
            myAnimator.avatar = animations.armature;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PlayIdle(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlayPatrol(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlayAttack(0);
        }
    }

    public void PlayIdle(int index)
    {
        if (animations.idles != null)
        {
            if (index < animations.idles.Length)
            {
                animations.overrider["0_Idle"] = animations.idles[index];
            }
            else
            {
                animations.overrider["0_Idle"] = animations.idles[0];
            }
            myAnimator.Play("Idle", 0, 0);
        }

    }

    public void PlayPatrol(int index)
    {
        if (animations.movements != null)
        {
            if (index < animations.movements.Length)
            {
                animations.overrider["0_Patrol"] = animations.movements[index];
            }
            else
            {
                animations.overrider["0_Patrol"] = animations.movements[0];
            }
            myAnimator.Play("Patrol", 0, 0);
        }
    }

    public void PlayAttack(int index)
    {
        if (animations.attacks != null)
        {
            if (index < animations.attacks.Length)
            {
                animations.overrider["0_Attack"] = animations.attacks[index];
            }
            else
            {
                animations.overrider["0_Attack"] = animations.attacks[0];
            }
            myAnimator.Play("Attack", 0, 0);
        }
    }
}
