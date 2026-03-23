using UnityEngine;

public class Pauser : MonoBehaviour
{
    public void PauseGame()
    {
        AudioListener.pause = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }
    
}
