using UnityEngine;

public class MenuSettingsChanger : MonoBehaviour
{   
    #region Fields
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _settingsUI;
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
    }

    public void ChangeToMenu()
    {
        _menuUI.SetActive(true);
        _settingsUI.SetActive(false);
    }
    
    #endregion
}