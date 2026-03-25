using UnityEngine;

public class MainIntro : MonoBehaviour
{   
    #region Fields
    [SerializeField] GameSceneManager _gameSceneManager;
    [SerializeField] AudioSource _audioSource;
    #endregion

    #region Private properties
    #endregion

    #region Unity callbacks
    void Start()
    {
        _audioSource.volume = AllGameData.cutsceneVolumen;
    }
    #endregion

    #region Public methods
    #endregion

    #region Public methods
    public void OnSkipButtonClicked()
    {
        _gameSceneManager.GoPlay();
    }
    #endregion

    #region Private methods
    #endregion
}