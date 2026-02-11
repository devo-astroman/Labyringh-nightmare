using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _idleSound;
    [SerializeField] private AudioClip _receiveHitSound;
    [SerializeField] private AudioClip _wakeUpSound;
    [SerializeField] private AudioClip _dieSound;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float _volume = 1f;
    [SerializeField] private bool _loopIdle = true;

    private bool _isDead;

    private void Awake()
    {
        if (_audioSource == null)
        {
            Debug.LogError($"{nameof(EnemySoundManager)}: AudioSource is not assigned.", this);
            return;
        }

        _audioSource.playOnAwake = false;
        _audioSource.volume = _volume;
    }

    // -------------------------
    // PUBLIC API
    // -------------------------

    public void PlayIdle()
    {
        if (_isDead || _audioSource == null || _idleSound == null)
            return;

        if (_audioSource.clip == _idleSound && _audioSource.isPlaying)
            return;

        _audioSource.Stop();
        _audioSource.loop = _loopIdle;
        _audioSource.clip = _idleSound;
        _audioSource.volume = .25f;
        _audioSource.pitch = .5f;
        _audioSource.Play();
    }

    public void PlayReceiveHit()
    {
        if (_isDead || _audioSource == null || _receiveHitSound == null)
            return;

        _audioSource.loop = false;
        _audioSource.volume = 1f;
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(_receiveHitSound, _volume);
    }

    public void PlayWakeUp()
    {
        if (_isDead || _audioSource == null || _wakeUpSound == null)
            return;

        _audioSource.loop = false;
        _audioSource.volume = 1f;
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(_wakeUpSound, _volume);
    }

    public void PlayDie()
    {
        if (_isDead || _audioSource == null || _dieSound == null)
            return;

        _isDead = true;

        _audioSource.Stop();
        _audioSource.loop = false;
        _audioSource.volume = 1f;
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(_dieSound, _volume);
    }
}
