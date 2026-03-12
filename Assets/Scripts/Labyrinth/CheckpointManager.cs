using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct CheckpointData
{
    public int PuzzleId;
    public int CheckpointId;
    public Vector3 CheckpointPosition;
}

public class CheckpointManager : MonoBehaviour
{
    #region Fields
    #endregion

    #region Private properties
    private int _currentCheckpointId = 0;
    private CheckpointData _currentCheckpointData;
    private Dictionary<int, CheckpointData> _checkpoints = new Dictionary<int, CheckpointData>();
    #endregion

    public void RegisterCheckpoint(int puzzleId, int checkpointId, Vector3 checkpointPosition)
    {
        _checkpoints[puzzleId] = new CheckpointData
        {
            PuzzleId = puzzleId,
            CheckpointId = checkpointId,
            CheckpointPosition = checkpointPosition
        };
    }

    public bool TryGetCheckpoint(int puzzleId, out CheckpointData checkpointData)
    {
        return _checkpoints.TryGetValue(puzzleId, out checkpointData);
    }

    public void SetCurrentCheckpointIdFromPuzzleId(int puzzleId)
    {
        if (TryGetCheckpoint(puzzleId, out CheckpointData checkpointData))
        {
            _currentCheckpointData = checkpointData;
        }
    }

    public void AddCurrentCheckpoint(int idCheckpoint)
    {
        _currentCheckpointId = idCheckpoint;
    }

    public Vector3 GetCurrentCheckpointPosition()
    {
        return _currentCheckpointData.CheckpointPosition;
    }

    #region Unity Callbacks
    #endregion
}