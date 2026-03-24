using UnityEngine;

public class SceneChanger : MonoBehaviour
{   
    #region Fields
    [SerializeField] GameSceneManager _gameSceneManager;
    #endregion

    #region Private properties
    #endregion


    #region Public methods    
    void Start()
    {
        _gameSceneManager.GoPlay();
    }
    #endregion

    #region Public methods
    #endregion

    #region Private methods
    #endregion
}