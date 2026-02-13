using UnityEngine;
using System.Collections;

public class SingleMaskAnimatorController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [Header("Mask Layer")]
    [SerializeField] private string _maskLayerName = "HitReaction";

    [Header("Blend Settings")]
    [SerializeField] private float _blendInTime = 0.05f;
    [SerializeField] private float _blendOutTime = 0.15f;

    private int _maskLayerIndex;
    private Coroutine _blendRoutine;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _maskLayerIndex = _animator.GetLayerIndex(_maskLayerName);

        if (_maskLayerIndex == -1)
            Debug.LogError($"Animator Layer '{_maskLayerName}' not found!");
    }

    // ------------------------------------------------
    // PUBLIC API
    // ------------------------------------------------

    /// <summary>
    /// Applies the mask immediately (weight = 1).
    /// </summary>
    public void ApplyInstant()
    {
        StopBlend();
        SetWeight(1f);
    }

    /// <summary>
    /// Clears the mask immediately (weight = 0).
    /// </summary>
    public void ClearInstant()
    {
        StopBlend();
        SetWeight(0f);
    }

    /// <summary>
    /// Smoothly applies the mask.
    /// </summary>
    public void ApplySmooth()
    {
        StartBlend(1f, _blendInTime);
    }

    /// <summary>
    /// Smoothly removes the mask.
    /// </summary>
    public void ClearSmooth()
    {
        StartBlend(0f, _blendOutTime);
    }

    /// <summary>
    /// Typical "hit reaction": fade in, hold, fade out.
    /// </summary>
    public void PlayHit(float holdTime = 0.05f)
    {
        StopBlend();
        _blendRoutine = StartCoroutine(HitRoutine(holdTime));
    }

    // ------------------------------------------------
    // INTERNAL
    // ------------------------------------------------

    private IEnumerator HitRoutine(float holdTime)
    {
        yield return BlendTo(1f, _blendInTime);
        yield return new WaitForSeconds(holdTime);
        yield return BlendTo(0f, _blendOutTime);
    }

    private void StartBlend(float target, float time)
    {
        StopBlend();
        _blendRoutine = StartCoroutine(BlendTo(target, time));
    }

    private IEnumerator BlendTo(float target, float duration)
    {
        float start = _animator.GetLayerWeight(_maskLayerIndex);
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            SetWeight(Mathf.Lerp(start, target, t / duration));
            yield return null;
        }

        SetWeight(target);
    }

    private void StopBlend()
    {
        if (_blendRoutine != null)
        {
            StopCoroutine(_blendRoutine);
            _blendRoutine = null;
        }
    }

    private void SetWeight(float value)
    {
        if (_maskLayerIndex != -1)
            _animator.SetLayerWeight(_maskLayerIndex, Mathf.Clamp01(value));
    }
}
