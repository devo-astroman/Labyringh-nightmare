using UnityEngine;

public enum RoomSides
{
    NORTH_SIDE,
    EAST_SIDE,
    SOUTH_SIDE,
    WEST_SIDE,
}

public class Room : MonoBehaviour
{
    [Header("Entrances - At clock direction")]
    [SerializeField] private GameObject[] _nEntrances;
    [SerializeField] private GameObject[] _eEntrances;
    [SerializeField] private GameObject[] _sEntrances;
    [SerializeField] private GameObject[] _wEntrances;


    public void OpenEntrances(RoomSides side, int[] idsToOpen)
    {
        GameObject[] entrances;
        switch (side)
        {
            case RoomSides.NORTH_SIDE:
                entrances = _nEntrances;
            break;
            case RoomSides.EAST_SIDE:
                entrances = _eEntrances;
            break;
            case RoomSides.SOUTH_SIDE:
                entrances = _sEntrances;
            break;
            default:
                entrances = _wEntrances;
            break;
        }

        for(int id = 0; id < idsToOpen.Length; id++)
        {
            entrances[id].SetActive(false);
        }
    }

    public void CloseAllEntrances()
    {
        CloseAllSideEntrances(_nEntrances);
        CloseAllSideEntrances(_eEntrances);
        CloseAllSideEntrances(_sEntrances);
        CloseAllSideEntrances(_wEntrances);
    } 

    private void CloseAllSideEntrances(GameObject[] entrances)
    {
        for(int i = 0; i<entrances.Length; i++)
        {
            entrances[i].SetActive(true);
        }
    }


}
