using UnityEngine;
using System;


public class AnimationFinished : MonoBehaviour
{
    public static bool IsAnimationFinished(Animator animator, string stateName, int layer = 0)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);

        return state.IsName(stateName)
            && state.normalizedTime >= 1f
            && !animator.IsInTransition(layer);
    }

}
