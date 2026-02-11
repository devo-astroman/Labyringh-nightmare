using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyB1 : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private HeroDetectorManager _heroDetectorManager;
    [SerializeField] private EnemyNavigatorManager _enemyNavigatorManager;
    [SerializeField] private EnemySoundManager _enemySoundManager;
    [SerializeField] private EnemyCollidersManager _enemyCollidersManager;
    
    public Action wakeUpAnimationEndsAction;
    public Action tailAttackAnimationEndsAction;
    public Action getHurtAnimationEndsAction;
    public Action dieAnimationEndsAction;

    public Action<GameObject> farDetectedHeroAction;
    public Action farUndetectedHeroAction;

    public Action<GameObject> nearDetectedHeroAction;
    public Action nearUndetectedHeroAction;

    private int _life = 5;

    void Start()
    {
        _enemyAnimator.wakeUpAnimationEndsAction += HandleWakeUpAnimationEndsAction;
        _enemyAnimator.tailAttackAnimationEndsAction += HandleTailAttackAnimationEndsAction;
        _enemyAnimator.getHurtAnimationEndsAction += HandleGetHurtAnimationEndsAction;
        _enemyAnimator.dieAnimationEndsAction += HandleDieAnimationEndsAction;

        _heroDetectorManager.DetectionInfoAction += HandleDetectionInfo;


        /* _heroDetectorManager.FarHeroDetectionAction += HandleFarHeroDetection;
        _heroDetectorManager.FarHeroUndetectionAction += HanldeFarHeroUndetection;

        _heroDetectorManager.NearHeroDetectionAction += HandleNearHeroDetection;
        _heroDetectorManager.NearHeroUndetectionAction += HandleNearHeroUndetection; */


        /* _heroDetectorManager.detectedAction += HandleDetectedAction;
        _heroDetectorManager.undetectedAction += HandleUndetectedAction; */
        
        _enemyCollidersManager.HideColliderVisibility();

        _enemySoundManager.PlayWakeUp();
    }

    public void SetPatrolPoints(Vector3[] patrolPoints)
    {
        _enemyNavigatorManager.SetPatrolPoints(patrolPoints);
    }

    public void PlayPatrolAnimation()
    {
        _enemyAnimator.PlayPatrolAnimation();
        _enemySoundManager.PlayIdle();
    }

    public void ExecutePatrol()
    {
        _enemyCollidersManager.ActivatePainColliders();
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



    public void PlayReceiveHitAnimation()
    {
        _life -= 1;
        _enemyCollidersManager.DeactivatePainColliders();
        _enemyNavigatorManager.StopNavigation();
        _enemySoundManager.PlayReceiveHit();
        _enemyAnimator.PlayGetHurtAnimation();
    }

    public void ExecuteDieEnemy()
    {
        _enemyCollidersManager.DeactivatePainColliders();
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

    public void StartFollow(Transform target)
    {
        _enemyNavigatorManager.StartFollow(target);
        _enemyNavigatorManager.ResumeNavigation();
    }

    void OnDestroy()
    {
        _enemyAnimator.wakeUpAnimationEndsAction -= HandleWakeUpAnimationEndsAction;
        _enemyAnimator.tailAttackAnimationEndsAction -= HandleTailAttackAnimationEndsAction;
        _enemyAnimator.getHurtAnimationEndsAction -= HandleGetHurtAnimationEndsAction;
        _enemyAnimator.dieAnimationEndsAction -= HandleDieAnimationEndsAction;

        _heroDetectorManager.DetectionInfoAction -= HandleDetectionInfo;

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