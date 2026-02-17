using UnityEngine;
using System;


public class AnimatorEventHandler : MonoBehaviour
{

    public Action<int> FireEvent1Action;
    public Action<int> FireEvent2Action;
    public Action<int> FireEvent3Action;
    public Action<int> FireEvent4Action;

    public Action<int> FireEvent5Action;

    public Action<int> FireEvent6Action;


    public void OnFireEvent1(int id)
    {
        FireEvent1Action?.Invoke(id);
    }

    public void OnFireEvent2(int id)
    {
        FireEvent2Action?.Invoke(id);
    }

    public void OnFireEvent3(int id)
    {
        FireEvent3Action?.Invoke(id);
    }

    public void OnFireEvent4(int id)
    {
        FireEvent4Action?.Invoke(id);
    }

    public void OnFireEvent5(int id)
    {
        FireEvent5Action?.Invoke(id);
    }

    public void OnFireEvent6(int id)
    {
        FireEvent6Action?.Invoke(id);
    }

}
