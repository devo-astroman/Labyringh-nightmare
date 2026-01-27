using UnityEngine;


public class LabWorld : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _playerHeroPrefab;
    [SerializeField] private GameObject _defaultCamera;

    private SetTimeoutUtility _timeoutToStart;

    void Start()
    {
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
        }, 1f);
    }

}
