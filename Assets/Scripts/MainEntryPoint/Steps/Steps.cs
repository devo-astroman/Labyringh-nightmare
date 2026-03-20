using System;
using UnityEngine;

public class Steps : MonoBehaviour
{
    #region Fields
    [SerializeField] private HeroDetector _heroDetector;    
    #endregion

    #region Private Fields  //private variables not SerializeFields
    #endregion

    #region Public Fields
    public Action triggerReachedAction;
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _heroDetector.detectedAction +=  HandleDetected;
    }
    void OnDestroy()
    {
        _heroDetector.detectedAction -=  HandleDetected;
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void HandleDetected(GameObject hero)
    {
        Debug.Log("HeroDetected!!!");
        triggerReachedAction?.Invoke();
    }
    #endregion
}