using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IntValue : MonoBehaviour
{

    #region Fields
    private int _intValue;
    #endregion

    #region public properties
    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks    
    #endregion

    #region Public methods
    public int GetValue()
    {
        return _intValue;
    }
    public void SetValue(int value)
    {
        _intValue = value;
    }
	#endregion
    
    #region Private methods
    #endregion
}
