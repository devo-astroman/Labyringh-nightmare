using UnityEngine;

public class MainMenu : MonoBehaviour
{   
    #region Fields
    [SerializeField] private GameSceneManager _gameSceneManager;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private MenuSettingsChanger _menuSettingsChanger;    
    #endregion

    #region Private properties
    private Camera _cameraHero;
    #endregion
    #region Unity callbacks
    void Awake()
    {
        //load settings preferences
        Debug.Log("Setting Music volume " + AllGameData.musicVolumen);
        _musicManager.SetVolume(AllGameData.musicVolumen);
        _musicManager.PlayMenuMusic();
    }
    #endregion

    #region Public methods
    public void StartButtonClicked()
    {
        Debug.Log("StartButtonClicked");
        _gameSceneManager.GoIntro();
    }
    public void SettingsButtonClicked()
    {        
        _menuSettingsChanger.ChangeToSettings();
    }

    public void ControlsButtonClicked()
    {        
        _menuSettingsChanger.ChangeToControls();
    }

    public void ControlsBackButtonClicked()
    {        
        _menuSettingsChanger.ChangeToMenu();
    }
    public void QuitButtonClicked()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    #endregion
}