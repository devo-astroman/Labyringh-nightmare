using UnityEngine;
using System;
using Unity.VisualScripting;
public class EB3Animator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] SkinnedMeshRenderer _meshRenderer;

    private EndStateBehaviour _attackBehaviour;
    private EndStateBehaviour _receiveHitBehaviour;

    public Action ReceiveHitEndsAction;
    public Action AttackEndsAction;

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
        _attackBehaviour = _animator.GetBehaviour<EndStateBehaviour>();
        if (_attackBehaviour != null)
        {
            _attackBehaviour.StateExitAction += HandleAttackFinished;
        }

        _receiveHitBehaviour = _animator.GetBehaviour<EndStateBehaviour>();
        if (_receiveHitBehaviour != null)
        {
            _receiveHitBehaviour.StateExitAction += HandleReceiveHitFinished;
        }
        
    }

    void OnDestroy()
    {
        if (_attackBehaviour != null)
        {
            _attackBehaviour.StateExitAction -= HandleAttackFinished;
        }

        if (_receiveHitBehaviour != null)
        {
            _receiveHitBehaviour.StateExitAction -= HandleReceiveHitFinished;
        }
    }

    private void HandleAttackFinished()
    {
        AttackEndsAction?.Invoke();
    }

    private void HandleReceiveHitFinished()
    {
        ReceiveHitEndsAction?.Invoke();
    }
}
