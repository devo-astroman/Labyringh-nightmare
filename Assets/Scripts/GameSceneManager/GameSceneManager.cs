using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    
    [Header("Scene Names (must match Build Settings)")]
    [SerializeField] private string sceneMainMenu = "MainMenu";
    [SerializeField] private string sceneIntro = "Intro";
    [SerializeField] private string sceneGameplay = "Gameplay";
    [SerializeField] private string sceneOutro = "Outro";

    public void GoMainMenu()
    {
        LoadScene(sceneMainMenu);
    }

    public void GoIntro()
    {
        LoadScene(sceneIntro);
    }

    public void GoPlay()
    {
        LoadScene(sceneGameplay);
    }

    public void GoOutro()
    {
        LoadScene(sceneOutro);
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
