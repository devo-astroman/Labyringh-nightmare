using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;

    private Tween _fadeTween;

    // Optional global event if needed
    public Action OnFadeFinished;

    private void Awake()
    {
        if (_fadeImage != null)
        {
            Color c = _fadeImage.color;
            c.a = 0f;
            _fadeImage.color = c;
        }
    }

    public void FadeToBlack(float duration = 1f, Action onComplete = null)
    {
        Debug.Log("_FadeToBlack_");

        if (_fadeImage == null) return;

        KillTween();

        _fadeTween = _fadeImage
            .DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                Debug.Log("Invoke OnFadeFinished");
                OnFadeFinished?.Invoke();
            });
    }

    public void FadeFromBlack(float duration = 1f, Action onComplete = null)
    {
        if (_fadeImage == null) return;

        KillTween();

        _fadeTween = _fadeImage
            .DOFade(0f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                OnFadeFinished?.Invoke();
            });
    }

    private void KillTween()
    {
        if (_fadeTween != null && _fadeTween.IsActive())
        {
            _fadeTween.Kill();
            _fadeTween = null;
        }
    }
}