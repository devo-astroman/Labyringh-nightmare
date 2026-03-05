using UnityEngine;
using System;
using System.Collections.Generic;

public class PointerLookAtCycler : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform pointer;
    [SerializeField] private Transform targetsParent;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeedDegPerSec = 360f;
    [SerializeField] private float finishedAngleThresholdDeg = 0.5f; // how close is "done"

    [Header("Behavior")]
    [Tooltip("If true, Next/Prev calls are ignored while rotating.")]
    [SerializeField] private bool ignoreNextPrevWhileRotating = true;

    [Header("Debug")]
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private bool isRotating = false;

    /// <summary>Fired when the pointer finishes rotating to the current target.</summary>
    public event Action<int, Transform> OnRotationFinished;

    private readonly List<Transform> targets = new List<Transform>();
    private bool finishedEventFiredForCurrent = false;

    private void Awake()
    {
        if (pointer == null)
            pointer = transform;

        CollectTargets();
        ClampIndex();
    }

    private void CollectTargets()
    {
        targets.Clear();
        if (targetsParent == null) return;

        foreach (Transform child in targetsParent)
            targets.Add(child);
    }

    public void RefreshTargets()
    {
        CollectTargets();
        ClampIndex();
        MarkNeedsRotation();
    }

    private void ClampIndex()
    {
        if (targets.Count == 0) { currentIndex = 0; return; }
        currentIndex = Mathf.Clamp(currentIndex, 0, targets.Count - 1);
    }

    private void Update()
    {
        if (pointer == null || targets.Count == 0) return;

        Transform target = targets[currentIndex];
        if (target == null) return;

        Vector3 dir = target.position - pointer.position;

        // Lock rotation to Y axis (top-down)
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion desired = Quaternion.LookRotation(dir.normalized, Vector3.up);

        // Are we already close enough?
        float angleRemaining = Quaternion.Angle(pointer.rotation, desired);
        bool done = angleRemaining <= finishedAngleThresholdDeg;

        if (done)
        {
            // Snap (optional, keeps it exact)
            pointer.rotation = desired;

            if (isRotating)
            {
                isRotating = false;
                if (!finishedEventFiredForCurrent)
                {
                    finishedEventFiredForCurrent = true;
                    OnRotationFinished?.Invoke(currentIndex, target);
                }
            }
            return;
        }

        // Not done -> rotate smoothly
        isRotating = true;
        finishedEventFiredForCurrent = false;

        pointer.rotation = Quaternion.RotateTowards(
            pointer.rotation,
            desired,
            rotationSpeedDegPerSec * Time.deltaTime
        );
    }

    public void Next()
    {
        if (targets.Count == 0) return;
        if (ignoreNextPrevWhileRotating && isRotating) return;

        currentIndex = (currentIndex + 1) % targets.Count;
        MarkNeedsRotation();
    }

    public void Prev()
    {
        if (targets.Count == 0) return;
        if (ignoreNextPrevWhileRotating && isRotating) return;

        currentIndex = (currentIndex - 1 + targets.Count) % targets.Count;
        MarkNeedsRotation();
    }

    public void SnapToCurrent()
    {
        if (pointer == null || targets.Count == 0) return;

        Transform target = targets[currentIndex];
        if (target == null) return;

        Vector3 dir = target.position - pointer.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) return;

        pointer.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);

        isRotating = false;
        finishedEventFiredForCurrent = true;
        OnRotationFinished?.Invoke(currentIndex, target);
    }

    private void MarkNeedsRotation()
    {
        // When we change target, we want rotation to start (or continue) and allow event to fire.
        finishedEventFiredForCurrent = false;
        // isRotating will become true in Update() once it detects it's not done yet.
    }
}