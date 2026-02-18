using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LabyrinthObjectsManager : MonoBehaviour
{
    [Header("Prefabs")]

    [Header("Runtime (auto-filled)")]
    [SerializeField] private List<RoomPositions> _allRoomPositions = new List<RoomPositions>();

    [SerializeField] private ObjectsFactory _objectsFactory;


    [SerializeField] private List<AmmoOnBox> _allAmmoOnBox = new List<AmmoOnBox>();

    

    private int _iRoom = 0;
    private int _iLayer = 0;
    private int _iCube = 0;

    public void ProcessRoom(GameObject roomGO, int x, int y, int mask)
    {
        
        RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();
        //roomPositionGO.name = $"RoomPos_{x}_{y}";

        roomPositionGO.HideAllCubePositions();
        //roomPositionGO.ShowCubePosition(_iLayer,_iCube);

        _allRoomPositions.Add(roomPositionGO);
        _iRoom = _allRoomPositions.Count; // keeps it consistent

        if(_iLayer == roomPositionGO.LAYER_BELOW)
        {
            Vector3 position = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,_iCube);

            /* GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cube.transform.position = position; */

            GameObject box = _objectsFactory.GetBox2Empties(position);
            //should save that box in a list or something similar


            
            if(x == 9 && y == 0)
            {
                GameObject ammoOnBoxGO = _objectsFactory.GetAmmoOnBox(position, 90);
                if (ammoOnBoxGO)
                {
                    _allAmmoOnBox.Add(ammoOnBoxGO.GetComponent<AmmoOnBox>());    
                }
            }else if (x == 3 && y == 2)
            {
                GameObject ammoOnBoxGO = _objectsFactory.GetAmmoOnBox(position, 32);
                if (ammoOnBoxGO)
                {
                    _allAmmoOnBox.Add(ammoOnBoxGO.GetComponent<AmmoOnBox>());    
                }

            }else if (x == 0 && y == 9)
            {
                GameObject ammoOnBoxGO = _objectsFactory.GetAmmoOnBox(position, 9);
                if (ammoOnBoxGO)
                {
                    _allAmmoOnBox.Add(ammoOnBoxGO.GetComponent<AmmoOnBox>());    
                }

            }else if (x == 6 && y == 6)
            {
                GameObject ammoOnBoxGO = _objectsFactory.GetAmmoOnBox(position, 66);
                if (ammoOnBoxGO)
                {
                    _allAmmoOnBox.Add(ammoOnBoxGO.GetComponent<AmmoOnBox>());    
                }
            }
            //should save that box in a list or something similar


        }


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
