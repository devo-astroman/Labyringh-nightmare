using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GlowController : MonoBehaviour
{
    #region Fields

    [Header("Glow Settings")]
    [SerializeField] private Material GlowMaterial;

    #endregion

    #region Private Fields

    private Renderer _renderer;
    private Material _originalMaterial;
    private bool _isGlowing;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer != null)
        {
            // Store original material
            _originalMaterial = _renderer.material;
        }
    }

    #endregion

    #region Public Methods

    public void EnableGlow()
    {
        if (_renderer == null || GlowMaterial == null)
            return;

        if (_isGlowing)
            return;

        _renderer.material = GlowMaterial;
        _isGlowing = true;
    }

    public void DisableGlow()
    {
        if (_renderer == null)
            return;

        if (!_isGlowing)
            return;

        _renderer.material = _originalMaterial;
        _isGlowing = false;
    }

    #endregion
}
