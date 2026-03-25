using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{   
    #region Fields
    [SerializeField] private MenuSettingsChanger _menuSettingsChanger;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _cutsceneSlider;
    [SerializeField] private AudioSource _cutsceneAudioSource;
    [SerializeField] private MusicManager _musicManager;
    #endregion
    #region Private properties
    #endregion
    #region Unity callbacks
    void Start()
    {
        Debug.Log("music " + AllGameData.defaultMusicVolumen);
        _musicSlider.value = AllGameData.defaultMusicVolumen;

        Debug.Log("sfx " + AllGameData.sfxVolumen);
        _sfxSlider.value = AllGameData.defaultSfxVolumen;

        Debug.Log("cutscenes " + AllGameData.sfxVolumen);
        _cutsceneSlider.value = AllGameData.defaultCutsceneVolumen;
    }
    #endregion

    #region Public methods
    public void BackButtonClicked()
    {
        Debug.Log("BackButtonClicked");
        _menuSettingsChanger.ChangeToMenu();
    }

    //sliders
    public void OnSfxPointerDown()
    {
        Debug.Log("OnSfxPointerDown");
        _sfxAudioSource.Play();
    }


    public void OnSfxValueChanged()
    {
        Debug.Log("OnSfxValueChanged");
        Debug.Log(_sfxSlider.value);
        _sfxAudioSource.volume = _sfxSlider.value;
        AllGameData.sfxVolumen = _sfxSlider.value;
    }

    public void OnSfxPointerUp()
    {
        Debug.Log("OnSfxPointerUp");
        _sfxAudioSource.Stop();
    }

    public void OnMusicPointerDown()
    {
        Debug.Log("OnMusicPointerDown");
        //_sfxAudioSource.Play();
    }


    public void OnMusicValueChanged()
    {
        Debug.Log("OnMusicValueChanged");
        Debug.Log(_musicSlider.value);
        AllGameData.musicVolumen = _musicSlider.value;
        _musicManager.SetVolume(_musicSlider.value);
    }

    public void OnMusicPointerUp()
    {
        Debug.Log("OnMusicPointerUp");
        //_sfxAudioSource.Stop();
    }

    public void OnCutscenePointerDown()
    {
        Debug.Log("OnCutscenePointerDown");
        _cutsceneAudioSource.Play();
    }


    public void OnCutsceneValueChanged()
    {
        Debug.Log("OnCutsceneValueChanged");
        Debug.Log(_cutsceneSlider.value);
        AllGameData.cutsceneVolumen = _cutsceneSlider.value;
        _cutsceneAudioSource.volume = _cutsceneSlider.value;
    }

    public void OnCutscenePointerUp()
    {
        Debug.Log("OnCutscenePointerUp");
        _cutsceneAudioSource.Stop();
    }

    #endregion
}
