using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    [SerializeField] private HeroDetector _heroDetector;
    // Start is called before the first frame update

    public Action<GameObject> attackTouchedHeroAction;

    public bool isAttacking = false;
    public bool alreadyNotified = false;
    

    void Start()
    {
        _heroDetector.detectedAction += HandleDetectedHero;
    }

    void OnDestroy()
    {
        _heroDetector.detectedAction -= HandleDetectedHero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttackHitCheck()
    {
        isAttacking = true;
        alreadyNotified = false;
        _heroDetector.gameObject.SetActive(true);
    }

    public void EndAttackHitCheck()
    {
        isAttacking = false;
        _heroDetector.gameObject.SetActive(false);
    }

    private void HandleDetectedHero(GameObject heroGO)
    {
        if (isAttacking)
        {
            if (!alreadyNotified)
            {
                alreadyNotified = true;
                attackTouchedHeroAction?.Invoke(heroGO);                
            }
            
        }
    }
}
