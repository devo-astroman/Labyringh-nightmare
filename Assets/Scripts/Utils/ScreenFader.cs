using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;

    private Tween _fadeTween;

    private void Awake()
    {
        if (_fadeImage != null)
        {
            Color c = _fadeImage.color;
            c.a = 0f;
            _fadeImage.color = c;
        }
    }

    public void FadeToBlack(float duration = 1f)
    {
        Debug.Log("_FadeToBlack_");
        
        if (_fadeImage == null) return;

        KillTween();
        
        _fadeTween = _fadeImage
            .DOFade(1f, duration)
            .SetEase(Ease.Linear);
    }

    public void FadeFromBlack(float duration = 1f)
    {
        if (_fadeImage == null) return;

        KillTween();

        _fadeTween = _fadeImage
            .DOFade(0f, duration)
            .SetEase(Ease.Linear);
    }

    private void KillTween()
    {
        if (_fadeTween != null && _fadeTween.IsActive())
        {
            _fadeTween.Kill();
        }
    }
}