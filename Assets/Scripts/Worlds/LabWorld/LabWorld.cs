using UnityEngine;


public class LabWorld : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _playerHeroPrefab;
    [SerializeField] private GameObject _defaultCamera;

    [SerializeField] private HeroFSM _heroFSM;
    [SerializeField] private VFXsManager _vFXsManager;
    

    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private LabyrinthCreator _labyrinthCreator;

    private SetTimeoutUtility _timeoutToStart;
    private SetTimeoutUtility _timeoutToSpawnEnemies;
    private Vector3 _heroSpawnPoint;

    [SerializeField] private Transform _enemySpawnPointTransform;
    private Vector3 _enemySpawnPoint;
    



    void Start()
    {   
        _timeoutToStart = new SetTimeoutUtility(this);
        _timeoutToSpawnEnemies = new SetTimeoutUtility(this);

        //_labyrinthCreator.GenerateLabyrinthTest();

        _labyrinthCreator.GenerateLabyrinth();
        Vector3[] positionsRoom = _labyrinthCreator.GetStartAndEndPositions();

        _heroSpawnPoint = new Vector3(positionsRoom[0].x,_spawnPoint.position.y,positionsRoom[0].z);

        Vector3 enemyRoom = _labyrinthCreator.GetRoomPosition(5,5);
        _enemySpawnPoint = new Vector3(enemyRoom.x,_enemySpawnPointTransform.position.y,enemyRoom.z);

        /* 
        Add _enemyManager.EnemyDeadAction += OnEnemyDeadAction(int id)
         */

        Debug.Log("LabWorld");
        
        SpawnPlayer();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        if(_timeoutToStart != null)
            _timeoutToStart.Dispose();
    }

    private void SpawnPlayer()
    {
        _timeoutToStart.SetTimeout(() => {
            Debug.Log("Should spawn the player");
            _defaultCamera.SetActive(false);
           // var hero = Instantiate(_playerHeroPrefab, _spawnPoint.position, _spawnPoint.rotation);

           var hero = Instantiate(_playerHeroPrefab, _heroSpawnPoint, _spawnPoint.rotation);

            _heroFSM = hero.GetComponentInChildren<HeroFSM>();
            _heroFSM.onFireAction+= HandleOnFireAction;

        }, .25f);


        _timeoutToSpawnEnemies.SetTimeout(() => {
            Debug.Log("Should spawn the enemy");
            _enemyManager.WakeUpEnemyT(_enemySpawnPoint,  _heroFSM.transform);

        }, 2f);
    }
    

    private void HandleOnFireAction(Vector3 hitPoint, Vector3 hitNormal,RaycastHit hit)
    {

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyCollider"))
        {
            //Here should detect who is the enemy using the id of the EnemyT
            _enemyManager.ProcessDamage(0);
        }
        else
        {
            _vFXsManager.ShowHitWallVFXs(hitPoint,hitNormal);
        }

    }

}
