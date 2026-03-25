using UnityEngine;

public class MainCredits : MonoBehaviour
{   
    #region Fields
    [SerializeField] GameSceneManager _gameSceneManager;
    #endregion

    #region Private properties
    #endregion

    #region Unity callbacks
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
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