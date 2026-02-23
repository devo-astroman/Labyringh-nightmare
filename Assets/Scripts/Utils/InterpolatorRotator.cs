using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InterpolatorRotator : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private Transform _objectToRotate; // better than GameObject for rotations

    [Header("Events")]
    [SerializeField] private UnityEvent _onRotationStarted;
    [SerializeField] private UnityEvent _onRotationFinished;
    #endregion

    #region Private properties
    private Coroutine _rotateRoutine;
    #endregion

    #region Public API
    /// <summary>
    /// Rotates the target by "degrees" (around Y by default) over "duration" seconds.
    /// </summary>
    public void RotateDegrees(float degrees, float duration)
    {
        if (_objectToRotate == null)
        {
            Debug.LogWarning($"{nameof(InterpolatorRotator)}: Missing object to rotate.");
            return;
        }

        if (duration <= 0f)
        {
            // Instant rotation
            _onRotationStarted?.Invoke();
            _objectToRotate.Rotate(0f,0f, degrees, Space.Self);
            _onRotationFinished?.Invoke();
            return;
        }

        // If already rotating, stop and start a new one (common UX)
        if (_rotateRoutine != null)
            StopCoroutine(_rotateRoutine);

        _rotateRoutine = StartCoroutine(RotateRoutine(degrees, duration));
    }

    /// <summary>
    /// Optional: code-based subscription (in addition to inspector UnityEvents).
    /// </summary>
    public event Action RotationStarted;
    public event Action RotationFinished;
    #endregion

    #region Private methods
    private IEnumerator RotateRoutine(float degrees, float duration)
    {
        _onRotationStarted?.Invoke();
        RotationStarted?.Invoke();

        Quaternion startRot = _objectToRotate.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0f, degrees, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // Smooth interpolation; change to t for linear if you prefer
            float easedT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
            _objectToRotate.rotation = Quaternion.Slerp(startRot, endRot, easedT);

            yield return null;
        }

        // Snap to exact end rotation to avoid floating point drift
        _objectToRotate.rotation = endRot;

        _rotateRoutine = null;

        _onRotationFinished?.Invoke();
        RotationFinished?.Invoke();
    }
    #endregion
}
