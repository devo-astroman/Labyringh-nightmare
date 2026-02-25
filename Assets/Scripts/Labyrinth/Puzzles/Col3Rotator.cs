using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col3Rotator : MonoBehaviour
{

    #region Fields
    [SerializeField] private InterpolatorRotator _interpolatorRotatorUp;
    [SerializeField] private ColButton _colButtonUp;
    [SerializeField] private IntValue _intValueUp;


    [SerializeField] private InterpolatorRotator _interpolatorRotatorMiddle;
    [SerializeField] private ColButton _colButtonMiddle;
    [SerializeField] private IntValue _intValueMiddle;

    [SerializeField] private InterpolatorRotator _interpolatorRotatorBottom;
    [SerializeField] private ColButton _colButtonBottom;
    [SerializeField] private IntValue _intValueBottom;
    #endregion

    #region public properties    
    #endregion

    #region Private properties
    private int _id = 0;
    private int nRotationUp = 0;
    private int nRotationMiddle = 0;
    private int nRotationBottom = 0;
    #endregion
    #region Unity Callbacks    
    #endregion

    #region Public methods
    public void SetId(int id)
    {
        _id = id;
    }

    public int GetId()
    {
        return _id;
    }

    public void SetInitialRotations(int nRotationsUp, int nRotationsMiddle, int nRotationsBottom)
    {
        Debug.Log(" " + nRotationsUp + " " + nRotationsMiddle + " " + nRotationsBottom);
        _interpolatorRotatorUp.RotateNTimesZ(90,nRotationsUp);
        _intValueUp.SetValue(nRotationsUp);

        _interpolatorRotatorMiddle.RotateNTimesZ(90,nRotationsMiddle);
        _intValueMiddle.SetValue(nRotationsMiddle);

        _interpolatorRotatorBottom.RotateNTimesZ(90,nRotationsBottom);
        _intValueBottom.SetValue(nRotationsBottom);
    }

    public int[] GetIntValues()
    {
        return new int[] { _intValueUp.GetValue(), _intValueMiddle.GetValue(), _intValueBottom.GetValue() };
    }

    public void ButtonUpActionated(){
        _interpolatorRotatorUp.RotateDegreesZ(90,.5f);
        _intValueUp.IncreaseValue(1,0,4);
        _colButtonUp.PressAndPull();
    }
    public void ButtonMiddleActionated(){
        _interpolatorRotatorMiddle.RotateDegreesZ(-90,.5f);
        _intValueMiddle.DecreaseValue(1,0,4);
        _colButtonMiddle.PressAndPull();
    }
    public void ButtonBottomActionated(){
        _interpolatorRotatorBottom.RotateDegreesZ(90,.5f);
        _intValueBottom.IncreaseValue(1,0,4);
        _colButtonBottom.PressAndPull();
    }



    #endregion

    #region Private methods
    
    #endregion
}
