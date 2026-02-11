using System;
using UnityEngine;

public class HeroDetectorManager : MonoBehaviour
{
    [SerializeField] private HeroDetector _farDetector;
    [SerializeField] private HeroDetector _nearDetector;

    public Action<GameObject> FarHeroDetectionAction;
    public Action FarHeroUndetectionAction;
    public Action<GameObject> NearHeroDetectionAction;
    public Action NearHeroUndetectionAction;

    public Action<int,GameObject> DetectionInfoAction;

    void Start()
    {
        _farDetector.detectedAction += HandleFarDetected;
        _farDetector.undetectedAction += HandleFarUndetected;

        _nearDetector.detectedAction += HandleNearDetected;
        _nearDetector.undetectedAction += HandleFaNeardetected;
    }

    void OnDestroy()
    {
        _farDetector.detectedAction -= HandleFarDetected;
        _farDetector.undetectedAction -= HandleFarUndetected;

        _nearDetector.detectedAction += HandleNearDetected;
        _nearDetector.undetectedAction += HandleFaNeardetected;
    }

    private void HandleFarDetected(GameObject go)
    {
        FarHeroDetectionAction?.Invoke(go);
        DetectionInfoAction?.Invoke(0,go);
    }

    private void HandleFarUndetected()
    {
        FarHeroUndetectionAction?.Invoke();
        DetectionInfoAction?.Invoke(1,null);
    }

    private void HandleNearDetected(GameObject go)
    {
        NearHeroDetectionAction?.Invoke(go);
        DetectionInfoAction?.Invoke(2,go);
    }

    private void HandleFaNeardetected()
    {
        NearHeroUndetectionAction?.Invoke();
        DetectionInfoAction?.Invoke(3,null);
    }


}
