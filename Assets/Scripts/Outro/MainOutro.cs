using UnityEngine;

public class MainOutro : MonoBehaviour
{   
    #region Fields
    [SerializeField] AnimationManager _animationManager;
    [SerializeField] ScreenFader _screenFader;
    [SerializeField] HeroDetector _heroDetector;

    
    #endregion

    #region Private properties
    #endregion


    #region Public methods    
    void Start()
    {
        _animationManager.PlayHeroAnimation();
        //_heroDetector.detectedAction += HandleDetected;
        Invoke(nameof(StartFade), 7f);
    }

    void OnDestroy()
    {
        //_heroDetector.detectedAction -= HandleDetected;
    }
    #endregion

    #region Public methods
    #endregion

    #region Private methods
    private void HandleDetected(GameObject hero)
    {
        _screenFader.FadeToBlack(1f);
    }
    private void StartFade()
    {
        _screenFader.FadeToBlack(.25f);
    }
    #endregion
}