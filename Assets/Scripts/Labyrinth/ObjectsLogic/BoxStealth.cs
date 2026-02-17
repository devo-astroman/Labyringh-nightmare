using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxStealth : MonoBehaviour
{
    
    #region Fields
    [SerializeField] private HeroDetector _heroDetector;
    #endregion

    #region public properties    
    #endregion

    #region Private properties
    private GameObject _detectedHeroGO;
    #endregion
    #region Unity Callbacks
    void Start()
    {
        _heroDetector.detectedAction += HandleDetected;
        _heroDetector.undetectedAction += HandleUndetected;
    }

    void OnDestroy()
    {
        _heroDetector.detectedAction -= HandleDetected;
        _heroDetector.undetectedAction -= HandleUndetected;
    }
	#endregion

    #region Public methods
	#endregion

    #region Private methods
    private void HandleDetected(GameObject go)
    {
        Debug.Log("BOX STEALTH DETECTED Parent: " + go.transform.parent);
        _detectedHeroGO = go;

        GameObject heroFSMGO = go.transform.parent.gameObject;
        if (heroFSMGO)
        {
            HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();

            if (heroFSM)
            {
                heroFSM.SetHiddenStealth();
            }
        }
    }

    private void HandleUndetected()
    {
        GameObject heroFSMGO = _detectedHeroGO.transform.parent.gameObject;
        if (heroFSMGO)
        {
            HeroFSM heroFSM = heroFSMGO.GetComponent<HeroFSM>();

            if (heroFSM)
            {
                heroFSM.SetShowStealth();
            }
        }
    }

    
	#endregion



}
