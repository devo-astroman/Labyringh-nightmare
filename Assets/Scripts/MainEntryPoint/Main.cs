using UnityEngine;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    #region Fields
    [SerializeField] World _world;
    [SerializeField] HeroManager _heroManager;
    [SerializeField] CheckpointManager _checkpointManager;
    
    #endregion

    #region Private Fields  //private variables not SerializeFields
    #endregion

    #region Properties
    public int test = -1;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _world.WorldCreationFinishedAction += HandleWorldCreationFinished;
        _heroManager.HeroDiedAction += HandleHeroDied;

        _world.GenerateWorld();
    }

    void Update()
    {
        if (test != -1)
        {
            AssignNewCurrentCheckpoint(test);
            //RestartHeroAtCheckpoint();
            test = -1;
        }
    }

    void OnDestroy()
    {
        _world.WorldCreationFinishedAction -= HandleWorldCreationFinished;
        _heroManager.HeroDiedAction -= HandleHeroDied;
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void HandleWorldCreationFinished()
    {

        _checkpointManager.AddCheckpoint(0,_world.GetStartPoint());
        List<PuzzleData> list = _world.GetPuzzleDataList();        
        list.ForEach(pData =>
        {
            _checkpointManager.AddCheckpoint(pData.id,pData.Checkpoint.transform.position);
        });


        _heroManager.CreateHero( _checkpointManager.GetCurrentCheckpointPosition());
 
    }

    private void HandleHeroDied()
    {
        //get current checkpoint
        RestartHeroAtCheckpoint(); 

    }
    private void AssignNewCurrentCheckpoint(int checkpointId)
    {
        _checkpointManager.SetCurrentCheckpoint(checkpointId);
    }
    private void RestartHeroAtCheckpoint()
    {
        Vector3 checkpointPosition = _checkpointManager.GetCurrentCheckpointPosition();

        _heroManager.ResetHeroAt(checkpointPosition);
    }
    #endregion
}