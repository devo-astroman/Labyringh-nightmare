using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Actionable : MonoBehaviour
{

    #region Fields
    #endregion

    #region public properties
    public UnityEvent MakeInteractionUnityEvent;

    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks    
    #endregion

    #region Public methods
    public void MakeInteraction()
    {
        MakeInteractionUnityEvent.Invoke();
    }
	#endregion
    
    #region Private methods
    #endregion
}
