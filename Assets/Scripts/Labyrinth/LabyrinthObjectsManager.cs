using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LabyrinthObjectsManager : MonoBehaviour
{
    [Header("Prefabs")]    

    [Header("Runtime (auto-filled)")]
    [SerializeField] private List<RoomPositions> _allRoomPositions = new List<RoomPositions>();

    private int _iRoom = 0;
    private int _iLayer = 0;
    private int _iCube = 0;

    public void ProcessRoom(GameObject roomGO, int x, int y, int mask)
    {
        
        RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();
        //roomPositionGO.name = $"RoomPos_{x}_{y}";

        roomPositionGO.HideAllCubePositions();
        roomPositionGO.ShowCubePosition(_iLayer,_iCube);

        _allRoomPositions.Add(roomPositionGO);
        _iRoom = _allRoomPositions.Count; // keeps it consistent

        _iCube++;
        if(_iCube >= 9)
        {
            _iCube=0;
            _iLayer++;
            if (_iLayer >= 3)
            {
                _iLayer=0;
            }
        }
    }

    // Optional: if you still need an array (read-only snapshot)
    public RoomPositions[] GetAllRoomPositionsArray()
    {
        return _allRoomPositions.ToArray();
    }
}
