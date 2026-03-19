using UnityEngine;

public class ExitFence : MonoBehaviour
{   
    #region Fields
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private GameObject _minimapIndicatorIsOpenGO;
    [SerializeField] SfxManager _sfxManager;
    #endregion

    #region Private properties
    private InterpolatorRotator _interpolatorRotatorL;
    private InterpolatorRotator _interpolatorRotatorR;

    #endregion

    #region Unity callbacks
    void Start()        
    {
        _minimapIndicatorIsOpenGO.SetActive(false);

        _interpolatorRotatorL = _leftDoor.gameObject.GetComponent<InterpolatorRotator>();

        _interpolatorRotatorR = _rightDoor.gameObject.GetComponent<InterpolatorRotator>();

        _interpolatorRotatorL.RotationFinished += HandleRotationFinished;
    }

    void OnDestroy()        
    {
        _interpolatorRotatorL.RotationFinished -= HandleRotationFinished;
    }

    #endregion

    #region Public methods
    public void OpenFence()
    {
        _interpolatorRotatorL.RotateDegreesY(90,2);
        _interpolatorRotatorR.RotateDegreesY(-90,2);
        _sfxManager.PlayClip1();
    }
    #endregion
    #region Private methods
    private void HandleRotationFinished()
    {
        _minimapIndicatorIsOpenGO.SetActive(true);
    }
    #endregion

}