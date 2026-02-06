using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    

    public void TriggerReceiveHit()
    {
        _animator.SetTrigger("receiveHit");
    }

    public void SetLife(float life)
    {
        _animator.SetFloat("life",life);
    }


}
