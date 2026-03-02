using UnityEngine;
using System;
public class EB3 : MonoBehaviour
{
    [SerializeField] EB3Animator _eB3Animator;
    [SerializeField] EnemyHealth _enemyHealth;
    [SerializeField] EnemyCollidersManager _enemyCollidersManager;
    [SerializeField] EnemySoundManager _enemySoundManager;
    [SerializeField] HeroDetectorManager _heroDetectorManager;

    [SerializeField] bool _receiveDamage;
    [SerializeField] bool _attack;
    [SerializeField] bool _die;

    public Action ReceiveDamageEndsAction;
    public Action AttackEndsAction;
    
    public Action<GameObject> FarHeroDetectedAction;

    private GameObject heroDetected;

    void Start()
    {
        _eB3Animator.ReceiveHitEndsAction += HandleReceiveHitEnds;
        _eB3Animator.AttackEndsAction += HandleAttackEnds;

        _heroDetectorManager.FarHeroDetectionAction += HandleFarHeroDetection;
    }

    void Update()
    {


        if (_receiveDamage)
        {
            ReceiveDamage(1);
            _receiveDamage = false;
        }

        if (_attack)
        {
            
            _receiveDamage = false;
        }

    }

    void OnDestroy()
    {
        _eB3Animator.ReceiveHitEndsAction -= HandleReceiveHitEnds;
        _eB3Animator.AttackEndsAction -= HandleAttackEnds;
        _heroDetectorManager.FarHeroDetectionAction -= HandleFarHeroDetection;
    }

    public void WaitForHero() //idle
    {
        _eB3Animator.PlayIdleAnimation();
    }

    public void SetHeroDetected(GameObject hero)
    {
        heroDetected = hero;
    }

    public void ReceiveDamage(float amountDamage)
    {
        _enemySoundManager.PlayReceiveHit();
        _eB3Animator.PlayReceiveHitAnimation();
        _enemyHealth.DecreaseLife(amountDamage);
    }

    public bool Attack()
    {
        if (heroDetected)
        {
            //_enemySoundManager.PlayAttack();
            Vector3 target = heroDetected.transform.position;
            _eB3Animator.LookToTarget(target);
            _eB3Animator.PlayAttackAnimation();
            return true;
        }
        else
        {
            Debug.Log("Warning there is no hero to attack");
            return false;
        }

        
    }

    public void Die()
    {
        _enemySoundManager.PlayDie();
        _eB3Animator.PlayDieAnimation();
        _enemyCollidersManager.DeactivatePainColliders();
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

    private void HandleAttackEnds()
    {
        AttackEndsAction?.Invoke();
    }

    private void HandleFarHeroDetection(GameObject hero)
    {
        FarHeroDetectedAction?.Invoke(hero);
    }
    
}