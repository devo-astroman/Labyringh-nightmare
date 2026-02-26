using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEditorInternal;


public class EB2Animator : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void PlayReceiveHitAnimation()
    {
        _animator.SetTrigger("ReceiveHit");
    }
}
