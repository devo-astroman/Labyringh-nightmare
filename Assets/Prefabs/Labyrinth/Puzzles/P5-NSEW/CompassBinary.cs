using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Compass
{
    public bool North;
    public bool South;
    public bool East;
    public bool West;
}

[System.Serializable]
public struct CompassCoord
{
    public BinaryEyeSymbol Sym;
    public MeshSwitcher MeshSwitcher;
    public ColButton ColButton;
}



public class CompassBinary : MonoBehaviour
{
    #region Fields
    [SerializeField] private Compass _current;
    [SerializeField] private Compass _solution;
    #endregion

    #region public properties
    #endregion

    #region Private properties
    [SerializeField] private CompassCoord _compassCoordN;
    [SerializeField] private CompassCoord _compassCoordS;
    [SerializeField] private CompassCoord _compassCoordE;
    [SerializeField] private CompassCoord _compassCoordW;

    #endregion
    #region Unity Callbacks
    void Start()
    {

        SetInitialValues();

        _compassCoordN.ColButton.buttonIteractAction += HandleButtonNIteract;
        _compassCoordS.ColButton.buttonIteractAction += HandleButtonSIteract;
        _compassCoordE.ColButton.buttonIteractAction += HandleButtonEIteract;
        _compassCoordW.ColButton.buttonIteractAction += HandleButtonWIteract;

    }

    void OnDestroy()
    {
        _compassCoordN.ColButton.buttonIteractAction -= HandleButtonNIteract;
        _compassCoordS.ColButton.buttonIteractAction -= HandleButtonSIteract;
        _compassCoordE.ColButton.buttonIteractAction -= HandleButtonEIteract;
        _compassCoordW.ColButton.buttonIteractAction -= HandleButtonWIteract;

    }
    #endregion

    #region Public methods
    public void InitialSetup(Compass current, Compass solution)
    {//Use this to define different solutions
        _current = current;
        _solution = solution;

        SetInitialValues();
    }
    
	#endregion
    
    #region Private methods
    private void SetInitialValues()
    {
        SetSymbolsCoordValue(_compassCoordN, _solution.North);
        SetSymbolsCoordValue(_compassCoordS, _solution.South);
        SetSymbolsCoordValue(_compassCoordE, _solution.East);
        SetSymbolsCoordValue(_compassCoordW, _solution.West);

        SetCompassCoordValue(_compassCoordN, _current.North);
        SetCompassCoordValue(_compassCoordS, _current.South);
        SetCompassCoordValue(_compassCoordE, _current.East);
        SetCompassCoordValue(_compassCoordW, _current.West);
    }
    private void SetSymbolsCoordValue(CompassCoord compassCoord, bool value)
    {
        compassCoord.Sym.SetIsOpen(value);
    }

    private void SetCompassCoordValue(CompassCoord compassCoord, bool value)
    {
        compassCoord.MeshSwitcher.SetIsVisible(value);
    }

    private void SwitchCompassCoordValue(CompassCoord compassCoord)
    {
        //compassCoord.Sym.Switch();
        compassCoord.MeshSwitcher.Switch();
    }

    private void HandleButtonNIteract()
    {
        SwitchCompassCoordValue(_compassCoordN);
        CheckSolutionReached(_compassCoordN.ColButton);
    }

        private void HandleButtonSIteract()
    {
        SwitchCompassCoordValue(_compassCoordS);
        CheckSolutionReached(_compassCoordS.ColButton);
    }

    private void HandleButtonEIteract()
    {
        SwitchCompassCoordValue(_compassCoordE);
        CheckSolutionReached(_compassCoordE.ColButton);
    }


    private void HandleButtonWIteract()
    {
        SwitchCompassCoordValue(_compassCoordW);
        CheckSolutionReached(_compassCoordW.ColButton);
    }

    private void CheckSolutionReached(ColButton colButton)
    {
        if (IsSolutionReached())
        {
            Debug.Log("SOLUTION REACHED");
            _compassCoordN.ColButton.PressButton();
            _compassCoordN.ColButton.Deactivate();

            _compassCoordE.ColButton.PressButton();
            _compassCoordE.ColButton.Deactivate();

            _compassCoordS.ColButton.PressButton();
            _compassCoordS.ColButton.Deactivate();

            _compassCoordW.ColButton.PressButton();
            _compassCoordW.ColButton.Deactivate();

        }
        else
        {
            Debug.Log("Try again");
            colButton.PressAndPull();
        }
    }

    private bool IsSolutionReached()
    {
        bool isOpenN = _compassCoordN.MeshSwitcher.GetIsVisible();
        bool isOpenS = _compassCoordS.MeshSwitcher.GetIsVisible();
        bool isOpenE = _compassCoordE.MeshSwitcher.GetIsVisible();
        bool isOpenW = _compassCoordW.MeshSwitcher.GetIsVisible();

        return _solution.North == isOpenN && _solution.South == isOpenS && _solution.East == isOpenE && _solution.West == isOpenW;
    }
    
    #endregion
}
