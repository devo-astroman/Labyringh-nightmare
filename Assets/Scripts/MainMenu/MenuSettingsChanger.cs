using UnityEngine;

public class MenuSettingsChanger : MonoBehaviour
{   
    #region Fields
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _settingsUI;
    [SerializeField] private GameObject _controlsUI;
    #endregion

    #region Private properties    
    #endregion
    #region Unity callbacks
    #endregion

    #region Public methods
    public void ChangeToSettings()
    {
        _menuUI.SetActive(false);
        _settingsUI.SetActive(true);
        _controlsUI.SetActive(false);
    }

    public void ChangeToMenu()
    {
        _menuUI.SetActive(true);
        _settingsUI.SetActive(false);
        _controlsUI.SetActive(false);
    }

    public void ChangeToControls()
    {
        _menuUI.SetActive(false);
        _settingsUI.SetActive(false);
        _controlsUI.SetActive(true);
    }
    
    #endregion
}