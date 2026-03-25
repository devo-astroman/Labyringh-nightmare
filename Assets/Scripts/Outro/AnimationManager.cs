using UnityEngine;
using DG.Tweening;
using TMPro;

public class AnimationManager : MonoBehaviour
{   
    #region Fields
    [SerializeField] GameObject _heroGO;
    [SerializeField] Transform _destiny;

    [SerializeField] TextMeshProUGUI _thanksTMP;
    [SerializeField] GameObject _mainMenuGO;
    #endregion

    #region Public methods    

    public void PlayHeroAnimation()
    {
        _heroGO.transform
            .DOMove(_destiny.position, 8f)
            .SetEase(Ease.Linear);
    }

    public void PlayShowThanks()
    {
        if (_thanksTMP == null)
            return;

        // Start fully transparent
        Color c = _thanksTMP.color;
        c.a = 0f;
        _thanksTMP.color = c;

        // Fade to visible
        _thanksTMP
            .DOFade(1f, 2f) // 2 seconds fade
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                ShowMainMenuButton();
            });
    }
    #endregion

    #region private methods
    private void ShowMainMenuButton()
    {
        _mainMenuGO.SetActive(true);
    }
    #endregion

}