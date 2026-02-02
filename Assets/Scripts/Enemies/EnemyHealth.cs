using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyHealth : MonoBehaviour
{   
    private float _life = 100;

    public Action OnDead;

    void Start()
    {
    }

    public void MakeDamage(int damageAmount)
    {
        _life -= damageAmount;
        
        if(_life <= 0)
        {
            _life = 0;
            OnDead?.Invoke();
        }
    }

    void OnDestroy()
    {
        
    }



}
