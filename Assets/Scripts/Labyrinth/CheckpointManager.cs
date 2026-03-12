using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    #region Fields
    #endregion

    #region Private properties
    private int _idCheckpoint = 0;
    #endregion
    public void AddCurrentCheckpoint(int idCheckpoint)
    {
            _idCheckpoint = idCheckpoint;
    }
    #region Unity Callbacks
	
	#endregion



}