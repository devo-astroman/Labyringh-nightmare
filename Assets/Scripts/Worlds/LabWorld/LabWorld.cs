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

        Vector3 enemyRoom = _labyrinthCreator.GetRoomPosition(2,2);
        _enemySpawnPoint = new Vector3(enemyRoom.x,_enemySpawnPointTransform.position.y,enemyRoom.z);

        /* 
        Add _enemyManager.EnemyDeadAction += OnEnemyDeadAction(int id)
         */

        Debug.Log("LabWorld");
        
        SpawnPlayer();
        SpawnEnemy();
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
            _defaultCamera.SetActive(false);
           // var hero = Instantiate(_playerHeroPrefab, _spawnPoint.position, _spawnPoint.rotation);

           var hero = Instantiate(_playerHeroPrefab, _heroSpawnPoint, _spawnPoint.rotation);

            _heroFSM = hero.GetComponentInChildren<HeroFSM>();
            _heroFSM.onFireAction+= HandleOnFireAction;

        }, .25f);
    }

    private void SpawnEnemy()
    {
        _timeoutToSpawnEnemies.SetTimeout(() => {
            Vector3 enemySpawnPosition = _labyrinthCreator.GetRoomPosition(0,1);
            Vector3 patrolPoint1 = _labyrinthCreator.GetRoomPosition(0,0);
            Vector3 patrolPoint2 = _labyrinthCreator.GetRoomPosition(0,5);
            Vector3 patrolPoint3 = _labyrinthCreator.GetRoomPosition(0,3);

            _enemyManager.WakeUpEnemyB1(enemySpawnPosition,new Vector3[]{patrolPoint1,patrolPoint2,patrolPoint3});


            Vector3 enemy2SpawnPosition = _labyrinthCreator.GetRoomPosition(9,9);
            Vector3 patrol2Point1 = _labyrinthCreator.GetRoomPosition(8,8);
            Vector3 patrol2Point2 = _labyrinthCreator.GetRoomPosition(4,4);
            Vector3 patrol2Point3 = _labyrinthCreator.GetRoomPosition(6,6);
            _enemyManager.WakeUpEnemyB1(enemy2SpawnPosition,new Vector3[]{patrol2Point1,patrol2Point2,patrol2Point3});

            Vector3 enemy3SpawnPosition = _labyrinthCreator.GetRoomPosition(9,8);
            Vector3 patrol3Point1 = _labyrinthCreator.GetRoomPosition(9,0);
            Vector3 patrol3Point2 = _labyrinthCreator.GetRoomPosition(5,0);
            Vector3 patrol3Point3 = _labyrinthCreator.GetRoomPosition(7,9);
            _enemyManager.WakeUpEnemyB1(enemy3SpawnPosition,new Vector3[]{patrol3Point1,patrol3Point2,patrol3Point3});


            Vector3 enemy4SpawnPosition = _labyrinthCreator.GetRoomPosition(9,6);
            Vector3 patrol4Point1 = _labyrinthCreator.GetRoomPosition(9,0);
            Vector3 patrol4Point2 = _labyrinthCreator.GetRoomPosition(5,0);
            Vector3 patrol4Point3 = _labyrinthCreator.GetRoomPosition(7,9);
            _enemyManager.WakeUpEnemyB1(enemy4SpawnPosition,new Vector3[]{patrol4Point1,patrol4Point2,patrol4Point3});


            _labyrinthCreator.ShowDebug(new Vector3[]{patrolPoint1,patrolPoint2,patrolPoint3});

        }, 2f);
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
            enemyB1FSM.ReceiveDamage(0);

        }
        else
        {
            _vFXsManager.ShowHitWallVFXs(hitPoint,hitNormal);
        }

    }

}
