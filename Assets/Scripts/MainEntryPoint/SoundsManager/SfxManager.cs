using UnityEngine;

public class SfxManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private AudioSource _aSource;
    [SerializeField] private AudioClip _clip1Sfx;    
    private float _defaultVolume = 1f;
    #endregion

    #region Private Fields
    #endregion
    #region Unity callbacks
    void Start()
    {
        Debug.Log("setting sfx Volumen" + AllGameData.sfxVolumen);
        _aSource.volume = AllGameData.sfxVolumen;
        _defaultVolume = AllGameData.sfxVolumen;
    }
    #endregion

    #region Public Methods
    public void PlayClip1()
    {
        PlayClip(_clip1Sfx, false,_aSource);
    }
    #endregion

    #region Private Methods
    private void PlayClip(AudioClip clip, bool loop, AudioSource audioSource)
    {
        if(audioSource == null)
        {
            audioSource = _aSource;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("MusicManager: AudioSource is not assigned.");
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("MusicManager: AudioClip is null.");
            return;
        }

        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.volume = _defaultVolume;
        audioSource.Play();
    }
    #endregion
}