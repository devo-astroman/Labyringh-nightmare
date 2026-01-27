using UnityEngine;
using System.Collections;

public class LayerHeroAnimatorController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [Header("Layer Names")]
    [SerializeField] private string _baseLayerName = "Base Layer";
    [SerializeField] private string _runLayerName = "Run";
    [SerializeField] private string _walkLayerName = "Walk";
    [SerializeField] private string _crouchLayerName = "Crouch";

    [Header("Blend Settings")]
    [SerializeField] private float _layerBlendTime = 0.2f;

    private int _baseLayerIndex;
    private int _runLayerIndex;
    private int _walkLayerIndex;
    private int _crouchLayerIndex;

    private int _currentLayer = -1;
    private Coroutine _blendRoutine;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _baseLayerIndex = _animator.GetLayerIndex(_baseLayerName);
        _runLayerIndex = _animator.GetLayerIndex(_runLayerName);
        _walkLayerIndex = _animator.GetLayerIndex(_walkLayerName);
        _crouchLayerIndex = _animator.GetLayerIndex(_crouchLayerName);

        ValidateLayer(_baseLayerIndex, _baseLayerName);
        ValidateLayer(_runLayerIndex, _runLayerName);
        ValidateLayer(_walkLayerIndex, _walkLayerName);
        ValidateLayer(_crouchLayerIndex, _crouchLayerName);

        SetBaseInstant();
    }

    private void ValidateLayer(int index, string name)
    {
        if (index == -1)
            Debug.LogError($"Animator Layer '{name}' not found!");
    }

    // -------------------------
    // PUBLIC API (Smooth)
    // -------------------------

    public void SetBase()   => BlendToLayer(_baseLayerIndex);
    public void SetRun()    => BlendToLayer(_runLayerIndex);
    public void SetWalk()   => BlendToLayer(_walkLayerIndex);
    public void SetCrouch() => BlendToLayer(_crouchLayerIndex);

    // -------------------------
    // PUBLIC API (Instant)
    // -------------------------

    public void SetBaseInstant()   => SetExclusiveLayerInstant(_baseLayerIndex);
    public void SetRunInstant()    => SetExclusiveLayerInstant(_runLayerIndex);
    public void SetWalkInstant()   => SetExclusiveLayerInstant(_walkLayerIndex);
    public void SetCrouchInstant() => SetExclusiveLayerInstant(_crouchLayerIndex);

    // -------------------------
    // INTERNAL
    // -------------------------

    private void BlendToLayer(int targetLayer)
    {
        if (targetLayer == -1) return;
        if (_currentLayer == targetLayer) return;

        if (_blendRoutine != null)
            StopCoroutine(_blendRoutine);

        _blendRoutine = StartCoroutine(BlendRoutine(targetLayer));
    }

    private IEnumerator BlendRoutine(int targetLayer)
    {
        float timer = 0f;

        float startBase   = _animator.GetLayerWeight(_baseLayerIndex);
        float startRun    = _animator.GetLayerWeight(_runLayerIndex);
        float startWalk   = _animator.GetLayerWeight(_walkLayerIndex);
        float startCrouch = _animator.GetLayerWeight(_crouchLayerIndex);

        while (timer < _layerBlendTime)
        {
            timer += Time.deltaTime;
            float t = timer / _layerBlendTime;

            SetLayerWeight(_baseLayerIndex,   Mathf.Lerp(startBase,   targetLayer == _baseLayerIndex ? 1f : 0f, t));
            SetLayerWeight(_runLayerIndex,    Mathf.Lerp(startRun,    targetLayer == _runLayerIndex ? 1f : 0f, t));
            SetLayerWeight(_walkLayerIndex,   Mathf.Lerp(startWalk,   targetLayer == _walkLayerIndex ? 1f : 0f, t));
            SetLayerWeight(_crouchLayerIndex, Mathf.Lerp(startCrouch, targetLayer == _crouchLayerIndex ? 1f : 0f, t));

            yield return null;
        }

        // Snap final values for precision
        SetExclusiveLayerInstant(targetLayer);

        _currentLayer = targetLayer;
        _blendRoutine = null;
    }

    private void SetExclusiveLayerInstant(int activeLayer)
    {
        if (activeLayer == -1) return;

        SetLayerWeight(_baseLayerIndex,   0f);
        SetLayerWeight(_runLayerIndex,    0f);
        SetLayerWeight(_walkLayerIndex,   0f);
        SetLayerWeight(_crouchLayerIndex, 0f);

        SetLayerWeight(activeLayer, 1f);

        _currentLayer = activeLayer;
    }

    private void SetLayerWeight(int index, float value)
    {
        if (index != -1)
            _animator.SetLayerWeight(index, value);
    }
}
