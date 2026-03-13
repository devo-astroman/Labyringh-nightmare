using UnityEngine;
public class World : MonoBehaviour
{
    #region Fields
    [SerializeField] private LabyrinthCreator _labyrinthCreator; 
    [SerializeField] private Enviroment _enviroment;    
    #endregion

    #region Private Fields
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks
    void Start()
    {

        _labyrinthCreator.GenerateLabyrinth((roomGO, x, y, mask) =>
        {
            Debug.Log($"Room {roomGO.name} at {x},{y} mask:{mask}");            
            
            RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();

            roomPositionGO.HideAllCubePositions();

            int[] randomIndexes = Get3RandomIndexes();
            
            
            if (UnityEngine.Random.Range(0, 10) >= 4)
            {
                int boxIndex = randomIndexes[0];
                Vector3 boxPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,boxIndex);

                _enviroment.PlaceBox(boxPosition);
                if (UnityEngine.Random.Range(0, 10) >= 4)
                {
                    _enviroment.PlaceAmmo(boxPosition,int.Parse(x+""+y));
                    //PlaceAmmo(roomGO,boxIndex, int.Parse(x+""+y));
                }
            }

                        
            if (UnityEngine.Random.Range(0, 1) == 0)
            {
                int spikeIndex = randomIndexes[1];                
                Vector3 spikePosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,spikeIndex);
                _enviroment.PlaceSpike(spikePosition, int.Parse(x+""+y));
                //PlaceSpike(roomGO,spikeIndex, int.Parse(x+""+y));
            }

        });
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void PlaceBox(GameObject roomGO, int positionInLayer)
    {
        RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();
        Vector3 position = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,positionInLayer);

        _enviroment.PlaceBox(position);
    }

    private void PlaceAmmo(GameObject roomGO, int positionInLayer, int id)
    {
        RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();
        Vector3 position = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,positionInLayer);

        _enviroment.PlaceAmmo(position,id);
    }

    private void PlaceSpike(GameObject roomGO, int positionInLayer, int id)
    {
        RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();
        Vector3 position = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,positionInLayer);

        _enviroment.PlaceSpike(position,id);
    }

    private int[] Get3RandomIndexes()
    {
        int n = 3;
        int[] tmp = new int[8];
        RandomRoomPositions.FillRandomIndexes(n, tmp);

        return tmp;
    }

    #endregion
}

