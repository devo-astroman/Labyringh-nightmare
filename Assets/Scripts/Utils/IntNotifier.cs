using System;
using UnityEngine;

public class IntNotifier : MonoBehaviour
{
    #region Fields
    #endregion

    #region public properties
    public Action<int> IntNotifyAction;
    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks
    #endregion

    #region Public methods 
    public void NotifyInt(int id)
    {
        IntNotifyAction?.Invoke(id);
    }
    #endregion

    #region Private methods
    #endregion
}
