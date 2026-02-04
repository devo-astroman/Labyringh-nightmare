using UnityEngine;


public class LabWorld : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _playerHeroPrefab;
    [SerializeField] private GameObject _defaultCamera;

    [SerializeField] private HeroFSM _heroFSM;
    [SerializeField] private FireContact _fireContact;

    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private LabyrinthCreator _labyrinthCreator;

    private SetTimeoutUtility _timeoutToStart;

    



    void Start()
    {   
        _labyrinthCreator.GenerateLabyrinth();

        Debug.Log("LabWorld");
        _timeoutToStart = new SetTimeoutUtility(this);
        SpawnPlayer();
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        _timeoutToStart.Dispose();
    }

    private void SpawnPlayer()
    {
        _timeoutToStart.SetTimeout(() => {
            Debug.Log("Should spawn the player");
            _defaultCamera.SetActive(false);
            var hero = Instantiate(_playerHeroPrefab, _spawnPoint.position, _spawnPoint.rotation);

            _heroFSM = hero.GetComponentInChildren<HeroFSM>();
            _heroFSM.onFireAction+= HandleOnFireAction;

            //_enemyManager.WakeUpEnemyT(_heroFSM.transform);

        }, .25f);
    }

    private void HandleOnFireAction(Vector3 hitPoint, Vector3 hitNormal,RaycastHit hit)
    {
        _fireContact.OnHit(hitPoint,hitNormal);

        // Layer check (FAST)
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _enemyManager.ProcessDamage(0);
        }

    }

}
