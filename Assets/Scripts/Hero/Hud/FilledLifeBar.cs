using UnityEngine;
using UnityEngine.UI;

public class FilledLifeBar : MonoBehaviour
{
    [SerializeField] private Image _filledImage;

    private float _maxLife = 1f;
    private float _currentLife = 1f;

    private void Awake()
    {
        UpdateFill();
    }

    public void SetMax(float maxLife)
    {
        Debug.Log("SetMax " + maxLife);
        _maxLife = Mathf.Max(1f, maxLife);
        _currentLife = _maxLife;
        /* if (_currentLife > _maxLife)
        {
            _currentLife = _maxLife;
        } */

        UpdateFill();
    }

    public void SetCurrentLife(float currentLife)
    {
        Debug.Log("setting  "+ currentLife);
        Debug.Log("before "+ _currentLife);
        _currentLife = Mathf.Clamp(currentLife, 0f, _maxLife);
        Debug.Log("after "+ currentLife);
        UpdateFill();
    }

    private void UpdateFill()
    {
        if (_filledImage == null)
        {
            Debug.LogWarning("FilledLifeBar: _filledImage is not assigned.");
            return;
        }
        Debug.Log("Should fill with " + _currentLife / _maxLife);
        //_filledImage.fillAmount = _currentLife / _maxLife;
        _filledImage.fillAmount = _currentLife / _maxLife;
    }
}