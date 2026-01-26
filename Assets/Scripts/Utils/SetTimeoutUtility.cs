using System;
using System.Collections;
using UnityEngine;

public class SetTimeoutUtility : IDisposable
{
    private MonoBehaviour context;
    private Coroutine runningCoroutine;
    private bool disposed = false;

    public SetTimeoutUtility(MonoBehaviour c)
    {
        context = c;
    }

    public void SetTimeout(Action fn, float delay)
    {
        if (disposed) 
        {
            Debug.LogWarning("SetTimeoutUtility: Attempted to use after Dispose(). Ignored.");
            return;
        }

        StopTimeout(); // ensure only 1 timeout at a time
        runningCoroutine = context.StartCoroutine(_SetTimeout(fn, delay));
    }

    private IEnumerator _SetTimeout(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!disposed)
        {
            callback?.Invoke();
        }
        runningCoroutine = null; // Auto-clear when done
    }

    public void StopTimeout()
    {
        if (runningCoroutine != null && context != null)
        {
            context.StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }
    }

    public void Dispose()
    {
        if (disposed) return;

        // Stop running coroutine safely
        StopTimeout();

        // Release reference to MonoBehaviour
        context = null;

        disposed = true;
    }
}
