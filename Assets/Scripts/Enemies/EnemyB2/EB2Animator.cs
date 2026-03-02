using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;
using Unity.VisualScripting;


public class EB2Animator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] SkinnedMeshRenderer _meshRenderer;

    public Action ReceiveHitEndsAction;

    private bool _checkEndOfReceiveHitAnimation = false;

    public void PlayReceiveHitAnimation()
    {
        _animator.SetTrigger("ReceiveHit");
    }

    public void PlayDieAnimation()
    {
        //not really an animation for now
        _animator.SetTrigger("Die");
    }

    void Update()
    {
        if (_checkEndOfReceiveHitAnimation && ReceiveHitEndsAction != null &&AnimationFinished.IsAnimationFinished(_animator, "ReceiveHit"))
        {
            ReceiveHitEndsAction.Invoke();
            _checkEndOfReceiveHitAnimation = false;
        }
    }

    public void NotifyWhenReceiveHitAnimationEnds()
    {
        _checkEndOfReceiveHitAnimation = true;
    }

    public void IgnoreWhenReceiveHitAnimationEnds()
    {
        _checkEndOfReceiveHitAnimation = false;
    }
}
