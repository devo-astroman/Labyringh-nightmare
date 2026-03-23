using UnityEngine;
using System.Collections;

public class SoundsManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private MusicManager _musicManager;    
    #endregion

    #region Private Fields
    #endregion

    #region properties
    #endregion


    #region Unity Callbacks
    #endregion

    #region Public Methods

    public void SetVolume(float value)
    {
        _musicManager.SetVolume(value);
    }
    public void PlayExploreMusic()
    {
        _musicManager.PlayExploreMusic();
    }

    public void CrossfadeToExploreMusic()
    {
        _musicManager.CrossfadeToExploreMusic();
    }

    public void PlayExploreMusicWithDelay(float duration=2f)
    {
        _musicManager.PlayExploreWithDelay(duration);
    }

    public void PlayWaveMusic()
    {
        _musicManager.PlayWaveMusic();
    }

    public void CrossfadeToWaveMusic(float duration = 1f)
    {
        _musicManager.CrossfadeToWaveMusic(duration);
    }

    public void PlayWaveFinishedMusic()
    {
        _musicManager.PlayWaveFinishedMusic();
    }

    public void CrossfadeToWaveFinishedMusic()
    {
        _musicManager.CrossfadeToWaveFinishedMusic(1f);
    }

    public void FadeMusicToStop()
    {
        _musicManager.FadeToStop(1f);
    }
    
    #endregion
}