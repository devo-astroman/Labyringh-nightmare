using UnityEngine;

public class Minimap : MonoBehaviour
{   
    #region Fields
    [SerializeField] private Canvas _canvas;
    #endregion

    #region Private properties
    private Camera _cameraHero;
    #endregion

    #region Public methods
    public void SetCameraHero(Camera cameraHero)
    {
        _cameraHero = cameraHero;
        //_canvas.worldCamera = cameraHero;
    }
    #endregion
}