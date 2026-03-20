using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    
    [Header("Scene Names (must match Build Settings)")]
    [SerializeField] private string sceneMainMenu = "MainMenu";
    [SerializeField] private string sceneGameplay = "Gameplay";

    public void GoMainMenu()
    {
        LoadScene(sceneMainMenu);
    }

    public void GoPlay()
    {
        LoadScene(sceneGameplay);
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name not set!");
            return;
        }

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log($"Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' not found in Build Settings!");
        }
    }

}
