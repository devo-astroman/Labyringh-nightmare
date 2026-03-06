using UnityEngine;

public class MeshSwitcher : MonoBehaviour
{
    #region Fields
    private bool _isVisible = true;
    [SerializeField] MeshRenderer _meshRenderer;
    #endregion

    #region public properties
    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks
    void Start()
    {
        _isVisible = _meshRenderer.enabled;
    }
    #endregion

    #region Public methods
    public void Show()
    {
        _isVisible = true;
        UpdateMesh();
    }
    public void Hide()
    {
        _isVisible = false;
        UpdateMesh();
    }
    public void Switch()
    {
        _isVisible = !_isVisible;
        UpdateMesh();
    }

    public bool GetIsVisible()
    {
        return _isVisible;
    }
    public void SetIsVisible(bool isVisible)
    {
        _isVisible = isVisible;
        UpdateMesh();
    }
	#endregion
    
    #region Private methods
    private void UpdateMesh()
    {
        _meshRenderer.enabled = _isVisible;
    }
    #endregion
}
