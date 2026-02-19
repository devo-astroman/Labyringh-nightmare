using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    #region Fields    
    [SerializeField] private HeroDetector _heroDetector;
    #endregion

    #region public properties    
    #endregion

    #region Private properties
    private int _id=0;
    #endregion
    #region Unity Callbacks
    void Start()
    {    
        _heroDetector.detectedAction += HandleDetected;
        _heroDetector.undetectedAction += HandleUndetectedAction;
    }

    void OnDestroy()
    {
        _heroDetector.detectedAction -= HandleDetected;
        _heroDetector.undetectedAction -= HandleUndetectedAction;
    }
    #endregion

    #region Public methods
    public void SetId(int id)
    {
        _id = id;
    }

    public int GetId()
    {
        return _id;
    }

	#endregion
    
    #region Private methods
    private void HandleDetected(GameObject hero)
    {
        HeroFSM heroFSM = hero.GetComponentInParent<HeroFSM>();
        heroFSM.TriggerReceiveDamageFromTrap();
    }

    private void HandleUndetectedAction()
    {
    }

    #endregion
}
