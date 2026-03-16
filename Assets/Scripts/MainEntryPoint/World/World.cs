using UnityEngine;
using System.Collections.Generic;
using System;

public struct PuzzleData
{
    public int id;
    public GameObject Puzzle;
    public GameObject Checkpoint;
}

public class World : MonoBehaviour
{
    #region Fields
    [SerializeField] private LabyrinthCreator _labyrinthCreator; 
    [SerializeField] private Enviroment _enviroment;    
    #endregion

    #region Private Fields
    private Vector3 _startPoint;
    private List<PuzzleData> _puzzleDataList = new List<PuzzleData>();

    private Vector3[] _wave1;
    #endregion

    #region Properties
    #endregion

    #region Events
    public Action WorldCreationFinishedAction;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        _labyrinthCreator.LabyrinthCreationFinishedAction += HandleLabyrinthCreationFinished;
    }

    void OnDestroy()
    {
        _labyrinthCreator.LabyrinthCreationFinishedAction -= HandleLabyrinthCreationFinished;
    }
    #endregion

    #region Public Methods
    public Vector3 GetStartPoint()
    {
        return _startPoint;
    }

    public List<PuzzleData> GetPuzzleDataList()
    {
        return _puzzleDataList;
    }

    public Vector3 GetPosition(int x, int y)
    {        
        return _labyrinthCreator.GetRoomPosition(x,y);
    }

    public void GenerateWorld()
    {
        _labyrinthCreator.GenerateLabyrinth((roomGO, x, y, mask) =>
        {
            
            //Debug.Log($"Room {roomGO.name} at {x},{y} mask:{mask}");

            RoomPositions roomPositionGO = roomGO.GetComponent<Room>().GetRoomPositions();
            roomPositionGO.HideAllCubePositions();

            //if(x==0 && y== 0) //start room so should be clean
            if(x==1 && y== 1) //start room so should be clean to test
            {
                _startPoint = roomPositionGO.GetPosition(roomPositionGO.LAYER_UP,4);                
            }
            else if (x==9 && y== 4) //exit room so should be clean
            {
                
            }
            //puzzles and checkpoint room
            else if(x==1 & y == 2) //p1 and checkpoint
            {
                Vector3 centerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
                GameObject p = _enviroment.PlacePuzzle(centerPosition,1);

                Vector3 checkpointPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,1);
                GameObject c = _enviroment.PlaceCheckpoint(checkpointPosition);

                _puzzleDataList.Add(new PuzzleData()
                {
                    id = 1,
                    Puzzle = p,
                    Checkpoint = c
                });

            }
            else if(x==0 & y == 9) //p2 and checkpoint
            {
                Vector3 centerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
                GameObject p = _enviroment.PlacePuzzle(centerPosition,2);

                Vector3 checkpointPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,3);
                GameObject c = _enviroment.PlaceCheckpoint(checkpointPosition);
                
                _puzzleDataList.Add(new PuzzleData()
                {
                    id = 2,
                    Puzzle = p,
                    Checkpoint = c
                });

            }
            else if(x==3 & y == 6) //p3 and checkpoint
            {
                Vector3 centerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
                GameObject p = _enviroment.PlacePuzzle(centerPosition,3);

                Vector3 checkpointPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,1);
                GameObject c = _enviroment.PlaceCheckpoint(checkpointPosition);

                _puzzleDataList.Add(new PuzzleData()
                {
                    id = 3,
                    Puzzle = p,
                    Checkpoint = c
                });

            }
            else if(x==7 & y == 8) //p4 and checkpoint
            {
                Vector3 centerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
                GameObject p = _enviroment.PlacePuzzle(centerPosition,4);

                Vector3 checkpointPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,0);
                GameObject c = _enviroment.PlaceCheckpoint(checkpointPosition);

                _puzzleDataList.Add(new PuzzleData()
                {
                    id = 4,
                    Puzzle = p,
                    Checkpoint = c
                });

            }
            else if(x==8 & y == 0) //p5 and checkpoint
            {
                Vector3 centerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
                GameObject p = _enviroment.PlacePuzzle(centerPosition,5);

                Vector3 checkpointPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,6);
                GameObject c = _enviroment.PlaceCheckpoint(checkpointPosition);

                _puzzleDataList.Add(new PuzzleData()
                {
                    id = 5,
                    Puzzle = p,
                    Checkpoint = c
                });

            }
            else if(x==5 & y == 3) //p6 and checkpoint
            {
                Vector3 centerPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,4);
                GameObject p = _enviroment.PlacePuzzle(centerPosition,6);

                Vector3 checkpointPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,5);
                GameObject c = _enviroment.PlaceCheckpoint(checkpointPosition);

                _puzzleDataList.Add(new PuzzleData()
                {
                    id = 6,
                    Puzzle = p,
                    Checkpoint = c
                });

            }
            else
            {
                int[] randomIndexes = Get3RandomIndexes();
                
                if (UnityEngine.Random.Range(0, 10) >= 4)
                {
                    int boxIndex = randomIndexes[0];
                    Vector3 boxPosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,boxIndex);

                    _enviroment.PlaceBox(boxPosition);
                    if (UnityEngine.Random.Range(0, 10) >= 4)
                    {
                        _enviroment.PlaceAmmo(boxPosition,int.Parse(x+""+y));
                    }
                }

                            
                if (UnityEngine.Random.Range(0, 1) == 0)
                {
                    int spikeIndex = randomIndexes[1];                
                    Vector3 spikePosition = roomPositionGO.GetPosition(roomPositionGO.LAYER_BELOW,spikeIndex);
                    _enviroment.PlaceSpike(spikePosition, int.Parse(x+""+y));
                }
                
            }

            
            

        });
    }
    #endregion

    #region Private Methods

    
    private int[] Get3RandomIndexes()
    {
        int n = 3;
        int[] tmp = new int[8];
        RandomRoomPositions.FillRandomIndexes(n, tmp);

        return tmp;
    }

    private void HandleLabyrinthCreationFinished()
    {
        WorldCreationFinishedAction?.Invoke();
    }

    #endregion
}

