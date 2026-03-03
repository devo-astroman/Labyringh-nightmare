using UnityEngine;
using System;
public class EB3 : MonoBehaviour
{
    [SerializeField] EB3Animator _eB3Animator;
    [SerializeField] EnemyHealth _enemyHealth;
    [SerializeField] EnemyCollidersManager _enemyCollidersManager;
    [SerializeField] EnemySoundManager _enemySoundManager;
    [SerializeField] HeroDetectorManager _heroDetectorManager;
    [SerializeField] EnemyBulletFirer _enemyBulletFirer;
    [SerializeField] EnemyVision _enemyVision;
    

    [SerializeField] bool _receiveDamage;
    [SerializeField] bool _attack;
    [SerializeField] bool _die;

    public Action ReceiveDamageEndsAction;
    public Action AttackEndsAction;
    
    public Action<GameObject> FarHeroDetectedAction;
    public Action FarHeroUndetectionAction;
    public Action<Vector3> EnemySawAction;
    

    private bool _checkIfIsHeroVisible = false;
    private bool _isHeroVisible = false;
    private Vector3 _lastPlaceHeroWasSee;
    private SetIntervalUtility _intervalScan;

    private GameObject heroDetected;

    void Start()
    {
        _intervalScan = new SetIntervalUtility(this);

        _eB3Animator.ReceiveHitEndsAction += HandleReceiveHitEnds;
        _eB3Animator.AttackEndsAction += HandleAttackEnds;

        _heroDetectorManager.FarHeroDetectionAction += HandleFarHeroDetection;
        _heroDetectorManager.FarHeroUndetectionAction += HandleFarHeroUndetection;
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
        _heroDetectorManager.FarHeroUndetectionAction -= HandleFarHeroUndetection;
        if (_intervalScan != null)
        {
            _intervalScan.Dispose();
        }
    }

    public void WaitHideForHero() //idle
    {
        //_eB3Animator.PlayIdleAnimation();
        _eB3Animator.PlayHideAnimation();
    }

    public void SetHeroDetected(GameObject hero)
    {
        heroDetected = hero;
    }

    public bool IsHeroDetected()
    {
        return heroDetected != null;
    }

    public void ReceiveDamage(float amountDamage)
    {
        _enemySoundManager.PlayReceiveHit();
        //_eB3Animator.PlayReceiveHitAnimation();
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
            //maybe take the last place known of the hero
            //to do save the last place known and make an attack to that point
            Debug.Log("Hero is gone");
            return false;
        }        
    }

    public void MakeFireAttack()
    {
        if (heroDetected)
        {
            //Vector3 target = heroDetected.transform.position;
            Vector3 target = _lastPlaceHeroWasSee;
            _enemyBulletFirer.FireBulletFromOringin(target);
        }
    }

    public void Die()
    {
        _enemySoundManager.PlayDie();
        _eB3Animator.PlayDieAnimation();
        _enemyCollidersManager.DeactivatePainColliders();
        if (_intervalScan != null)
        {
            _intervalScan.Dispose();
        }
    }

    public void SetHealthLife(int life)
    {
        _enemyHealth.SetLife(life);
    }

    public float GetCurrentLife()
    {
        return _enemyHealth.GetLife();
    }

    public void Show()
    {
        _eB3Animator.PlayShowAnimation();
    }

    public void Hide()
    {
        _eB3Animator.PlayHideAnimation();
    }

/*     public void CheckHeroVisibility()
    {
        _checkIfIsHeroVisible =true;
    }
    public void IgnoreHeroVisibility()
    {
        _checkIfIsHeroVisible=false;
    } */
    public void ScanHero()
    {
        _checkIfIsHeroVisible =true;
        _intervalScan.SetInterval(() =>
        {
            if (heroDetected && _checkIfIsHeroVisible)
            {
                if (_enemyVision.CanSeePlayer(heroDetected.transform))
                {
                    EnemySawAction?.Invoke(heroDetected.transform.position);
                }
                
            }

        },1f);
    }

    public void StopScanHero()
    {
        _checkIfIsHeroVisible =false;
    }

    public void SetLastPlaceHeroWasSee(Vector3 position)
    {
        _lastPlaceHeroWasSee = position;
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
        heroDetected = hero;
        FarHeroDetectedAction?.Invoke(hero);
    }

    private void HandleFarHeroUndetection()
    {
        heroDetected = null;
        FarHeroUndetectionAction?.Invoke();
    }

    
}
