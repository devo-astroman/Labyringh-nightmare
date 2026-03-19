using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private AudioSource _aSource;
    [SerializeField] private AudioClip _exploreMusic;
    [SerializeField] private AudioClip _waveMusic;
    [SerializeField] private AudioClip _waveFinishedMusic;
    [SerializeField] private float _defaultVolume = 1f;
    #endregion

    #region Private Fields 
    private Coroutine _fadeCoroutine;
    #endregion

    #region Public Methods
    public void PlayExploreMusic()
    {
        PlayClip(_exploreMusic, true,_aSource);
    }

    public void PlayWaveMusic()
    {
        PlayClip(_waveMusic, true,_aSource);
    }

    public void PlayWaveFinishedMusic()
    {
        PlayClip(_waveFinishedMusic, false,_aSource);
    }

    public void CrossfadeToExploreMusic(float duration = 2f)
    {
        CrossfadeToClip(_exploreMusic, true, duration);
    }

    public void CrossfadeToWaveMusic(float duration = 2f)
    {
        CrossfadeToClip(_waveMusic, true, duration);
    }

    public void CrossfadeToWaveFinishedMusic(float duration = 2f)
    {
        CrossfadeToClip(_waveFinishedMusic, false, duration);
    }

    public void PlayExploreWithDelay(float delay)
    {
        if (_aSource == null)
        {
            Debug.LogWarning("MusicManager: AudioSource is not assigned.");
            return;
        }

        /* if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        } */

        StartCoroutine(PlayExploreWithDelayCoroutine(delay));
    }

    public void FadeToStop(float duration = 2f)
    {
        if (_aSource == null)
        {
            Debug.LogWarning("MusicManager: AudioSource is not assigned.");
            return;
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(FadeOutCoroutine(duration));
    }

    public void SetVolume(float volume)
    {
        if (_aSource == null)
        {
            Debug.LogWarning("MusicManager: AudioSource is not assigned.");
            return;
        }

        _defaultVolume = Mathf.Clamp01(volume);
        _aSource.volume = _defaultVolume;
    }

    public void StopMusic()
    {
        if (_aSource == null)
        {
            Debug.LogWarning("MusicManager: AudioSource is not assigned.");
            return;
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        _aSource.Stop();
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

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.volume = _defaultVolume;
        audioSource.Play();
    }

    private void CrossfadeToClip(AudioClip newClip, bool loop, float duration)
    {
        if (_aSource == null)
        {
            Debug.LogWarning("MusicManager: AudioSource is not assigned.");
            return;
        }

        if (newClip == null)
        {
            Debug.LogWarning("MusicManager: AudioClip is null.");
            return;
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        _fadeCoroutine = StartCoroutine(CrossfadeCoroutine(newClip, loop, duration));
    }

    private IEnumerator CrossfadeCoroutine(AudioClip newClip, bool loop, float duration)
    {
        if (duration <= 0f)
        {
            _aSource.clip = newClip;
            _aSource.loop = loop;
            _aSource.volume = _defaultVolume;
            _aSource.Play();
            _fadeCoroutine = null;
            yield break;
        }

        float halfDuration = duration * 0.5f;
        float startVolume = _aSource.volume;

        // Fade out current clip
        if (_aSource.isPlaying && _aSource.clip != null)
        {
            float t = 0f;
            while (t < halfDuration)
            {
                t += Time.deltaTime;
                _aSource.volume = Mathf.Lerp(startVolume, 0f, t / halfDuration);
                yield return null;
            }
        }

        _aSource.volume = 0f;
        _aSource.Stop();

        // Switch clip
        _aSource.clip = newClip;
        _aSource.loop = loop;
        _aSource.Play();

        // Fade in new clip
        float tIn = 0f;
        while (tIn < halfDuration)
        {
            tIn += Time.deltaTime;
            _aSource.volume = Mathf.Lerp(0f, _defaultVolume, tIn / halfDuration);
            yield return null;
        }

        _aSource.volume = _defaultVolume;
        _fadeCoroutine = null;
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = _aSource.volume;
        float t = 0f;

        if (duration <= 0f)
        {
            _aSource.Stop();
            _fadeCoroutine = null;
            yield break;
        }

        while (t < duration)
        {
            t += Time.deltaTime;
            _aSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        _aSource.volume = 0f;
        _aSource.Stop();
        _aSource.volume = _defaultVolume;
        _fadeCoroutine = null;
    }

    private IEnumerator PlayExploreWithDelayCoroutine(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        PlayClip(_exploreMusic, true, _aSource);
    }
    #endregion
}