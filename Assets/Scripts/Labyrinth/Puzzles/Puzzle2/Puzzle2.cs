using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntTriple
{
    public int A;
    public int B;
    public int C;
}

public class Puzzle2 : MonoBehaviour
{

    #region Fields
    [SerializeField] private Col3Rotator _col3Rotator;
    
    [SerializeField] private ColButton _colButtonRotateUp;
    [SerializeField] private ColButton _colButtonRotateMiddle;
    [SerializeField] private ColButton _colButtonRotateBottom;

    [SerializeField] private ColButton _colButtonCheck;

    [SerializeField] private IntTriple _solution;
    #endregion

    #region public properties
    void Start()
    {
        Random.InitState(System.Environment.TickCount);
        int nRotationsUp = Random.Range(1,4);
        int nRotationsMiddle = Random.Range(1,4);
        int nRotationsBottom = Random.Range(1,4);

        if(nRotationsUp == nRotationsMiddle)
        {
            nRotationsUp--;
            if(nRotationsUp == 0) nRotationsUp += 2;
        }

        _col3Rotator.SetInitialRotations(nRotationsUp,nRotationsMiddle,nRotationsBottom);

    }
    #endregion

    #region Private properties    

    private int _col1Solution = 3;
    private int _col2Solution = 0;
    private int _col3Solution = 1;
    #endregion
    #region Unity Callbacks    
    #endregion

    #region Public methods
    public void ButtonCheckActionated()
    {
        int[] col3RotatorValues = _col3Rotator.GetIntValues();

        Debug.Log("values " + col3RotatorValues[0] + " " + col3RotatorValues[1] + " " + col3RotatorValues[2]);

        //if success ...

        if(col3RotatorValues[0] == col3RotatorValues[1] && col3RotatorValues[1] == col3RotatorValues[2])
        {
            Debug.Log("Success!");
            _colButtonCheck.PressButton();
            _colButtonCheck.Deactivate();
            _colButtonRotateUp.Deactivate();
            _colButtonRotateMiddle.Deactivate();
            _colButtonRotateBottom.Deactivate();
        }
        else
        {
            Debug.Log("Wrong!");
            _colButtonCheck.PressAndPull();
        }

    }
    
    
	#endregion
    
    #region Private methods
    private void DeactivatePuzzle()
    {
    
    }

    #endregion
}
