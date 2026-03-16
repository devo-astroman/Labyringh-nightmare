using UnityEngine;
using System;


public class EB2 : MonoBehaviour
{
    
    [SerializeField] EnemyNavigatorManager _enemyNavigatorManager;
    [SerializeField] EnemyHeroTarget _targetToFollow;
    [SerializeField] EB2Animator _eB2Animator;
    [SerializeField] EnemyHealth _enemyHealth;
    [SerializeField] EnemyCollidersManager _enemyCollidersManager;
    [SerializeField] EnemySoundManager _enemySoundManager;
    [SerializeField] HeroDetectorManager _heroDetectorManager;
    [SerializeField] GameObject _minimapIndigactorGO;
    

    [SerializeField] bool _follow;
    [SerializeField] bool _receiveDamage;


    public Action ReceiveDamageEndsAction;
    public Action<GameObject> NearHeroDetectedAction;
    

    void Start()
    {
        _eB2Animator.ReceiveHitEndsAction += HandleReceiveHitEnds;
        _heroDetectorManager.NearHeroDetectionAction += HandleNearHeroDetection;
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
        _heroDetectorManager.NearHeroDetectionAction -= HandleNearHeroDetection;
    }

    public void SetTargetToFollow(EnemyHeroTarget target)
    {
       _targetToFollow = target;
    }

    public void FollowTarget()
    {
        _enemySoundManager.PlayIdle(1f);
        _enemyNavigatorManager.StartFollow(_targetToFollow);

    }

    public void ResumeFollowTarget()
    {
        _enemySoundManager.PlayIdle(1f);
        _enemyNavigatorManager.ResumeFollow();
    }

    public void ReceiveDamage(float amountDamage)
    {
        _enemySoundManager.PlayReceiveHit();
        _enemyNavigatorManager.StopNavigation();
        _eB2Animator.NotifyWhenReceiveHitAnimationEnds();
        _eB2Animator.PlayReceiveHitAnimation();
        _enemyHealth.DecreaseLife(amountDamage);
    }

    public void Die()
    {
        _enemySoundManager.PlayDie();
        _eB2Animator.PlayDieAnimation();
        _enemyCollidersManager.DeactivatePainColliders();
        _minimapIndigactorGO.SetActive(false);
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

    public void DamageTarget()
    {
        GameObject parentGO = _targetToFollow.transform.parent?.gameObject;
        if (parentGO)
        {
            parentGO.GetComponent<HeroFSM>().TriggerReceiveHitFromEnemy();
        }
    }

    public void TurnOnMinimapIndicator()
    {
        _minimapIndigactorGO.SetActive(true);
    }

    private void HandleReceiveHitEnds()
    {
        ReceiveDamageEndsAction?.Invoke();
    }

    private void HandleNearHeroDetection(GameObject hero)
    {
        NearHeroDetectedAction?.Invoke(hero);
    }

    


}


