using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void Reset();
}