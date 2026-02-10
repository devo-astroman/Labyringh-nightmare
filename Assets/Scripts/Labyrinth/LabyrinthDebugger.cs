using UnityEngine;

public class LabyrinthDebugger : MonoBehaviour
{
    [SerializeField] private GameObject _roomMarkPref;

    [SerializeField] private Transform _highPosition;

    public void ShowRoomMarksAt(Vector3[] positions)
    {

        if (_roomMarkPref == null)
        {
            Debug.LogError("LabyrinthDebugger: Room mark prefab is not assigned.");
            return;
        }

        if (positions == null || positions.Length == 0)
            return;

        for (int i = 0; i < positions.Length; i++)
        {
            Instantiate(
                _roomMarkPref,
                new Vector3(positions[i].x,_highPosition.position.y,positions[i].z),
                Quaternion.identity,
                transform // optional: keep markers organized under this object
            );
        }
    }
}
