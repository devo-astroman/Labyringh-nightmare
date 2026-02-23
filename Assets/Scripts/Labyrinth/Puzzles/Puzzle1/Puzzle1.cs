using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1 : MonoBehaviour
{

    #region Fields
    [SerializeField] private ColRotator _col1;
    [SerializeField] private ColRotator _col2;
    [SerializeField] private ColRotator _col3;
    [SerializeField] private ColButton _colButton;
    #endregion

    #region public properties    
    #endregion

    #region Private properties    

    private int _col1Solution = 3;
    private int _col2Solution = 0;
    private int _col3Solution = 1;
    #endregion
    #region Unity Callbacks    
    #endregion

    #region Public methods
    public void ColButtonPressed()
    {
        int col1Value = _col1.GetComponent<IntValue>().GetValue();
        int col2Value = _col2.GetComponent<IntValue>().GetValue();
        int col3Value = _col3.GetComponent<IntValue>().GetValue();

        Debug.Log("col1Value " + col1Value + " !== " + _col1Solution);
        Debug.Log("col2Value " + col2Value + " !== " + _col2Solution);
        Debug.Log("col3Value " + col3Value + " !== " + _col3Solution);

        if(_col1Solution == col1Value && _col2Solution == col2Value && _col3Solution == col3Value)
        {
            Debug.Log("Success!!!");
            _colButton.PressButton();
            DeactivatePuzzle();
        }
        else
        {
            Debug.Log("-PressAndPull-");
            _colButton.PressAndPull();
        }


        
    }
	#endregion
    
    #region Private methods
    private void DeactivatePuzzle()
    {
        _colButton.Deactivate();
        _col1.Deactivate();
        _col2.Deactivate();
        _col3.Deactivate();
    }

    #endregion
}
