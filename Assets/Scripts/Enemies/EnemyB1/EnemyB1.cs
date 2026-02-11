using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyB1 : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private HeroDetector _heroDetector;
    [SerializeField] private EnemyNavigatorManager _enemyNavigatorManager;
    [SerializeField] private EnemySoundManager _enemySoundManager;
    [SerializeField] private EnemyCollidersManager _enemyCollidersManager;
    

    public Action wakeUpAnimationEndsAction;
    public Action tailAttackAnimationEndsAction;
    public Action getHurtAnimationEndsAction;
    public Action dieAnimationEndsAction;

    public Action<GameObject> detectedHeroAction;

    private int _life = 5;

    void Start()
    {
        _enemyAnimator.wakeUpAnimationEndsAction += HandleWakeUpAnimationEndsAction;
        _enemyAnimator.tailAttackAnimationEndsAction += HandleTailAttackAnimationEndsAction;
        _enemyAnimator.getHurtAnimationEndsAction += HandleGetHurtAnimationEndsAction;
        _enemyAnimator.dieAnimationEndsAction += HandleDieAnimationEndsAction;

        _heroDetector.detectedAction += HandleDetectedAction;
        
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

        _enemyNavigatorManager.StopNavigation();
        _enemySoundManager.PlayReceiveHit();
        _enemyAnimator.PlayGetHurtAnimation();
    }

    public void ExecuteDieEnemy()
    {
        _enemyNavigatorManager.StopNavigation();
        _enemyCollidersManager.RemoveCollisions();
        _enemySoundManager.PlayDie();
        _enemyAnimator.PlayDieAnimation();
    }

    public int GetCurrentLife()
    {
        return _life;
    }

    void OnDestroy()
    {
        _enemyAnimator.wakeUpAnimationEndsAction -= HandleWakeUpAnimationEndsAction;
        _enemyAnimator.tailAttackAnimationEndsAction -= HandleTailAttackAnimationEndsAction;
        _enemyAnimator.getHurtAnimationEndsAction -= HandleGetHurtAnimationEndsAction;
        _enemyAnimator.dieAnimationEndsAction -= HandleDieAnimationEndsAction;

        _heroDetector.detectedAction -= HandleDetectedAction;
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

    private void HandleDetectedAction(GameObject heroGO)
    {
        detectedHeroAction?.Invoke(heroGO);
    }
}