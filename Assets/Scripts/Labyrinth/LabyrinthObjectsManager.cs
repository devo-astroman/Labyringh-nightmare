using System.Collections.Generic;
using UnityEngine;
using System;

public class LabyrinthObjectsManager : MonoBehaviour
{
    [Header("Prefabs")]

    [Header("Runtime (auto-filled)")]
    [SerializeField] private List<RoomPositions> _allRoomPositions = new List<RoomPositions>();

    [SerializeField] private ObjectsFactory _objectsFactory;


    [SerializeField] private List<AmmoOnBox> _allAmmoOnBox = new List<AmmoOnBox>();
    [SerializeField] private List<Spikes> _allSpikes = new List<Spikes>();

    [SerializeField] private List<string> _roomProcessed = new List<string>();

    [SerializeField] private PuzzlesManager _puzzleManager;
    public Action<int> PuzzleSolvedAction;

    private int _iRoom = 0;
    private int _iLayer = 0;
    private int _iCube = 0;

    void Start()
    {
        _puzzleManager.PuzzleSolvedAction += HandlePuzzleSolved;
    }

    public void ActivatePuzzle(int id)
    {
        _puzzleManager.ActivatePuzzle(id);
    }

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
            GameObject p1 = _objectsFactory.GetPuzzle1(upperRCornerPosition,id,mask);
            _puzzleManager.RegisterPuzzle(0,p1);

            ActivatableBehaviour activatableBehaviour = p1.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Activate();

            //GameObject puzzle2 = _objectsFactory.GetPuzzle2(upperRCornerPosition,id,mask);
            //GameObject puzzleCompass = _objectsFactory.GetPuzzleCompass(upperRCornerPosition,id,mask);
            //GameObject puzzleCompass = _objectsFactory.GetPuzzleRowBin(upperRCornerPosition,id,mask);
            

        }else  if(x==4 & y == 2)
        {
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p2 = _objectsFactory.GetPuzzle2(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p2.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();

            _puzzleManager.RegisterPuzzle(1,p2);

        }else  if(x==1 & y == 6)
        {
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p3 = _objectsFactory.GetPuzzle3(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p3.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();
            _puzzleManager.RegisterPuzzle(2,p3);

        }else  if(x==3 & y == 5)
        {
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p4 = _objectsFactory.GetPuzzle4(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p4.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();
            _puzzleManager.RegisterPuzzle(3,p4);

        }else  if(x==3 & y == 8)
        {

            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p5 = _objectsFactory.GetPuzzle5(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p5.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();
            _puzzleManager.RegisterPuzzle(4,p5);

        }else  if(x==6 & y == 6)
        {

            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p6 = _objectsFactory.GetPuzzle6(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p6.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();
            _puzzleManager.RegisterPuzzle(5,p6);

        }else  if(x==8 & y == 1)
        {

            /* Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p6 = _objectsFactory.GetPuzzle6(upperRCornerPosition,id,mask);
            _puzzleManager.RegisterPuzzle(6,p3); */
            
        }else 
        {// rest of the rooms should be random

            int n = 3;
            RandomRoomPositions.FillRandomIndexes(n, tmp);

            if (UnityEngine.Random.Range(0, 10) == 0)
            {
                //place one box in the middle
                Vector3 middlePosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,tmp[0]);
                GameObject box = _objectsFactory.GetBox2Empties(middlePosition);
            }

            if (UnityEngine.Random.Range(0, 1) == 0)
            {
                //place a spike in the upper right corner
                Vector3 lowerLCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,tmp[1]);
                GameObject spikes = _objectsFactory.GetSpikes(lowerLCornerPosition,id);
            }

            if (UnityEngine.Random.Range(0, 10) == 0)
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

    void OnDestroy()
    {
        _puzzleManager.PuzzleSolvedAction -= HandlePuzzleSolved;
    }

    private void HandlePuzzleSolved(int id)
    {
        PuzzleSolvedAction?.Invoke(id);
    }
}
