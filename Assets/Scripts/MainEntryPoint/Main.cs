using UnityEngine;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    #region Fields
    [SerializeField] private World _world;
    [SerializeField] private HeroManager _heroManager;
    [SerializeField] private CheckpointManager _checkpointManager;
    [SerializeField] private PuzzlesManager _puzzlesManager;
    //[SerializeField] private WaveManager _waveManager;
    [SerializeField] private EnemyWaveManager _enemyWaveManager;
    #endregion

    #region Private Fields  //private variables not SerializeFields
    #endregion

    #region Properties
    public int test = -1;
    public int testDeactivate = -1;
    public int reset = -1;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _world.WorldCreationFinishedAction += HandleWorldCreationFinished;
        _heroManager.HeroDiedAction += HandleHeroDied;
        _puzzlesManager.PuzzleSolvedAction += HandlePuzzleSolved;
        //_waveManager.WaveFinishedAction += HandleWaveFinished;

        _world.GenerateWorld();        
    }

    void Update()
    {
        /* if (test != -1)
        {
            //AssignNewCurrentCheckpoint(test);
            //RestartHeroAtCheckpoint();
            ActivatePuzzle(test);
            test = -1;
        }

        if (testDeactivate != -1)
        {
            //AssignNewCurrentCheckpoint(test);
            //RestartHeroAtCheckpoint();
            DeactivatePuzzle(test);
            testDeactivate = -1;
        }

        if (reset != -1)
        {
            //AssignNewCurrentCheckpoint(test);
            //RestartHeroAtCheckpoint();
            ResetPuzzle(reset);
            reset = -1;
        } */

        

        
    }

    void OnDestroy()
    {
        _world.WorldCreationFinishedAction -= HandleWorldCreationFinished;
        _heroManager.HeroDiedAction -= HandleHeroDied;
        _puzzlesManager.PuzzleSolvedAction -= HandlePuzzleSolved;
        //_waveManager.WaveFinishedAction -= HandleWaveFinished;
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void HandleWorldCreationFinished()
    {
        _checkpointManager.AddCheckpoint(0,_world.GetStartPoint());
        List<PuzzleData> puzzlesDataList = _world.GetPuzzleDataList();
        PuzzleData firstPuzzle = new PuzzleData();
        puzzlesDataList.ForEach(pData =>
        {
            _checkpointManager.AddCheckpoint(pData.id,pData.Checkpoint.transform.position);

            _puzzlesManager.RegisterPuzzle(pData);
            if(pData.id == 1) //getting the first puzzle
            {
                firstPuzzle = pData;
            }

        });

        _puzzlesManager.DeactivateAllPuzzlesBut(firstPuzzle.id);
        _heroManager.CreateHero(_checkpointManager.GetCurrentCheckpointPosition());

        PrepareWaves();
    }

    private void PrepareWaves()
    {   
        Transform heroTransform = _heroManager.GetHeroTransform();
        _enemyWaveManager.SetHeroTransform(heroTransform);

        _enemyWaveManager.CreateInfoWaves();
        _enemyWaveManager.ConvertPositions((x, y) =>
        {
            return _world.GetPosition(x,y);
        });

        _enemyWaveManager.PrepareEnemies();
        

        //_enemyWaveManager.DebugWaves();
    }

    private void HandleHeroDied()
    {
        //get current checkpoint
        RestartHeroAtCheckpoint();
    }

    private void HandleWaveFinished(int idWave)
    {
        if(idWave < 6)
        {
            _puzzlesManager.ActivatePuzzle(idWave);
        }
        else
        {
            Debug.Log("OPEN THE EXIT");
            Debug.Log("Throw the key");
            //_labyrinthCreator.OpenExitFence();

        }
        
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

    private void ActivatePuzzle(int idPuzzle)
    {
        _puzzlesManager.ActivatePuzzle(idPuzzle);
    }

    private void DeactivatePuzzle(int idPuzzle)
    {
        _puzzlesManager.DeactivatePuzzle(idPuzzle);
    }
    private void ResetPuzzle(int idPuzzle)
    {
        _puzzlesManager.ResetPuzzle(idPuzzle);
    }

    private void HandlePuzzleSolved(int idPuzzle)
    {
        Debug.Log("Puzzle solved " + idPuzzle);

        _enemyWaveManager.RunWave(idPuzzle);
    }

    
    #endregion
}