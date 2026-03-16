using UnityEngine;
using System;


public class EnemyB1 : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private HeroDetectorManager _heroDetectorManager;
    [SerializeField] private EnemyNavigatorManager _enemyNavigatorManager;
    [SerializeField] private EnemySoundManager _enemySoundManager;
    [SerializeField] private EnemyCollidersManager _enemyCollidersManager;
    [SerializeField] private EnemyAttackManager _enemyAttackManager;
    [SerializeField] private GameObject _minimapIndicator;
    
    public Action wakeUpAnimationEndsAction;
    public Action tailAttackAnimationEndsAction;
    public Action getHurtAnimationEndsAction;
    public Action dieAnimationEndsAction;

    public Action<GameObject> farDetectedHeroAction;
    public Action farUndetectedHeroAction;

    public Action<GameObject> nearDetectedHeroAction;
    public Action nearUndetectedHeroAction;

    public Action<GameObject> attackTouchedHeroAction;

    public Action attackDamageStartAction;
    public Action attackDamageEndAction;

    private int _life = 5;
    private int _id = 0;

    void Start()
    {
        _enemyAnimator.wakeUpAnimationEndsAction += HandleWakeUpAnimationEndsAction;
        //_enemyAnimator.tailAttackAnimationEndsAction += HandleTailAttackAnimationEndsAction;
        _enemyAnimator.tailAttackAnimationEndsAction_Check += HandleTailAttackAnimationEndsAction;
        //_enemyAnimator.getHurtAnimationEndsAction += HandleGetHurtAnimationEndsAction;
        _enemyAnimator.receiveHitAnimationEndsAction_Check += HandleGetHurtAnimationEndsAction;
        _enemyAnimator.dieAnimationEndsAction += HandleDieAnimationEndsAction;

        _heroDetectorManager.DetectionInfoAction += HandleDetectionInfo;

        _enemyAttackManager.attackTouchedHeroAction += HandleAttackTouchedHero;

        _enemyAnimator.attackDamageStartAction += HandleAttackDamageStart;
        _enemyAnimator.attackDamageEndsAction += HandleAttackDamageEnds;


        /* _heroDetectorManager.FarHeroDetectionAction += HandleFarHeroDetection;
        _heroDetectorManager.FarHeroUndetectionAction += HanldeFarHeroUndetection;

        _heroDetectorManager.NearHeroDetectionAction += HandleNearHeroDetection;
        _heroDetectorManager.NearHeroUndetectionAction += HandleNearHeroUndetection; */


        /* _heroDetectorManager.detectedAction += HandleDetectedAction;
        _heroDetectorManager.undetectedAction += HandleUndetectedAction; */
        
        _enemyCollidersManager.HideColliderVisibility();

        _enemySoundManager.PlayWakeUp();
    }

    public void SetId(int id)
    {
        _id=id;
    }

    public int GetId()
    {
        return _id;
    }

    public void SetPatrolPoints(Vector3[] patrolPoints)
    {
        _enemyNavigatorManager.SetPatrolPoints(patrolPoints);
    }

    public void PlayPatrolAnimation()
    {
        _enemyAnimator.PlayPatrolAnimation();
        _enemySoundManager.PlayIdle(.5f);
    }

    public void AllowReceiveDamage()
    {
        _enemyCollidersManager.ActivatePainColliders();
    }

    public void BlockReceiveDamage()
    {
        _enemyCollidersManager.DeactivatePainColliders();
    }

    public void ExecutePatrol()
    {
        //_enemyCollidersManager.ActivatePainColliders();
        _enemyNavigatorManager.ExecutePatrol();
    }

    public void StopPatrol()
    {
        _enemyNavigatorManager.StopNavigation();
    }

    public void StopNavigation()
    {
        _enemyNavigatorManager.StopNavigation();
    }

    public void DecreaseLife()
    {
        _life -= 1;
    }

    public void PlayReceiveHitAnimation()
    {
        //_enemyCollidersManager.DeactivatePainColliders();
        _enemyNavigatorManager.StopNavigation();
        _enemySoundManager.PlayReceiveHit();
        _enemyAnimator.PlayGetHurtAnimation();
    }

    public void ExecuteDieEnemy()
    {
        //_enemyCollidersManager.DeactivatePainColliders();
        _enemyNavigatorManager.StopNavigation();
        _enemySoundManager.PlayDie();
        _enemyAnimator.PlayDieAnimation();
    }

    public void ExecuteAttack()
    {
        _enemyNavigatorManager.StopNavigation();
        _enemyAnimator.PlayAttackAnimation();
        //_enemySoundManager.PlayAttack(); todo
    }

    public int GetCurrentLife()
    {
        return _life;
    }

    public void StartFollow(EnemyHeroTarget target)
    {
        _enemyNavigatorManager.StartFollow(target);
        _enemyNavigatorManager.ResumeNavigation();
    }

    public void NotifyAttackHitHero()
    {
        _enemyAttackManager.StartAttackHitCheck();
    }

    public void IgnoreAttackHitHero()
    {
        _enemyAttackManager.EndAttackHitCheck();
    }
