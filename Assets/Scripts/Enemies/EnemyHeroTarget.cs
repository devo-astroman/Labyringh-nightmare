using UnityEngine;

public class EnemyHeroTarget : MonoBehaviour
{
    public Transform CurrentTransform { get; private set; }
    [SerializeField] public GameObject Current;

    public void SetCurrentTransform(Transform heroTransform)
    {
        CurrentTransform = heroTransform;

        Current = CurrentTransform.gameObject;
    }
}