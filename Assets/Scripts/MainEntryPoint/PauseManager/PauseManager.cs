using UnityEngine;
using System;

public class PauseManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private Pauser _pauser;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _controlsMenu;
    #endregion

    #region Private Fields 
    private bool isPaused = false;
    #endregion

    #region Public Fields 
    public Action MainMenuButtonClickedAction;
    #endregion

    #region Unity Callbacks    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    #endregion

    #region Public Methods
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void MainMenuButtonClicked()
    {
        MainMenuButtonClickedAction?.Invoke();
    }

    public void UnPauseToGo()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _pauser.ResumeGame();
    }
    public void ControlsButtonClicked()
    {
        _pauseMenu.SetActive(false);
        _controlsMenu.SetActive(true);
    }
    public void ExitControlsButtonClicked()
    {
        _pauseMenu.SetActive(true);
        _controlsMenu.SetActive(false);
    }
    #endregion

    #region Private Methods
    private void PauseGame()
    {
        Debug.Log("_PauseGame_!!!!");

        if (_pauser != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _pauser.PauseGame();
            _pauseMenu.SetActive(true);
        }

        //Time.timeScale = 0f; // freezes gameplay
        _pauser.PauseGame();
    }

    private void ResumeGame()
    {
        Debug.Log("_ResumeGame_!!!!");
        if (_pauser != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _pauser.ResumeGame();
            _pauseMenu.SetActive(false);
            _controlsMenu.SetActive(false);
        }

        //Time.timeScale = 1f; // resumes gameplay
        _pauser.ResumeGame();
    }
    #endregion
}