using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyB1 : MonoBehaviour
{
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private HeroDetector _heroDetector;
    [SerializeField] private EnemyNavigatorManager _enemyNavigatorManager;
    

    public Action wakeUpAnimationEndsAction;
    public Action tailAttackAnimationEndsAction;
    public Action getHurtAnimationEndsAction;
    public Action dieAnimationEndsAction;

    public Action<GameObject> detectedHeroAction;

    

    void Start()
    {
        _enemyAnimator.wakeUpAnimationEndsAction += HandleWakeUpAnimationEndsAction;
        _enemyAnimator.tailAttackAnimationEndsAction += HandleTailAttackAnimationEndsAction;
        _enemyAnimator.getHurtAnimationEndsAction += HandleGetHurtAnimationEndsAction;
        _enemyAnimator.dieAnimationEndsAction += HandleDieAnimationEndsAction;

        _heroDetector.detectedAction += HandleDetectedAction;
    }

    public void SetPatrolPoints(Vector3[] patrolPoints)
    {
        _enemyNavigatorManager.SetPatrolPoints(patrolPoints);
    }

    public void PlayPatrolAnimation()
    {
        _enemyAnimator.PlayPatrolAnimation();
    }

    public void ExecutePatrol()
    {
        _enemyNavigatorManager.ExecutePatrol();
    }

    public void StopPatrol()
    {
        _enemyNavigatorManager.StopNavigation();
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