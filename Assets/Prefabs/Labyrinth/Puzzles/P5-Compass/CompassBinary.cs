using System;
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



public class CompassBinary : Puzzle
{
    #region Fields
    [SerializeField] private Compass _current;
    [SerializeField] private Compass _solution;
    [SerializeField] private int _id;
    [SerializeField] GameObject _signalVfx;
    #endregion

    #region public properties
    public Action<int> PuzzleSolvedAction;
    #endregion

    #region Private properties
    [SerializeField] PuzzleNotifier _puzzleNotifier;
    [SerializeField] private CompassCoord _compassCoordN;
    [SerializeField] private CompassCoord _compassCoordS;
    [SerializeField] private CompassCoord _compassCoordE;
    [SerializeField] private CompassCoord _compassCoordW;

    private bool _originalMeshSwitcherN;
    private bool _originalMeshSwitcherS;
    private bool _originalMeshSwitcherE;
    private bool _originalMeshSwitcherW;

    #endregion
    #region Unity Callbacks
    void Start()
    {
        _originalMeshSwitcherN = _compassCoordN.MeshSwitcher.GetIsVisible();
        _originalMeshSwitcherS = _compassCoordS.MeshSwitcher.GetIsVisible();
        _originalMeshSwitcherE = _compassCoordE.MeshSwitcher.GetIsVisible();
        _originalMeshSwitcherW = _compassCoordW.MeshSwitcher.GetIsVisible();
        
        SetSymbolValues();

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

        SetSymbolValues();
    }

    public override void Activate()
    {
       _compassCoordN.ColButton.PullButton();
       _compassCoordN.ColButton.Activate();

       _compassCoordE.ColButton.PullButton();
       _compassCoordE.ColButton.Activate();

       _compassCoordS.ColButton.PullButton();
       _compassCoordS.ColButton.Activate();

       _compassCoordW.ColButton.PullButton();
       _compassCoordW.ColButton.Activate();
       _signalVfx.SetActive(true);
    }

    public override void Deactivate()
    {
       _compassCoordN.ColButton.PressButton();
       _compassCoordN.ColButton.Deactivate();

       _compassCoordE.ColButton.PressButton();
       _compassCoordE.ColButton.Deactivate();

       _compassCoordS.ColButton.PressButton();
       _compassCoordS.ColButton.Deactivate();

       _compassCoordW.ColButton.PressButton();
       _compassCoordW.ColButton.Deactivate();
       _signalVfx.SetActive(false);
    }

    public override void Reset()
    {
        _compassCoordN.MeshSwitcher.SetIsVisible(_originalMeshSwitcherN);
        _compassCoordS.MeshSwitcher.SetIsVisible(_originalMeshSwitcherS);
        _compassCoordE.MeshSwitcher.SetIsVisible(_originalMeshSwitcherE);
        _compassCoordW.MeshSwitcher.SetIsVisible(_originalMeshSwitcherW); 
    }
    
	#endregion
    
    #region Private methods
    private void SetSymbolValues()
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

    private void HandleButtonNIteract(int id)
    {
        SwitchCompassCoordValue(_compassCoordN);
        CheckSolutionReached(_compassCoordN.ColButton);
    }

    private void HandleButtonSIteract(int id)
    {
        SwitchCompassCoordValue(_compassCoordS);
        CheckSolutionReached(_compassCoordS.ColButton);
    }

    private void HandleButtonEIteract(int id)
    {
        SwitchCompassCoordValue(_compassCoordE);
        CheckSolutionReached(_compassCoordE.ColButton);
    }


    private void HandleButtonWIteract(int id)
    {
        SwitchCompassCoordValue(_compassCoordW);
        CheckSolutionReached(_compassCoordW.ColButton);
    }

    private void CheckSolutionReached(ColButton colButton)
    {
        if (IsSolutionReached())
        {
            Debug.Log("SOLUTION REACHED");
            Deactivate();
            PuzzleSolvedAction?.Invoke(_id);
            _puzzleNotifier.NotifyPuzzleSolved(_id);
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
