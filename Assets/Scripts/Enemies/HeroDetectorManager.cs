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
        Debug.Log("FAR __ HandleFarDetected ");

        GameObject heroFSMGO = go.transform.parent.gameObject;
        if (heroFSMGO)
        {
            HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();

            if (heroFSM)
            {

                Debug.Log("FAR __ HandleFarDetected hidden " + heroFSM.IsHidden());

                if (!heroFSM.IsHidden())
                {
                    FarHeroDetectionAction?.Invoke(go);
                    DetectionInfoAction?.Invoke(0,go);
                }
                else
                {
                    Debug.Log("Ignore ");
                    //ignore cause the hero is hidden
                }
            }
        }
        
    }

    private void HandleFarUndetected()
    {
        FarHeroUndetectionAction?.Invoke();
        DetectionInfoAction?.Invoke(1,null);
    }

    private void HandleNearDetected(GameObject go)
    {
        GameObject heroFSMGO = go.transform.parent.gameObject;
        if (heroFSMGO)
        {
            HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();

            if (heroFSM)
            {
                Debug.Log("NEAR __ HandleFarDetected hidden " + heroFSM.IsHidden());

                if (!heroFSM.IsHidden())
                {
                    NearHeroDetectionAction?.Invoke(go);
                    DetectionInfoAction?.Invoke(2,go);
                }
                else
                {
                    Debug.Log("Ignore ");
                    //ignore cause the hero is hidden
                }
            }
        }
    }

    private void HandleFaNeardetected()
    {
        NearHeroUndetectionAction?.Invoke();
        DetectionInfoAction?.Invoke(3,null);
    }


}
