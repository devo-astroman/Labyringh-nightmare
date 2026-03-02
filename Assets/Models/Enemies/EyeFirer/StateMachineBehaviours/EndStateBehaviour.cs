using UnityEngine;
using System;

public class EndStateBehaviour : StateMachineBehaviour
{
    public event Action StateExitAction;

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        StateExitAction?.Invoke();
    }
}