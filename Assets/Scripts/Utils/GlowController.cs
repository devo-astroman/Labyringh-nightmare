using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GlowController : MonoBehaviour
{
    [Header("Glow Settings")]
    [SerializeField] private Color _glowColor = Color.yellow;
    [SerializeField, Range(0f, 10f)] private float _intensity = 3f;

    [Header("Optional Pulse")]
    [SerializeField] private bool _usePulse = false;
    [SerializeField] private float _pulseSpeed = 2f;
    [SerializeField] private float _pulseAmplitude = 1.5f;

    private Renderer _renderer;
    private Material _materialInstance;
    private bool _isGlowing;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();

        // IMPORTANT: create material instance so we don’t modify shared material
        _materialInstance = _renderer.material;
        _materialInstance.EnableKeyword("_EMISSION");

        DisableGlow(); // start off
    }

    void Update()
    {
        if (_isGlowing && _usePulse)
        {
            float pulse = _intensity + Mathf.PingPong(Time.time * _pulseSpeed, _pulseAmplitude);
            _materialInstance.SetColor("_EmissionColor", _glowColor * pulse);
        }
    }

    // --------------------------------------------------
    // PUBLIC API
    // --------------------------------------------------

    public void EnableGlow()
    {
        _isGlowing = true;
        _materialInstance.SetColor("_EmissionColor", _glowColor * _intensity);
    }

    public void DisableGlow()
    {
        _isGlowing = false;
        _materialInstance.SetColor("_EmissionColor", Color.black);
    }
}
