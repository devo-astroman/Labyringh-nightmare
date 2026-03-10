using UnityEngine;
using UnityEngine.Events;
using System;

public class ColButton : MonoBehaviour
{

    #region Fields
    [SerializeField] private GameObject _colButtonGO;
    [SerializeField] private HeroDetector _heroDetector;
    [SerializeField] private InterpolatorMover _interpolatorMover;
    [SerializeField] private Transform _originTransform;
    [SerializeField] private Transform _destinyTransform;
    #endregion

    #region public properties
    public UnityEvent onPressed;    
    public UnityEvent onPulled;
    public Action<int> buttonIteractAction;


    
    #endregion

    #region Private properties
    private int _id = 0;
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

    public void MakeGlowOn()
    {
        _colButtonGO.GetComponent<GlowController>().EnableGlow();
    }

    public void MakeGlowOff()
    {
        _colButtonGO.GetComponent<GlowController>().DisableGlow();
    }

    public void NotifyButtonPressed()
    {
        //_interpolatorMover.MoveFromTo(_originTransform.position,_destinyTransform.position,1f);
        onPressed?.Invoke();
        buttonIteractAction?.Invoke(_id);
    }

    public void PressButton()
    {
        _interpolatorMover.MoveFromTo(_originTransform.position,_destinyTransform.position,1f);
        //onPressed?.Invoke();
    }

    public void PullButton()
    {        
        _interpolatorMover.MoveFromTo(_destinyTransform.position,_originTransform.position,1f);
        onPulled?.Invoke();
    }

    public void PressAndPull()
    {
        _interpolatorMover.MoveToAndReturn(_originTransform.position,_destinyTransform.position,.25f);
        
    }

    public void Activate()
    {
        MakeGlowOn();
        _heroDetector.detectedAction += HandleDetected;
        _heroDetector.undetectedAction += HandleUndetectedAction;        
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
        _heroFSM.DetectInteractable(_colButtonGO);
        MakeGlowOn();
    }

    private void HandleUndetectedAction()
    {
        _heroFSM.UndetectInteractable();
        MakeGlowOff();
    }
    #endregion
}
