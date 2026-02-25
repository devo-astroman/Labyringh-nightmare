using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class LabyrinthObjectsManager : MonoBehaviour
{
    [Header("Prefabs")]

    [Header("Runtime (auto-filled)")]
    [SerializeField] private List<RoomPositions> _allRoomPositions = new List<RoomPositions>();

    [SerializeField] private ObjectsFactory _objectsFactory;


    [SerializeField] private List<AmmoOnBox> _allAmmoOnBox = new List<AmmoOnBox>();
    [SerializeField] private List<Spikes> _allSpikes = new List<Spikes>();

    [SerializeField] private List<string> _roomProcessed = new List<string>();
    

    

    private int _iRoom = 0;
    private int _iLayer = 0;
    private int _iCube = 0;

    /* public void ProcessRoom(GameObject roomGO, int x, int y, int mask)
    {
        
        RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();
        //roomPositionGO.name = $"RoomPos_{x}_{y}";

        roomPositionGO.HideAllCubePositions();
        //roomPositionGO.ShowCubePosition(_iLayer,_iCube);

        _allRoomPositions.Add(roomPositionGO);
        _iRoom = _allRoomPositions.Count; // keeps it consistent

        if(_iLayer == roomPositionGO.LAYER_BELOW)
        {
            Debug.Log("x " + x + " y " + y + " iCube " + _iCube);

            Vector3 position = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,_iCube);

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
            else
            {
                int r =  Random.Range(0,5);
                if(r == 0)
                {
                    //Improve here
                    / * 
                    Vector3 position = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,_iCube);

                    int id = int.Parse(x.ToString() + y.ToString());
                    GameObject spikesGO = _objectsFactory.GetSpikes(position, id);
                    Debug.Log("spikesGO >>> " + spikesGO.GetComponent<Spikes>().GetId());
                    _allSpikes.Add(spikesGO.GetComponent<Spikes>());  * /
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
    } */

    public void ProcessRoom(GameObject roomGO, int x, int y, int mask)
    {
        RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();
        roomPositionGO.HideAllCubePositions();
        int[] tmp = new int[8];

        
        int id = int.Parse(x.ToString() + y.ToString());

        if(_roomProcessed.Contains(x +" "+y)) return;
        
        if(x==0 & y == 1)        
        {            
            // per room:
            int n = 3;
            RandomRoomPositions.FillRandomIndexes(n, tmp);


            //place one box in the middle
            Vector3 middlePosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,tmp[0]);
            GameObject box = _objectsFactory.GetBox2Empties(middlePosition);
            Debug.Log("tmp " + tmp[0] + " " + tmp[1] + " " + tmp[2]);

            //place a spike in the upper right corner
            Vector3 lowerLCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,tmp[1]);
            GameObject spikes = _objectsFactory.GetSpikes(lowerLCornerPosition,id);



            //place other box in the lower corner with an ammo
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,tmp[2]);
            GameObject box2 = _objectsFactory.GetBox2Empties(upperRCornerPosition);
            GameObject ammo = _objectsFactory.GetAmmoOnBox(upperRCornerPosition,id);

        }
        //puzzle rooms
        //else  if(x==1 & y == 1)
        else  if(x==1 & y == 0)
        {
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject puzzle2 = _objectsFactory.GetPuzzle2(upperRCornerPosition,id,mask);
            

        }else  if(x==4 & y == 2)
        {
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject puzzle1 = _objectsFactory.GetPuzzle1(upperRCornerPosition,id,mask);

        }else  if(x==1 & y == 6)
        {
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject puzzle1 = _objectsFactory.GetPuzzle1(upperRCornerPosition,id,mask);

        }else  if(x==3 & y == 5)
        {
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject puzzle1 = _objectsFactory.GetPuzzle1(upperRCornerPosition,id,mask);

        }else  if(x==3 & y == 8)
        {

            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject puzzle1 = _objectsFactory.GetPuzzle1(upperRCornerPosition,id,mask);

        }else  if(x==6 & y == 6)
        {

            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject puzzle1 = _objectsFactory.GetPuzzle1(upperRCornerPosition,id,mask);

        }else  if(x==8 & y == 1)
        {

            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject puzzle1 = _objectsFactory.GetPuzzle1(upperRCornerPosition,id,mask);
            
        }else 
        {// rest of the rooms should be random

            int n = 3;
            RandomRoomPositions.FillRandomIndexes(n, tmp);

            if (Random.Range(0, 10) == 0)
            {
                //place one box in the middle
                Vector3 middlePosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,tmp[0]);
                GameObject box = _objectsFactory.GetBox2Empties(middlePosition);
            }

            if (Random.Range(0, 1) == 0)
            {
                //place a spike in the upper right corner
                Vector3 lowerLCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,tmp[1]);
                GameObject spikes = _objectsFactory.GetSpikes(lowerLCornerPosition,id);
            }

            if (Random.Range(0, 10) == 0)
            {
                //place other box in the lower corner with an ammo
                Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,tmp[2]);
                GameObject box2 = _objectsFactory.GetBox2Empties(upperRCornerPosition);
                GameObject ammo = _objectsFactory.GetAmmoOnBox(upperRCornerPosition,id);
            }

            
        }

        _roomProcessed.Add(x +" "+y);
    }

    // Optional: if you still need an array (read-only snapshot)
    public RoomPositions[] GetAllRoomPositionsArray()
    {
        return _allRoomPositions.ToArray();
    }
}
