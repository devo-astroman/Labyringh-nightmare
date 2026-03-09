using System;
using UnityEngine;

public class PuzzleNotifier : MonoBehaviour
{
    #region Fields
    #endregion

    #region public properties
    public Action<int> PuzzleSolvedAction;
    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks
    #endregion

    #region Public methods 
    public void NotifyPuzzleSolved(int id)
    {
        PuzzleSolvedAction?.Invoke(id);
    }
    #endregion

    #region Private methods
    #endregion
}
