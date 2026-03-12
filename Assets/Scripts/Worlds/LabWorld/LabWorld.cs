using UnityEngine;


public class LabWorld : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _playerHeroPrefab;
    [SerializeField] private GameObject _defaultCamera;

    [SerializeField] private HeroFSM _heroFSM;
    [SerializeField] private VFXsManager _vFXsManager;
    [SerializeField] private Minimap _minimap;
    [SerializeField] private CheckpointManager _checkpointManager;
    

//    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private LabyrinthCreator _labyrinthCreator;    

    private SetTimeoutUtility _timeoutToStart;
    private SetTimeoutUtility _timeoutToSpawnEnemies;
    private SetTimeoutUtility _timeoutToSpawnEnemies2;
    private SetTimeoutUtility _timeoutToSpawnEnemies3;
    private Vector3 _heroSpawnPoint;

    [SerializeField] private Transform _enemySpawnPointTransform;
    private Vector3 _enemySpawnPoint;
    



    void Start()
    {   
        _timeoutToStart = new SetTimeoutUtility(this);
        _timeoutToSpawnEnemies = new SetTimeoutUtility(this);
        _timeoutToSpawnEnemies2 = new SetTimeoutUtility(this);
        _timeoutToSpawnEnemies3 = new SetTimeoutUtility(this);

        //_labyrinthCreator.GenerateLabyrinthTest();

        _labyrinthCreator.PuzzleSolved += HandlePuzzleSolved;


        _labyrinthCreator.GenerateLabyrinth();
        Vector3[] positionsRoom = _labyrinthCreator.GetStartAndEndPositions();

        _heroSpawnPoint = new Vector3(positionsRoom[0].x,_spawnPoint.position.y,positionsRoom[0].z);

        Vector3 enemyRoom = _labyrinthCreator.GetRoomPosition(2,2);
        _enemySpawnPoint = new Vector3(enemyRoom.x,_enemySpawnPointTransform.position.y,enemyRoom.z);

        
        _waveManager.WaveFinishedAction += HandleWaveFinished;





        Debug.Log("LabWorld");
        
        SpawnPlayer();
/*         SpawnEnemy();
        SpawnEnemy2();
        SpawnEnemy3(); */
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        if(_timeoutToStart != null)
            _timeoutToStart.Dispose();

        _labyrinthCreator.PuzzleSolved -= HandlePuzzleSolved;
    }

    private void SpawnPlayer()
    {
        _timeoutToStart.SetTimeout(() => {
            _defaultCamera.SetActive(false);
           // var hero = Instantiate(_playerHeroPrefab, _spawnPoint.position, _spawnPoint.rotation);

           var hero = Instantiate(_playerHeroPrefab, _heroSpawnPoint, _spawnPoint.rotation);

            Camera camera = hero.GetComponentInChildren<Camera>();

            if (camera)
            {
                _minimap.SetCameraHero(camera);
            }
            else
            {
                Debug.Log("Warning not camera found!!!");
            }


           _heroFSM = hero.GetComponentInChildren<HeroFSM>();
           _heroFSM.onFireAction+= HandleOnFireAction;
           _heroFSM.HeroDiedAction += HandleHeroDied;

           

           PrepareWaves();

        }, .25f);
    }

    private void PrepareWaves()
    {
        Vector3 ePos1 = _labyrinthCreator.GetRoomPosition(0,1);
        Vector3 ePos2 = _labyrinthCreator.GetRoomPosition(9,9);
        Vector3 ePos3 = _labyrinthCreator.GetRoomPosition(9,8);
        Vector3 ePos4 = _labyrinthCreator.GetRoomPosition(9,6);
        Transform heroTransform = _heroFSM.transform.Find("Hero");

        Vector3[] positions = new Vector3[]{ePos1,ePos2,ePos3,ePos4};

        WaveData wd = new WaveData
        {
            typeEnemies = new int[] { 0, 0, 0, 0 },   // example types
            bornPositions = positions,
            patrolPoints = null,             // or provide patrol paths
            heroTransform = heroTransform
        };
        _waveManager.PrepareWave(1,wd);

////
        ePos1 = _labyrinthCreator.GetRoomPosition(7,0);
        /* ePos2 = _labyrinthCreator.GetRoomPosition(9,9);
        ePos3 = _labyrinthCreator.GetRoomPosition(9,8);
        ePos4 = _labyrinthCreator.GetRoomPosition(9,6);
        positions = new Vector3[]{ePos1,ePos2,ePos3,ePos4}; */
        positions = new Vector3[]{ePos1};
        
        WaveData wd2 = new WaveData
        {
            typeEnemies = new int[] { 1 },
            bornPositions = positions,
            patrolPoints = null,             // or provide patrol paths
            heroTransform = null
        };
        _waveManager.PrepareWave(2,wd2);
            
////
        ePos1 = _labyrinthCreator.GetRoomPosition(0,1);
        positions = new Vector3[]{ePos1};
        Vector3 patrolP1 = _labyrinthCreator.GetRoomPosition(0,0);
        Vector3 patrolP2 = _labyrinthCreator.GetRoomPosition(0,5);
        Vector3 patrolP3 = _labyrinthCreator.GetRoomPosition(0,3);
        Vector3[][] patrolPoints = new Vector3[][]
        {
            new Vector3[]{ patrolP1, patrolP2, patrolP3 },         
        };

        WaveData wd3 = new WaveData
        {
            typeEnemies = new int[] { 2 },
            bornPositions = positions,
            patrolPoints = patrolPoints,
            heroTransform = null
        };
        _waveManager.PrepareWave(3,wd3);

////
        ePos1 = _labyrinthCreator.GetRoomPosition(0,1);
        positions = new Vector3[]{ePos1};

        WaveData wd4 = new WaveData
        {
            typeEnemies = new int[] { 0 },   // example types
            bornPositions = positions,
            patrolPoints = null,             // or provide patrol paths
            heroTransform = heroTransform
        };
        _waveManager.PrepareWave(4,wd4);

////
        ePos1 = _labyrinthCreator.GetRoomPosition(7,7);
        positions = new Vector3[]{ePos1};

        WaveData wd5 = new WaveData
        {
            typeEnemies = new int[] { 0 },   // example types
            bornPositions = positions,
            patrolPoints = null,             // or provide patrol paths
            heroTransform = heroTransform
        };
        _waveManager.PrepareWave(5,wd5);

////
        ePos1 = _labyrinthCreator.GetRoomPosition(3,3);
        positions = new Vector3[]{ePos1};

        WaveData wd6 = new WaveData
        {
            typeEnemies = new int[] { 0 },   // example types
            bornPositions = positions,
            patrolPoints = null,             // or provide patrol paths
            heroTransform = heroTransform
        };
        _waveManager.PrepareWave(6,wd6);

        
    }
    

    private void HandleOnFireAction(Vector3 hitPoint, Vector3 hitNormal,RaycastHit hit)
    {
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

    }

    private void HandleHeroDied()
    {
        //Get position from the checkpoint manager
        //Move player to the position
        //Set hero fsm to Run

        //Deactivate the last Checkpoint
        //Deactivate the current wave of enemies




    }

    private void HandlePuzzleSolved(int id)
    {
        //should deactivate the puzzle
        //should activate the corresponding enemy wave
        Debug.Log("_______HandlePuzzleSolved_____ " + id);
        //id = 10;
        //_labyrinthCreator.OpenExitFence(); //to test
        if (id < 6)
        {
            _waveManager.RunWave(id+1);
        }
        else
        {
           Debug.Log("Last puzzle");
        }
    }

    private void HandleWaveFinished(int idWave)
    {

        Debug.Log("_______HandleWaveFinished>>>> " + idWave);
        if(idWave < 6)
        {
            _labyrinthCreator.ActivatePuzzle(idWave);
        }
        else
        {
            Debug.Log("OPEN THE EXIT");
            Debug.Log("Throw the key");
            _labyrinthCreator.OpenExitFence();

        }
    }

}
