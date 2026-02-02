using System;
using System.Collections;
using UnityEngine;

public class SetIntervalUtility : IDisposable
{
    private MonoBehaviour context;
    private Coroutine runningCoroutine;
    private bool disposed;

    public SetIntervalUtility(MonoBehaviour c)
    {
        context = c;
    }

    public void SetInterval(Action fn, float interval)
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(SetIntervalUtility));

        StopInterval(); // ensure only 1 interval runs at a time
        runningCoroutine = context.StartCoroutine(_SetInterval(fn, interval));
    }

    private IEnumerator _SetInterval(Action callback, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            callback?.Invoke();
        }
    }

    public void StopInterval()
    {
        if (runningCoroutine != null && context != null)
        {
            context.StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }
    }

    // -------------------------
    // IDisposable Implementation
    // -------------------------

    public void Dispose()
    {
        if (disposed) return;

        StopInterval();

        context = null;
        disposed = true;
    }
}
