using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHurtController : MonoBehaviour
{
    #region Fields
	[SerializeField] private Animator _animator;
    [SerializeField] private Hud _hud;

    #endregion

    #region Private properties
	private string _ReceiveHitParam = "ReceiveHit";
    private SetTimeoutUtility _timeout;

    #endregion
    public void MakePlayerGetHurt()
    {
        Debug.Log("Make player get hurt");
        _animator.SetTrigger(_ReceiveHitParam);
        //animate hurt

        //show screen red
        _hud.ShowHurtScreen();
        _timeout.SetTimeout(() => {
            _hud.HideHurtScreen();
        }, .8f); 

        //play sound
    }

    #region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
		_timeout = new SetTimeoutUtility(this);
    }
	#endregion



}
