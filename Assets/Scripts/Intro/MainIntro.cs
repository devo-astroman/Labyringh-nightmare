using UnityEngine;

public class MainIntro : MonoBehaviour
{   
    #region Fields
    [SerializeField] GameSceneManager _gameSceneManager;
    #endregion

    #region Private properties
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