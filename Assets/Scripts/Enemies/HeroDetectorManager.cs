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

    public bool justNotify = false;

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
        if (justNotify)
        {
            FarHeroDetectionAction?.Invoke(go);
            DetectionInfoAction?.Invoke(0,go);
            return;
        }


        GameObject heroFSMGO = go.transform.parent.gameObject;
        if (heroFSMGO)
        {
            HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();

            if (heroFSM)
            {

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
