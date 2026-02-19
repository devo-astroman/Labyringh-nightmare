using System;
using UnityEngine;

public static class RandomRoomPositions
{
    // Reused pool: 0..8 (9 values). Not thread-safe; fine on Unity main thread.
    private static readonly int[] _pool = new int[9];

    /// <summary>
    /// Returns nIndexes unique random ints in [0..8]. nIndexes must be [1..8].
    /// Allocates a new int[] result.
    /// </summary>
    public static int[] GetRandomIndexes(int nIndexes)
    {
        if (nIndexes < 1 || nIndexes > 8)
        {
            Debug.LogError("GetRandomIndexes: nIndexes must be between 1 and 8.");
            return Array.Empty<int>();
        }

        // Fill pool 0..8
        for (int i = 0; i < 9; i++)
            _pool[i] = i;

        // Partial Fisher–Yates shuffle for first nIndexes entries
        for (int i = 0; i < nIndexes; i++)
        {
            int j = UnityEngine.Random.Range(i, 9); // [i..8]
            (_pool[i], _pool[j]) = (_pool[j], _pool[i]);
        }

        int[] result = new int[nIndexes];
        Array.Copy(_pool, result, nIndexes);
        return result;
    }

    /// <summary>
    /// Zero-allocation option: fills an existing buffer with unique random ints in [0..8].
    /// buffer.Length must be >= nIndexes.
    /// </summary>
    public static void FillRandomIndexes(int nIndexes, int[] buffer)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (nIndexes < 1 || nIndexes > 8) throw new ArgumentOutOfRangeException(nameof(nIndexes));
        if (buffer.Length < nIndexes) throw new ArgumentException("Buffer too small.", nameof(buffer));

        for (int i = 0; i < 9; i++)
            _pool[i] = i;

        for (int i = 0; i < nIndexes; i++)
        {
            int j = UnityEngine.Random.Range(i, 9); // [i..8]
            (_pool[i], _pool[j]) = (_pool[j], _pool[i]);
            buffer[i] = _pool[i];
        }
    }
}
