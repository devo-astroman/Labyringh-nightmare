using UnityEngine;
using System;


public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private AnimatorEventHandler _animatorEventHandler;
    
    public Action wakeUpAnimationEndsAction;
    public Action tailAttackAnimationEndsAction;
    public Action tailAttackAnimationEndsAction_Check;
    public Action receiveHitAnimationEndsAction_Check;
    public Action getHurtAnimationEndsAction;
    public Action dieAnimationEndsAction;
    public Action attackDamageStartAction;
    public Action attackDamageEndsAction;    

    private bool _checkEndOfAttackAnimation = false;
    private bool _checkEndOfReceiveHitAnimation = false;

    void Start()
    {
        _animatorEventHandler.FireEvent1Action += HandleFireEvent1Action;
        _animatorEventHandler.FireEvent2Action += HandleFireEvent2Action;
        _animatorEventHandler.FireEvent3Action += HandleFireEvent3Action;
        _animatorEventHandler.FireEvent4Action += HandleFireEvent4Action;
        _animatorEventHandler.FireEvent5Action += HandleFireEvent5Action;
        _animatorEventHandler.FireEvent6Action += HandleFireEvent6Action;
    }

    void Update()
    {

        if (_checkEndOfAttackAnimation && IsAnimationFinished(_animator, "Attack_TailAttack_Event"))
        {
            tailAttackAnimationEndsAction_Check?.Invoke();
        }

        if (_checkEndOfReceiveHitAnimation && IsAnimationFinished(_animator, "ReceiveHit_GetHurt_Event"))
        {
            receiveHitAnimationEndsAction_Check?.Invoke();
        }
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

    public void CheckEndOfAttackAnimation()
    {
        _checkEndOfAttackAnimation = true;
    }

    public void IgnoreEndOfAttackAnimation()
    {
        _checkEndOfAttackAnimation = false;
    }

    public void CheckEndOfReceiveHitAnimation()
    {
        _checkEndOfReceiveHitAnimation = true;
    }

    public void IgnoreEndOfReceiveHitAnimation()
    {
        _checkEndOfReceiveHitAnimation = false;
    }

    void OnDestroy()
    {
        _animatorEventHandler.FireEvent1Action -= HandleFireEvent1Action;
        _animatorEventHandler.FireEvent2Action -= HandleFireEvent2Action;
        _animatorEventHandler.FireEvent3Action -= HandleFireEvent3Action;
        _animatorEventHandler.FireEvent4Action -= HandleFireEvent4Action;
        _animatorEventHandler.FireEvent5Action -= HandleFireEvent5Action;
        _animatorEventHandler.FireEvent6Action -= HandleFireEvent6Action;
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

    private void HandleFireEvent5Action(int id)
    {
        
        attackDamageStartAction?.Invoke();
    }

    private void HandleFireEvent6Action(int id)
    {
        
        attackDamageEndsAction?.Invoke();
    }

    private bool IsAnimationFinished(Animator animator, string stateName, int layer = 0)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);
        return state.IsName(stateName)
            && state.normalizedTime >= 1f
            && !animator.IsInTransition(layer);
    }


}
