using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyHealth : MonoBehaviour
{   
    private float _life = 100;

    public Action DeadAction;

    void Start()
    {
    }

    public void SetLife(float life)
    {
        _life = life;
    }

    public void MakeDamage(int damageAmount)
    {
        _life -= damageAmount;
        
        if(_life <= 0)
        {
            _life = 0;
            DeadAction?.Invoke();
        }
    }

    public float GetLife()
    {
        return _life;
    }

    void OnDestroy()
    {
        
    }



}
