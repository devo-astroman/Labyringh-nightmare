using UnityEngine;
using System.Collections.Generic;
public class Main : MonoBehaviour
{
    #region Fields
    [SerializeField] World _world;
    [SerializeField] HeroManager _heroManager;
    #endregion

    #region Private Fields  //private variables not SerializeFields
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _world.WorldCreationFinishedAction += HandleWorldCreationFinished;

        _world.GenerateWorld();        
    }

    void OnDestroy()
    {
        _world.WorldCreationFinishedAction -= HandleWorldCreationFinished;
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void HandleWorldCreationFinished()
    {

        GameObject hero = _heroManager.CrateHero(_world.GetStartPoint());



        /* Debug.Log("start point " + _world.GetStartPoint());

        List<PuzzleData> list = _world.GetPuzzleDataList();
        Debug.Log("puzzles data " + _world.GetPuzzleDataList());
        list.ForEach(pData =>
        {
            Debug.Log("__");
            Debug.Log(pData.id);
            Debug.Log(pData.Checkpoint.transform.position);
            Debug.Log(pData.Puzzle.transform.position);
            Debug.Log("__");
            
        });
        Debug.Log("flag "); */
    }
    #endregion
}