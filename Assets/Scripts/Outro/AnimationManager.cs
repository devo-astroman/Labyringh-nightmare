using UnityEngine;
using DG.Tweening;

public class AnimationManager : MonoBehaviour
{   
    #region Fields
    [SerializeField] GameObject _heroGO;
    [SerializeField] Transform _destiny;
    #endregion

    #region Private properties
    #endregion

    #region Public methods    
    public void PlayHeroAnimation()
    {
        _heroGO.transform.DOMove(_destiny.position,8f).SetEase(Ease.Linear);
    }
    #endregion
}