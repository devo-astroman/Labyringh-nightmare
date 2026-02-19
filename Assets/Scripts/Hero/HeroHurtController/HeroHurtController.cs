using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHurtController : MonoBehaviour
{
    #region Fields
	[SerializeField] private Animator _animator;
    [SerializeField] private Hud _hud;
    [SerializeField] private HeroSoundManager _heroSoundManager;

    #endregion

    #region Private properties
	private string _ReceiveHitParam = "ReceiveHit";
    private int _coolDownTime = 2;
    private bool _coolDownEnded = true;
    private SetTimeoutUtility _timeout;
    private SetTimeoutUtility _timeoutCoolDown;

    #endregion
    public void MakePlayerGetHurt()
    {

        if (_coolDownEnded)
        {
            _animator.SetTrigger(_ReceiveHitParam);

            _hud.ShowHurtScreen();
            _timeout.SetTimeout(() => {
                _hud.HideHurtScreen();
            }, .8f); 

            _heroSoundManager.PlayReceiveHit();

            _coolDownEnded = false;
            _timeoutCoolDown.SetTimeout(() => {
                _coolDownEnded = true;
            }, _coolDownTime);
        }

        
    }

    #region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
		_timeout = new SetTimeoutUtility(this);
        _timeoutCoolDown = new SetTimeoutUtility(this);
    }

	void OnDestroy()
    {
		_timeout.Dispose();
        _timeoutCoolDown.Dispose();
    }
	#endregion



}
