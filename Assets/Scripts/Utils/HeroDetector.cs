using System;
using UnityEngine;

public class HeroDetector : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private bool _useTag = true;
    [SerializeField] private string _heroTag = "HeroTag";
    [SerializeField] private string _heroLayerName = "HeroCollider";

    // Action to notify detection
    public Action<GameObject> detectedAction;

    private int _heroLayer;

    private void Awake()
    {
        _heroLayer = LayerMask.NameToLayer(_heroLayerName);

        if (!_useTag && _heroLayer == -1)
        {
            Debug.LogError($"HeroDetector: Layer '{_heroLayerName}' not found.", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsHero(other.gameObject))
        {
            detectedAction?.Invoke(other.gameObject);
        }
    }

    // Optional: continuous detection
    private void OnTriggerStay(Collider other)
    {
        if (IsHero(other.gameObject))
        {
            detectedAction?.Invoke(other.gameObject);
        }
    }

    private bool IsHero(GameObject obj)
    {
        if (_useTag)
            return obj.CompareTag(_heroTag);

        return obj.layer == _heroLayer;
    }
}
