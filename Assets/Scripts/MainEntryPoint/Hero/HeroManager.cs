using UnityEngine;

public class HeroManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _heroPrefab;
    [SerializeField] private Transform _generatedTransform;
    #endregion

    #region Private Fields  //private variables not SerializeFields
    private Transform _heroParent;
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks    
    #endregion

    #region Public Methods
    public GameObject CrateHero(Vector3 position)
    {

        GameObject hero = Instantiate(_heroPrefab, position, Quaternion.identity,_heroParent);

        return hero;

        //Camera camera = hero.GetComponentInChildren<Camera>();
        /* _heroFSM = hero.GetComponentInChildren<HeroFSM>();
        _heroFSM.onFireAction+= HandleOnFireAction;
        _heroFSM.HeroDiedAction += HandleHeroDied; */
           // PrepareWaves();

        
    }
    #endregion

    #region Private Methods
    #endregion
}