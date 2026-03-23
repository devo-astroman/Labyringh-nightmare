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
    [SerializeField] private VFXsManager _vFXsManager;
    [SerializeField] private SoundsManager _soundsManager;
    [SerializeField] private Steps _steps;
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
        Debug.Log("Should set " + AllGameData.musicVolumen);
        _soundsManager.SetVolume(AllGameData.musicVolumen);

        _world.WorldCreationFinishedAction += HandleWorldCreationFinished;
        _heroManager.HeroDiedAction += HandleHeroDied;
        _heroManager.FireAction += HandleFire;
        _puzzlesManager.PuzzleSolvedAction += HandlePuzzleSolved;
        _enemyWaveManager.waveAllDeadAction += HandleWaveFinished;
        _steps.triggerReachedAction += HandleTriggerReached;
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
        _heroManager.FireAction -= HandleFire;
        _puzzlesManager.PuzzleSolvedAction -= HandlePuzzleSolved;
        _enemyWaveManager.waveAllDeadAction -= HandleWaveFinished;
        _steps.triggerReachedAction += HandleTriggerReached;
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

        _soundsManager.PlayExploreMusic();

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
        //RestartHeroAtCheckpoint();
        Vector3 checkpointPosition = _checkpointManager.GetCurrentCheckpointPosition();
        _heroManager.ResetHeroAt(checkpointPosition);

        //StopCurrentEnemyWave();
        Transform heroTransform = _heroManager.GetHeroTransform();
        _enemyWaveManager.SetHeroTransform(heroTransform);
        _enemyWaveManager.RestartWave();

        //RestartLastSolvedPuzzle();
        _puzzlesManager.ResetLastPuzzleSolved();
    }
    
    private void HandlePuzzleSolved(int idPuzzle)
    {
        Debug.Log("finished puzzle " + idPuzzle);
        _checkpointManager.SetCurrentCheckpoint(idPuzzle);
        _enemyWaveManager.RunWave(idPuzzle);
        _soundsManager.CrossfadeToWaveMusic();
    }

    private void HandleWaveFinished(int idWave)
    {
        if(idWave < 6)
        {
            _puzzlesManager.ActivatePuzzle(idWave+1);
        }
        else
        {
            Debug.Log("OPEN THE EXIT");
            Debug.Log("Throw the key");
            //_labyrinthCreator.OpenExitFence();
            _world.OpenExitFence();

        }

        _soundsManager.FadeMusicToStop();
        //_soundsManager.CrossfadeToWaveFinishedMusic();
        _soundsManager.PlayExploreMusicWithDelay(3f);

        
    }

    private void HandleFire(Vector3 hitPoint, Vector3 hitNormal,RaycastHit hit)
    {
        Debug.Log("gun fired!!!");
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyCollider"))
        {
            //Here should detect who is the enemy using the id of the EnemyT
            _vFXsManager.ShowBloodVFXs(hitPoint,hitNormal);
            //_enemyManager.ProcessDamage(0);

            Debug.Log("shoot: " + hit.collider.gameObject.name);
            EnemyB1FSM enemyB1FSM = hit.collider.gameObject.GetComponentInParent<EnemyB1FSM>();
            if (enemyB1FSM)
            {
                enemyB1FSM.ReceiveDamage(0);
            }
            else            
            {
                EB2FSM eB2FSM = hit.collider.gameObject.GetComponentInParent<EB2FSM>();
                if (eB2FSM)
                {
                    eB2FSM.ReceiveDamage(0);
                }
                else
                {
                    EB3FSM eB3FSM = hit.collider.gameObject.GetComponentInParent<EB3FSM>();
                    if (eB3FSM)
                    {
                        eB3FSM.ReceiveDamage(0);
                    }
                }
            }
        }
        else
        {
            _vFXsManager.ShowHitWallVFXs(hitPoint,hitNormal);
        }
        int ammoInPocket =  _heroManager.GetHeroAmmoInPocket();
        _world.UpdateAmmo(ammoInPocket);

    }

    private void HandleTriggerReached()
    {
        Debug.Log("Go to ending");
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

    private void RestartLastSolvedPuzzle()
    {
        _puzzlesManager.ResetLastPuzzleSolved();
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



    
    #endregion
}