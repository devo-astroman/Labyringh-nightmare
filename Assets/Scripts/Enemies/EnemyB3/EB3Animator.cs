using UnityEngine;
using System;
using Unity.VisualScripting;
public class EB3Animator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] SkinnedMeshRenderer _meshRenderer;
    [SerializeField] AnimatorEventHandler _animatorEventHandler;


    public Action ReceiveHitEndsAction;
    public Action AttackEndsAction;
    public Action AttackPrimeMomentReachedAction;

    

    public void PlayShowAnimation()
    {
        _animator.SetBool("Show",true);
    }

    public void PlayHideAnimation()
    {
        _animator.SetBool("Show",false);
    }

    public void PlayIdleAnimation()
    {
        _animator.SetBool("Idle",true);
    }

    public void StopIdleAnimation()
    {
        _animator.SetBool("Idle",false);
    }

    public void PlayReceiveHitAnimation()
    {
        _animator.SetTrigger("ReceiveHit");
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }

    public void PlayDieAnimation()
    {
        _animator.SetTrigger("Die");
    }

    public void LookToTarget(Vector3 target)
    {
        Transform t = _animator.transform;

        Vector3 direction = target - t.position;
        direction.y = 0f; // optional: ignore vertical rotation

        if (direction != Vector3.zero)
        {
            t.rotation = Quaternion.LookRotation(direction);
        }
    }

    void Start()
    {

        _animatorEventHandler.FireEvent1Action += HandleFireEvent1Action;

        var behaviours = _animator.GetBehaviours<EndStateBehaviour>();
        foreach (var b in behaviours)
            b.StateExitAction += HandleAnimationFinished;
    }

    void OnDestroy()
    {
        _animatorEventHandler.FireEvent1Action -= HandleFireEvent1Action;

        var behaviours = _animator.GetBehaviours<EndStateBehaviour>();
        foreach (var b in behaviours)
            b.StateExitAction -= HandleAnimationFinished;
       
    }

    private void HandleAnimationFinished(string stateName)
    {
        if(stateName == "Attack_eventHandler")
        {
            HandleAttackFinished();
        }
    }

    private void HandleAttackFinished()
    {
        AttackEndsAction?.Invoke();
    }

    private void HandleFireEvent1Action(int id)
    {
        AttackPrimeMomentReachedAction?.Invoke();
    }

}
