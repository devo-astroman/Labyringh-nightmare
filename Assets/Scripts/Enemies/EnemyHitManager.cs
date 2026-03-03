using UnityEngine;
using System;


public class EnemyHitManager : MonoBehaviour
{
    public Action HitAction;
    public bool _ignoreHits = false;


    public void IgnoreHits()
    {
        _ignoreHits = true;
    }

    public void AllowHits()
    {
        _ignoreHits = false;
    }

    public void MakeHit()
    {
        if (!_ignoreHits)
        {
            HitAction?.Invoke();            
        }

    }




}
