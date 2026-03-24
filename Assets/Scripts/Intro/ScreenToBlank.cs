using UnityEngine;

public class ScreenToBlank : MonoBehaviour
{   
    #region Fields
    [SerializeField] ScreenFader _screenFader;
    #endregion

    #region Private properties
    #endregion


    #region Public methods    
    void Start()
    {
        Invoke(nameof(StartFade), 0f);
    }

    void OnDestroy()
    {
        //_heroDetector.detectedAction -= HandleDetected;
    }
    #endregion

    #region Public methods
    #endregion

    #region Private methods    
    private void StartFade()
    {
        _screenFader.FadeToBlack(2f);
    }


    #endregion
}