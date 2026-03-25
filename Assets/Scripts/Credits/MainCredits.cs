using UnityEngine;

public class MainCredits : MonoBehaviour
{   
    #region Fields
    [SerializeField] GameSceneManager _gameSceneManager;
    #endregion

    #region Private properties
    #endregion

    #region Public methods
    #endregion

    #region Public methods
    public void OnMainMenuButtonClicked()
    {
        _gameSceneManager.GoMainMenu();
    }
    #endregion

    #region Private methods    
    #endregion
}