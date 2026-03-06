using UnityEngine;
using UnityEngine.Events;
using System;


public class Actionable : MonoBehaviour
{

    #region Fields
    #endregion

    #region public properties
    public UnityEvent MakeInteractionUnityEvent;
    public Action MakeInteractionAction;

    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks    
    #endregion

    #region Public methods
    public void MakeInteraction()
    {
        MakeInteractionUnityEvent.Invoke();
        MakeInteractionAction?.Invoke();
    }
	#endregion
    
    #region Private methods
    #endregion
}
