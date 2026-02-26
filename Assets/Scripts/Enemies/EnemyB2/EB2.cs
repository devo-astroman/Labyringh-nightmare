using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EB2 : MonoBehaviour
{
    
    [SerializeField] EnemyNavigatorManager _enemyNavigatorManager;
    [SerializeField] Transform _targetToFollow;

    [SerializeField] bool _follow;


    public void FollowTarget()
    {
        _enemyNavigatorManager.StartFollow(_targetToFollow);
    }

    void Update()
    {
        if (_follow)
        {
            
            FollowTarget();
            _follow = false;
        }
    }


}
