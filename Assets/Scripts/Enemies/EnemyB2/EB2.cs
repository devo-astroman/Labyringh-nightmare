using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EB2 : MonoBehaviour
{
    
    [SerializeField] EnemyNavigatorManager _enemyNavigatorManager;
    [SerializeField] Transform _targetToFollow;
    [SerializeField] EB2Animator _eB2Animator;
    [SerializeField] EnemyHealth _enemyHealth;
    [SerializeField] EnemyCollidersManager _enemyCollidersManager;

    

    [SerializeField] bool _follow;
    [SerializeField] bool _receiveDamage;


    public Action ReceiveDamageEndsAction;

    void Start()
    {
        _eB2Animator.ReceiveHitEndsAction += HandleReceiveHitEnds;
    }


    void Update()
    {
        if (_follow)
        {
            
            FollowTarget();
            _follow = false;
        }

        if (_receiveDamage)
        {
            ReceiveDamage(1);
            _receiveDamage = false;
        }

    }

    void OnDestroy()
    {
        _eB2Animator.ReceiveHitEndsAction -= HandleReceiveHitEnds;
    }

    public void FollowTarget()
    {
        _enemyNavigatorManager.StartFollow(_targetToFollow);
    }

    public void ResumeFollowTarget()
    {
        _enemyNavigatorManager.ResumeFollow();
    }

    public void ReceiveDamage(float amountDamage)
    {
        _enemyNavigatorManager.StopNavigation();
        _eB2Animator.NotifyWhenReceiveHitAnimationEnds();
        _eB2Animator.PlayReceiveHitAnimation();
        _enemyHealth.DecreaseLife(amountDamage);
    }

    public void Die()
    {
        _eB2Animator.PlayDieAnimation();
        _enemyCollidersManager.DeactivatePainColliders();
    }

    public void CleanReceiveDamage()
    {
        _eB2Animator.IgnoreWhenReceiveHitAnimationEnds();
     
    }

    public void SetHealthLife(int life)
    {
        _enemyHealth.SetLife(life);
    }

    public float GetCurrentLife()
    {
        return _enemyHealth.GetLife();
    }

    private void HandleReceiveHitEnds()
    {
        ReceiveDamageEndsAction?.Invoke();
    }


}


