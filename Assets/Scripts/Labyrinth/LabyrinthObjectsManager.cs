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
    [SerializeField] private CheckpointManager _checkpointManager;


    
    
    public Action<int> PuzzleSolvedAction;

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
        //------puzzle rooms //P1
        else  if(x==1 & y == 2)        
        {
            int idPuzzle = 0;
            Vector3 centerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p1 = _objectsFactory.GetPuzzle1(centerPosition,id,mask);
            _puzzleManager.RegisterPuzzle(idPuzzle,p1);

            ActivatableBehaviour activatableBehaviour = p1.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Activate();


            Vector3 centerLeftPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,1);
            GameObject checkpoint1 = _objectsFactory.GetCheckpoint(centerLeftPosition,idPuzzle);
            _checkpointManager.RegisterCheckpoint(idPuzzle,idPuzzle,checkpoint1.transform.position);

            //initializing
            _checkpointManager.SetCurrentCheckpointIdFromPuzzleId(idPuzzle);

        }else  if(x==0 & y == 9) //P2
        {
            int idPuzzle = 1;
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p2 = _objectsFactory.GetPuzzle2(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p2.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();

            _puzzleManager.RegisterPuzzle(idPuzzle,p2);

            Vector3 centerLeftPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,3);
            GameObject checkpoint2 = _objectsFactory.GetCheckpoint(centerLeftPosition,idPuzzle);            
            _checkpointManager.RegisterCheckpoint(idPuzzle,idPuzzle,checkpoint2.transform.position);

        }else  if(x==3 & y == 6)  //P3
        {
            int idPuzzle = 2;
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p3 = _objectsFactory.GetPuzzle3(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p3.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();
            _puzzleManager.RegisterPuzzle(idPuzzle,p3);

            Vector3 centerLeftPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,1);
            GameObject checkpoint3 = _objectsFactory.GetCheckpoint(centerLeftPosition,idPuzzle);
            _checkpointManager.RegisterCheckpoint(idPuzzle,idPuzzle,checkpoint3.transform.position);

        }else  if(x==7 & y == 8) //P4
        {
            int idPuzzle = 3;
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p4 = _objectsFactory.GetPuzzle4(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p4.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();
            _puzzleManager.RegisterPuzzle(idPuzzle,p4);


            Vector3 centerLeftPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,0);
            GameObject checkpoint4 = _objectsFactory.GetCheckpoint(centerLeftPosition,idPuzzle);
            _checkpointManager.RegisterCheckpoint(idPuzzle,idPuzzle,checkpoint4.transform.position);

        }else  if(x==8 & y == 0) //P5
        {
            int idPuzzle = 4;
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p5 = _objectsFactory.GetPuzzle5(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p5.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();
            _puzzleManager.RegisterPuzzle(idPuzzle,p5);

            Vector3 centerLeftPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,6);
            GameObject checkpoint5 = _objectsFactory.GetCheckpoint(centerLeftPosition,idPuzzle);
            _checkpointManager.RegisterCheckpoint(idPuzzle,idPuzzle,checkpoint5.transform.position);

        }else  if(x==5 & y == 3) //P6
        {
            int idPuzzle = 5;
            Vector3 upperRCornerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
            GameObject p6 = _objectsFactory.GetPuzzle6(upperRCornerPosition,id,mask);
            ActivatableBehaviour activatableBehaviour = p6.GetComponent<ActivatableBehaviour>();
            activatableBehaviour.Deactivate();
            _puzzleManager.RegisterPuzzle(idPuzzle,p6);

            Vector3 centerLeftPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,5);
            GameObject checkpoint6 = _objectsFactory.GetCheckpoint(centerLeftPosition,idPuzzle);
            _checkpointManager.RegisterCheckpoint(idPuzzle,idPuzzle,checkpoint6.transform.position);

        }else  if(x==9 & y == 4)    
        {
            
        }else 
        {// rest of the rooms should be random

            int n = 3;
            RandomRoomPositions.FillRandomIndexes(n, tmp);

            if (UnityEngine.Random.Range(0, 10) == 0)
            {
                //place one box
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

    public void SetCurrentCheckpointFromSolvedPuzzleId(int puzzleId)
    {
        _checkpointManager.SetCurrentCheckpointIdFromPuzzleId(puzzleId);
    }  

    public Vector3 GetCurrentCheckpointPosition()
    {
        return _checkpointManager.GetCurrentCheckpointPosition();
    }

    public void ActivateLastPuzzle()
    {
        _puzzleManager.ActivateLastPuzzle();
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
