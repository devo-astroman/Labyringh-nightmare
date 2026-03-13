using UnityEngine;
using System;

public class HeroManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _heroPrefab;
    [SerializeField] private Transform _generatedTransform;
    #endregion

    #region Private Fields  //private variables not SerializeFields
    private GameObject hero;
    private Transform _heroParent;
    #endregion

    #region Properties
    #endregion

    #region Events
    public Action HeroDiedAction;
    #endregion

    #region Unity Callbacks
    void OnDestroy()
    {
        if (hero)
        {
            HeroFSM _heroFSM = hero.GetComponentInChildren<HeroFSM>();
            _heroFSM.HeroDiedAction -= HandleHeroDied;    
        }
        
    }
    #endregion

    #region Public Methods
    public void CreateHero(Vector3 position)
    {
        hero = Instantiate(_heroPrefab, position, Quaternion.identity,_heroParent);

        //Camera camera = hero.GetComponentInChildren<Camera>();
        HeroFSM _heroFSM = hero.GetComponentInChildren<HeroFSM>();
        //_heroFSM.onFireAction+= HandleOnFireAction;
        _heroFSM.HeroDiedAction += HandleHeroDied;
           // PrepareWaves();
    }

    public void ResetHeroAt(Vector3 position)
    {
        HeroFSM _heroFSM = hero.GetComponentInChildren<HeroFSM>();
        _heroFSM.HeroDiedAction -= HandleHeroDied;
        Destroy(hero);
        CreateHero(position);
    }
    #endregion

    #region Private Methods
    private void HandleHeroDied()
    {
        HeroDiedAction?.Invoke();
    }
    #endregion
}