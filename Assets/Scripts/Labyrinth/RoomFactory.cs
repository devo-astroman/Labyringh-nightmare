using UnityEngine;

public class RoomFactory : MonoBehaviour
{
    [Header("Rooms Prefabs")]
    [SerializeField] private GameObject _room1x1;
    [SerializeField] private GameObject _room2x2;
    [SerializeField] private GameObject _room4x4;

    public GameObject GetRoom1x1(Vector3 position)
    {
        return Instantiate(_room1x1, position, Quaternion.identity);
    }

    public GameObject GetRoom1x1(Vector3 position, RoomSides side, int[] idsOpenEntrance)
    {
        GameObject roomGO =  Instantiate(_room1x1, position, Quaternion.identity);
        Room room = roomGO.GetComponent<Room>();
        room.OpenEntrances(side,idsOpenEntrance);

        return roomGO;
    }

    public GameObject GetRoom2x2(Vector3 position)
    {
        return Instantiate(_room2x2, position, Quaternion.identity);
    }

    public GameObject GetRoom2x2(Vector3 position, RoomSides side, int[] idsOpenEntrance)
    {
        GameObject roomGO =  Instantiate(_room2x2, position, Quaternion.identity);
        Room room = roomGO.GetComponent<Room>();
        room.OpenEntrances(side,idsOpenEntrance);

        return roomGO;
    }

    public GameObject GetRoom4x4(Vector3 position)
    {
        return Instantiate(_room4x4, position, Quaternion.identity);
    }

    public GameObject GetRoom4x4(Vector3 position, RoomSides side, int[] idsOpenEntrance)
    {
        GameObject roomGO =  Instantiate(_room4x4, position, Quaternion.identity);
        Room room = roomGO.GetComponent<Room>();
        room.OpenEntrances(side,idsOpenEntrance);

        return roomGO;
    }


}
