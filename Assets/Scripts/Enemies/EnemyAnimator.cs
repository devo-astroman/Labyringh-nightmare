using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private AnimatorEventHandler _animatorEventHandler;
    
    public Action wakeUpAnimationEndsAction;
    public Action tailAttackAnimationEndsAction;
    public Action getHurtAnimationEndsAction;
    public Action dieAnimationEndsAction;

    void Start()
    {
        _animatorEventHandler.FireEvent1Action += HandleFireEvent1Action;
        _animatorEventHandler.FireEvent2Action += HandleFireEvent2Action;
        _animatorEventHandler.FireEvent3Action += HandleFireEvent3Action;
        _animatorEventHandler.FireEvent4Action += HandleFireEvent4Action;
    }


    public void TriggerReceiveHit()
    {
        _animator.SetTrigger("receiveHit");
    }

    public void SetLife(float life)
    {
        _animator.SetFloat("life",life);
    }

    public void TriggerAttack()
    {
        _animator.SetTrigger("attack");
    }

    public void PlayPatrolAnimation()
    {
        _animator.SetBool("Follow&Patrol",true);
        _animator.SetBool("Attack",false);
        _animator.SetBool("ReceiveHit",false);
        _animator.SetBool("Die",false);
    }

    public void PlayGetHurtAnimation()
    {
        _animator.SetBool("Follow&Patrol",false);
        _animator.SetBool("Attack",false);
        _animator.SetBool("ReceiveHit",true);
        _animator.SetBool("Die",false);
    }

    public void PlayDieAnimation()
    {
        _animator.SetBool("Follow&Patrol",false);
        _animator.SetBool("Attack",false);
        _animator.SetBool("ReceiveHit",false);
        _animator.SetBool("Die",true);
    }

    public void PlayAttackAnimation()
    {
        _animator.SetBool("Follow&Patrol",false);
        _animator.SetBool("Attack",true);
        _animator.SetBool("ReceiveHit",false);
        _animator.SetBool("Die",false);
    }

/*     public void PlayPatroAnimation()
    {
        _animator.SetBool("Follow&Patrol",true);
        _animator.SetBool("Attack",false);
        _animator.SetBool("ReceiveHit",false);
        _animator.SetBool("Die",false);
    } */

    void OnDestroy()
    {
        _animatorEventHandler.FireEvent1Action -= HandleFireEvent1Action;
        _animatorEventHandler.FireEvent2Action -= HandleFireEvent2Action;
        _animatorEventHandler.FireEvent3Action += HandleFireEvent3Action;
        _animatorEventHandler.FireEvent4Action += HandleFireEvent4Action;
    }

    private void HandleFireEvent1Action(int id)
    {
        wakeUpAnimationEndsAction?.Invoke();
    }

    private void HandleFireEvent2Action(int id)
    {
        tailAttackAnimationEndsAction?.Invoke();
    }

    private void HandleFireEvent3Action(int id)
    {
        getHurtAnimationEndsAction?.Invoke();
    }

    private void HandleFireEvent4Action(int id)
    {
        dieAnimationEndsAction?.Invoke();
    }

}
