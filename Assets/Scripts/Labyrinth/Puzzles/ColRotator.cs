using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColRotator : MonoBehaviour
{

    #region Fields
    [SerializeField] private GameObject _colRotatorGO;
    [SerializeField] private HeroDetector _heroDetector;
    [SerializeField] private InterpolatorRotator _interpolatorRotator;

    [SerializeField] private IntValue _intValue;    
    #endregion

    #region public properties    
    #endregion

    #region Private properties
    private int _id = 0;
    private int nRotation = 0;
    private HeroFSM _heroFSM;
    #endregion
    #region Unity Callbacks
    void Start()
    {
        _heroDetector.detectedAction += HandleDetected;
        _heroDetector.undetectedAction += HandleUndetectedAction;
    }

    void OnDestroy()
    {
        Deactivate();
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

    public void MakeGlowOn()
    {
        _colRotatorGO.GetComponent<GlowController>().EnableGlow();
    }

    public void MakeGlowOff()
    {
        _colRotatorGO.GetComponent<GlowController>().DisableGlow();
    }

    public void MakeRotation()
    {
        Debug.Log("Should rotate!!!");
        _interpolatorRotator.RotateDegrees(90f, 0.5f);
        nRotation++;
        if (nRotation >= 4)
        {
            nRotation = 0;
        }

        _intValue.SetValue(nRotation);
    }
    public void Deactivate()
    {
        if(_heroFSM)
            _heroFSM.UndetectInteractable();

        MakeGlowOff();

        _heroDetector.detectedAction -= HandleDetected;
        _heroDetector.undetectedAction -= HandleUndetectedAction;
    }
	#endregion
    
    #region Private methods
    private void HandleDetected(GameObject hero)
    {
        _heroFSM = hero.GetComponentInParent<HeroFSM>();
        _heroFSM.DetectInteractable(_colRotatorGO);
        MakeGlowOn();
    }

    private void HandleUndetectedAction()
    {
        _heroFSM.UndetectInteractable();
        MakeGlowOff();
    }

    



    #endregion
}
