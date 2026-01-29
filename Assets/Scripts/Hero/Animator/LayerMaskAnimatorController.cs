using UnityEngine;
using System.Collections;

public class LayerMaskAnimatorController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [Header("Mask Layer Names")]
    [SerializeField] private string _maskLayerAName = "PointUp";
    [SerializeField] private string _maskLayerBName = "PointDown";

    [Header("Blend Settings")]
    [SerializeField] private float _blendTime = 0.2f;

    private int _maskLayerAIndex;
    private int _maskLayerBIndex;

    private Coroutine _blendARoutine;
    private Coroutine _blendBRoutine;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _maskLayerAIndex = _animator.GetLayerIndex(_maskLayerAName);
        _maskLayerBIndex = _animator.GetLayerIndex(_maskLayerBName);

        ValidateLayer(_maskLayerAIndex, _maskLayerAName);
        ValidateLayer(_maskLayerBIndex, _maskLayerBName);
    }

    private void ValidateLayer(int index, string name)
    {
        if (index == -1)
            Debug.LogError($"Animator Layer '{name}' not found!");
    }

    // -------------------------
    // PUBLIC API — INSTANT
    // -------------------------

    public void SetMaskAInstant(float weight)
    {
        SetLayerWeight(_maskLayerAIndex, Mathf.Clamp01(weight));
    }

    public void SetMaskBInstant(float weight)
    {
        SetLayerWeight(_maskLayerBIndex, Mathf.Clamp01(weight));
    }

    public void ClearMasksInstant()
    {
        SetMaskAInstant(0f);
        SetMaskBInstant(0f);
    }

    // -------------------------
    // PUBLIC API — SMOOTH
    // -------------------------

    public void SetMaskASmooth(float targetWeight)
    {
        BlendMask(_maskLayerAIndex, ref _blendARoutine, targetWeight);
    }

    public void SetMaskBSmooth(float targetWeight)
    {
        BlendMask(_maskLayerBIndex, ref _blendBRoutine, targetWeight);
    }

    public void ClearMasksSmooth()
    {
        SetMaskASmooth(0f);
        SetMaskBSmooth(0f);
    }

    // -------------------------
    // PUBLIC API — ADDITIVE CONTROL
    // -------------------------

    public void AddToMaskA(float delta)
    {
        float current = _animator.GetLayerWeight(_maskLayerAIndex);
        SetMaskASmooth(current + delta);
    }

    public void AddToMaskB(float delta)
    {
        float current = _animator.GetLayerWeight(_maskLayerBIndex);
        SetMaskBSmooth(current + delta);
    }

    // -------------------------
    // INTERNAL
    // -------------------------

    private void BlendMask(int layerIndex, ref Coroutine routine, float targetWeight)
    {
        if (layerIndex == -1) return;

        targetWeight = Mathf.Clamp01(targetWeight);

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(BlendRoutine(layerIndex, targetWeight));
    }

    private IEnumerator BlendRoutine(int layerIndex, float targetWeight)
    {
        float timer = 0f;
        float start = _animator.GetLayerWeight(layerIndex);

        while (timer < _blendTime)
        {
            timer += Time.deltaTime;
            float t = timer / _blendTime;

            SetLayerWeight(layerIndex, Mathf.Lerp(start, targetWeight, t));
            yield return null;
        }

        SetLayerWeight(layerIndex, targetWeight);
    }

    private void SetLayerWeight(int index, float value)
    {
        if (index != -1)
            _animator.SetLayerWeight(index, value);
    }
}
