using UnityEngine;
using System;

public class HeroManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _heroPrefab;
    [SerializeField] private Transform _generatedTransform;
    #endregion

    #region Private Fields  //private variables not SerializeFields
    private GameObject _hero;
    private Transform _heroParent;
    private Transform _heroTransform;
    #endregion

    #region Properties
    #endregion

    #region Events
    public Action HeroDiedAction;
    public Action<Vector3, Vector3, RaycastHit> FireAction;

    
    #endregion

    #region Unity Callbacks
    void OnDestroy()
    {
        if (_hero)
        {
            HeroFSM _heroFSM = _hero.GetComponentInChildren<HeroFSM>();
            _heroFSM.HeroDiedAction -= HandleHeroDied;
            _heroFSM.onFireAction -= HandleFireAction;
        }
        
    }
    #endregion

    #region Public Methods
    public void CreateHero(Vector3 position)
    {
        _hero = Instantiate(_heroPrefab, position, Quaternion.identity,_heroParent);

        //Camera camera = hero.GetComponentInChildren<Camera>();
        HeroFSM _heroFSM = _hero.GetComponentInChildren<HeroFSM>();
        _heroTransform = _heroFSM.transform.Find("Hero");

        //_heroFSM.onFireAction+= HandleOnFireAction;
        _heroFSM.HeroDiedAction += HandleHeroDied;
        _heroFSM.onFireAction += HandleFireAction;
           // PrepareWaves();
    }

    public void ResetHeroAt(Vector3 position)
    {
        HeroFSM _heroFSM = _hero.GetComponentInChildren<HeroFSM>();
        _heroFSM.HeroDiedAction -= HandleHeroDied;
        Destroy(_hero);
        CreateHero(position);
    }

    public Transform GetHeroTransform()
    {
        return _heroTransform;
    }

    public int GetHeroAmmoInPocket()
    {
        HeroFSM _heroFSM = _hero.GetComponentInChildren<HeroFSM>();
        return _heroFSM.GetAmmoInpocket();
    }
    #endregion

    #region Private Methods
    private void HandleHeroDied()
    {
        HeroDiedAction?.Invoke();
    }

    private void HandleFireAction(Vector3 hitPoint, Vector3 hitNormal,RaycastHit hit)
    {
        FireAction?.Invoke(hitPoint, hitNormal,hit);
    }

    
    #endregion
}