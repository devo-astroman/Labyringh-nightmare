using UnityEngine;
using System;
using System.Collections;

public class CutsceneSfxManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private AudioSource _aSource;
    [SerializeField] private AudioSource _aSource2;
    [SerializeField] private AudioClip _clip1Sfx;
    [SerializeField] private AudioClip _clip2Sfx;

    private float _defaultVolume = 1f;
    #endregion

    #region Actions 

    public Action Clip1FinishedAction;
    public Action Clip2FinishedAction;

    #endregion

    #region Unity callbacks

    void Start()
    {
        _defaultVolume = AllGameData.cutsceneVolumen;

        if (_aSource != null)
            _aSource.volume = _defaultVolume;

        if (_aSource2 != null)
            _aSource2.volume = _defaultVolume;
    }

    #endregion

    #region Public Methods

    public void PlayClip1()
    {
        PlayClip(_clip1Sfx, false, _aSource, Clip1FinishedAction);
    }

    public void PlayClip2()
    {
        PlayClip(_clip2Sfx, false, _aSource2, Clip2FinishedAction);
    }

    #endregion

    #region Private Methods

    private void PlayClip(
        AudioClip clip,
        bool loop,
        AudioSource audioSource,
        Action onFinished)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("CutsceneSfxManager: AudioSource is not assigned.");
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("CutsceneSfxManager: AudioClip is null.");
            return;
        }

        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.volume = _defaultVolume;
        audioSource.Play();

        if (!loop)
        {
            StartCoroutine(NotifyWhenFinished(audioSource, onFinished));
        }
    }

    private IEnumerator NotifyWhenFinished(
        AudioSource source,
        Action onFinished)
    {
        yield return new WaitWhile(() => source.isPlaying);

        onFinished?.Invoke();
    }

    #endregion
}