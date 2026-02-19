using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoOnBox : MonoBehaviour
{

    #region Fields
    [SerializeField] private GameObject[] _ammoBoxesGO;
    [SerializeField] private HeroDetector _heroDetector;
    #endregion

    #region public properties    
    #endregion

    #region Private properties
    private int _id = 0;
    private GameObject _activeAmmoBoxGO;
    private HeroFSM _heroFSM;
    #endregion
    #region Unity Callbacks
    void Start()
    {
        ShowOneRandomAmmoBox();
        _heroDetector.detectedAction += HandleDetected;
        _heroDetector.undetectedAction += HandleUndetectedAction;
    }

    void OnDestroy()
    {
        _heroDetector.detectedAction -= HandleDetected;
        _heroDetector.undetectedAction -= HandleUndetectedAction;
    }
    #endregion

    #region Public methods
    public void SetId(int id)
    {
        _id = id;
    }

    public int GetId()
    {
        return _id;
    }
    public void ShowOneRandomAmmoBox()
    {
        if (_ammoBoxesGO == null || _ammoBoxesGO.Length == 0)
        {
            Debug.LogWarning("No ammo boxes assigned.");
            return;
        }

        // Deactivate all first
        for (int i = 0; i < _ammoBoxesGO.Length; i++)
        {
            if (_ammoBoxesGO[i] != null)
                _ammoBoxesGO[i].SetActive(false);
        }

        // Pick random index
        int randomIndex = Random.Range(0, _ammoBoxesGO.Length);

        // Activate selected one
        if (_ammoBoxesGO[randomIndex] != null)
        {
            _ammoBoxesGO[randomIndex].SetActive(true);
            _activeAmmoBoxGO = _ammoBoxesGO[randomIndex];
        }
    }

    public void MakeGlowOn()
    {
        _activeAmmoBoxGO.GetComponent<GlowController>().EnableGlow();
    }

    public void MakeGlowOff()
    {
        _activeAmmoBoxGO.GetComponent<GlowController>().DisableGlow();
    }


	#endregion
    
    #region Private methods
    private void HandleDetected(GameObject hero)
    {
        _heroFSM = hero.GetComponentInParent<HeroFSM>();
        _heroFSM.DetectAmmoBox(_activeAmmoBoxGO);

        MakeGlowOn();
    }

    private void HandleUndetectedAction()
    {
        _heroFSM.UnDetectAmmoBox();
        MakeGlowOff();
    }



    #endregion
}
