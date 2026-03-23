using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{   
    #region Fields
    [SerializeField] private MenuSettingsChanger _menuSettingsChanger;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private AudioSource _sfxAudioSource;
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
        _sfxSlider.value = AllGameData.sfxVolumen;
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
        AllGameData.sfxVolumen = _musicSlider.value;
        _musicManager.SetVolume(_musicSlider.value);
    }

    public void OnMusicPointerUp()
    {
        Debug.Log("OnMusicPointerUp");
        //_sfxAudioSource.Stop();
    }

    #endregion
}
