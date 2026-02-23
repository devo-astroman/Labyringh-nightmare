using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InterpolatorMover : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private Transform _objectToMove;

    [Header("Events")]
    [SerializeField] private UnityEvent _onMoveStarted;
    [SerializeField] private UnityEvent _onMoveFinished;

    #endregion

    #region Private properties
    private Coroutine _moveRoutine;
    private bool _isMoving = false;
    #endregion

    #region Public API

    /// <summary>
    /// Moves from current position to target position over duration.
    /// </summary>
    public void MoveTo(Vector3 targetPosition, float duration)
    {
        
        if (_objectToMove == null)
        {
            Debug.LogWarning($"{nameof(InterpolatorMover)}: Missing object to move.");
            return;
        }

        if (_isMoving) return;

        if (duration <= 0f)
        {
            _onMoveStarted?.Invoke();
            MoveStarted?.Invoke();

            _objectToMove.position = targetPosition;

            _onMoveFinished?.Invoke();
            MoveFinished?.Invoke();
            return;
        }

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        
        _isMoving = true;
        _moveRoutine = StartCoroutine(MoveRoutine(_objectToMove.position, targetPosition, duration));
    }

    /// <summary>
    /// Moves from point A to B over duration.
    /// </summary>
    public void MoveFromTo(Vector3 start, Vector3 end, float duration)
    {
        if (_objectToMove == null)
            return;

        _objectToMove.position = start;
        MoveTo(end, duration);
    }

    public void MoveToAndReturn(Vector3 start, Vector3 end, float duration)
    {
        if (_objectToMove == null)
            return;
        
        if (_isMoving) return;

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

                _isMoving = true;
        _moveRoutine = StartCoroutine(MoveToAndReturnRoutine(start, end, duration));
    }


    public event Action MoveStarted;
    public event Action MoveFinished;

    #endregion

    #region Private methods

    private IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, float duration)
    {
        _onMoveStarted?.Invoke();
        MoveStarted?.Invoke();

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            float easedT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
            _objectToMove.position = Vector3.Lerp(startPos, endPos, easedT);

            yield return null;
        }

        _objectToMove.position = endPos;

        _moveRoutine = null;
        _isMoving = false;

        _onMoveFinished?.Invoke();
        MoveFinished?.Invoke();
    }

    private IEnumerator MoveToAndReturnRoutine(Vector3 startPos, Vector3 endPos, float duration)
    {
        _onMoveStarted?.Invoke();
        MoveStarted?.Invoke();

        _objectToMove.position = startPos;

        // A → B
        yield return StartCoroutine(InternalMove(startPos, endPos, duration));
        // B → A
        yield return StartCoroutine(InternalMove(endPos, startPos, duration));

        _moveRoutine = null;
        _isMoving = false;

        _onMoveFinished?.Invoke();
        MoveFinished?.Invoke();
    }


    private IEnumerator InternalMove(Vector3 startPos, Vector3 endPos, float duration)
    {
        if (duration <= 0f)
        {
            _objectToMove.position = endPos;
            yield break;
        }

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            float easedT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
            _objectToMove.position = Vector3.Lerp(startPos, endPos, easedT);

            yield return null;
        }

        _objectToMove.position = endPos;
    }


    #endregion
}
