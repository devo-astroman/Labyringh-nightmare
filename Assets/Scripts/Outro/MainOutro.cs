using UnityEngine;

public class MainOutro : MonoBehaviour
{   
    #region Fields
    [SerializeField] AnimationManager _animationManager;
    [SerializeField] ScreenFader _screenFader;
    [SerializeField] HeroDetector _heroDetector;
    [SerializeField] CutsceneSfxManager _cutsceneSfxManager;    
    [SerializeField] GameSceneManager _gameSceneManager;    
    #endregion

    #region Private properties
    #endregion


    #region Public methods    
    void Start()
    {
        _cutsceneSfxManager.Clip1FinishedAction += HandleClip1Finished;
        
        _cutsceneSfxManager.PlayClip2();
        _screenFader.OnFadeFinished += HandleFadeFinished;
        _animationManager.PlayHeroAnimation();        
        Invoke(nameof(StartFade), 7f);
    }

    void OnDestroy()
    {        
        _screenFader.OnFadeFinished -= HandleFadeFinished;
        _cutsceneSfxManager.Clip1FinishedAction -= HandleClip1Finished;
    }
    #endregion

    #region Public methods

    public void OnMainMenuButtonClicked()
    {
         _gameSceneManager.GoMainMenu();
    }
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
    private void HandleFadeFinished()
    {
        _cutsceneSfxManager.PlayClip1();
    }

    private void HandleClip1Finished()
    {
        //_gameSceneManager.GoCredits();
        _animationManager.PlayShowThanks();
    }
    #endregion
}