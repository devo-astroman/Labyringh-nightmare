using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   
    #region Fields
    [SerializeField] private GameSceneManager _gameSceneManager;
    [SerializeField] private MusicManager _musicManager;
    #endregion

    #region Private properties
    private Camera _cameraHero;
    #endregion
    #region Unity callbacks
    void Start()
    {
        _musicManager.PlayMenuMusic();
    }
    #endregion

    #region Public methods
    public void StartButtonClicked()
    {
        Debug.Log("StartButtonClicked");
        _gameSceneManager.GoPlay();
    }
    public void SettingsButtonClicked()
    {
        Debug.Log("SettingsButtonClicked");
    }
    public void QuitButtonClicked()
    {
        Debug.Log("QuitButtonClicked");

        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    #endregion
}