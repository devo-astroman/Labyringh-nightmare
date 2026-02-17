using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStealthManager : MonoBehaviour
{   
    #region Fields
    #endregion

    #region public properties
    [SerializeField] public bool IsHidden { get; private set; }
    #endregion

    #region Private properties
    #endregion
    #region Unity Callbacks
	#endregion

    #region Public methods
    public void SetHidden(bool hidden) => IsHidden = hidden;
	#endregion




}
