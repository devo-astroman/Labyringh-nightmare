using UnityEngine;

public class Settings : MonoBehaviour
{   
    #region Fields
    [SerializeField] private MenuSettingsChanger _menuSettingsChanger;
    #endregion
    #region Private properties
    #endregion
    #region Unity callbacks
    #endregion

    #region Public methods
    public void BackButtonClicked()
    {
        Debug.Log("BackButtonClicked");
        _menuSettingsChanger.ChangeToMenu();
    }

    #endregion
}