//End of animation Attack
    public void CheckEndOfAttackAnimation()
    {
        _enemyAnimator.CheckEndOfAttackAnimation();
    }

    public void IgnoreEndOfAttackAnimation()
    {
        _enemyAnimator.IgnoreEndOfAttackAnimation();
    }
//
//End of animation ReceiveHit
    public void CheckEndOfReceiveHitAnimation()
    {
        _enemyAnimator.CheckEndOfReceiveHitAnimation();
    }

    public void IgnoreEndOfReceiveHitAnimation()
    {
        _enemyAnimator.IgnoreEndOfReceiveHitAnimation();
    }
//

    public void TurnOffMinimapIndicator()
    {
        _minimapIndicator.SetActive(false);
    }
    public void TurnOnMinimapIndicator()
    {
        _minimapIndicator.SetActive(true);
    }
    void OnDestroy()
    {
        _enemyAnimator.wakeUpAnimationEndsAction -= HandleWakeUpAnimationEndsAction;
        //_enemyAnimator.tailAttackAnimationEndsAction -= HandleTailAttackAnimationEndsAction;
        _enemyAnimator.tailAttackAnimationEndsAction_Check -= HandleTailAttackAnimationEndsAction;
        //_enemyAnimator.getHurtAnimationEndsAction -= HandleGetHurtAnimationEndsAction;
        _enemyAnimator.receiveHitAnimationEndsAction_Check -= HandleGetHurtAnimationEndsAction;
        _enemyAnimator.dieAnimationEndsAction -= HandleDieAnimationEndsAction;

        _heroDetectorManager.DetectionInfoAction -= HandleDetectionInfo;

        _enemyAttackManager.attackTouchedHeroAction -= HandleAttackTouchedHero;

        _enemyAnimator.attackDamageStartAction -= HandleAttackDamageStart;
        _enemyAnimator.attackDamageEndsAction -= HandleAttackDamageEnds;

        /* _heroDetectorManager.FarHeroDetectionAction += HandleFarHeroDetection;
        _heroDetectorManager.FarHeroUndetectionAction += HanldeFarHeroUndetection;

        _heroDetectorManager.NearHeroDetectionAction += HandleNearHeroDetection;
        _heroDetectorManager.NearHeroUndetectionAction += HandleNearHeroUndetection; */

        /* _heroDetectorManager.detectedAction -= HandleDetectedAction;
        _heroDetectorManager.undetectedAction -= HandleUndetectedAction; */

        
    }

    private void HandleWakeUpAnimationEndsAction()
    {
        wakeUpAnimationEndsAction?.Invoke();
    }

    private void HandleTailAttackAnimationEndsAction()
    {
        Debug.Log("End Check!!!");
        tailAttackAnimationEndsAction?.Invoke();
    }

    private void HandleGetHurtAnimationEndsAction()
    {
        getHurtAnimationEndsAction?.Invoke();
    }

    private void HandleDieAnimationEndsAction()
    {
        dieAnimationEndsAction?.Invoke();
    }

    private void HandleAttackTouchedHero(GameObject heroGO)
    {
        attackTouchedHeroAction?.Invoke(heroGO);
    }

    private void HandleAttackDamageStart()
    {
        attackDamageStartAction?.Invoke();
    }

    private void HandleAttackDamageEnds()
    {
        attackDamageEndAction?.Invoke();
    }
    

    



    private void HandleDetectionInfo(int type, GameObject heroGO)
    {
        if(type == 0){ //far detection
            farDetectedHeroAction?.Invoke(heroGO);
        }else if (type == 1)
        {
            farUndetectedHeroAction?.Invoke();
        }else if (type == 2)
        {
            nearDetectedHeroAction?.Invoke(heroGO);
        }else if (type == 3)
        {
            nearUndetectedHeroAction?.Invoke();
        }
    }





    
}