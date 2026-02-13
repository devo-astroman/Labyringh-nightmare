using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHurtController : MonoBehaviour
{
    #region Fields
	[SerializeField] private Animator _animator;

    #endregion

    #region Private properties
	private string _ReceiveHitParam = "ReceiveHit";

    #endregion
    public void MakePlayerGetHurt()
    {
        Debug.Log("Make player get hurt");
        _animator.SetTrigger(_ReceiveHitParam);
        //animate hurt

        //show blood
        //play sound
    }



}
