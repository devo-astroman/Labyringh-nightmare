using UnityEngine;
using System;

public class BinRow : Puzzle
{
    #region Fields
    [SerializeField] private bool[] _currentLeft;
    [SerializeField] private bool[] _solutionLeft;

    [SerializeField] private bool[] _currentRight;
    [SerializeField] private bool[] _solutionRight;
    [SerializeField] SfxManager _sfxManager;

    private int _id;
    [SerializeField] private PuzzleNotifier _puzzleNotifier;
    [SerializeField] private CompassCoord[] _leftRow;
    [SerializeField] private CompassCoord[] _rightRow;
    [SerializeField] private GameObject _signalVfx;
    #endregion

    #region Events
    public Action<int> PuzzleSolvedAction;
    #endregion

    #region Private Fields
    private bool[] _originalLeftValues;
    private bool[] _originalRightValues;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        _originalLeftValues = new bool[_leftRow.Length];
        _originalRightValues = new bool[_rightRow.Length];

        for (int i = 0; i < _leftRow.Length; i++)
        {
            _originalLeftValues[i] = _leftRow[i].MeshSwitcher.GetIsVisible();
        }

        for (int i = 0; i < _rightRow.Length; i++)
        {
            _originalRightValues[i] = _rightRow[i].MeshSwitcher.GetIsVisible();
        }

        SetInitialValues();

        for (int i = 0; i < _leftRow.Length; i++)
        {
            _leftRow[i].ColButton.SetId(i);
            _leftRow[i].ColButton.buttonIteractAction += HandleLeftButtonInteract;
            SetCompassCoordValue(_leftRow[i], _currentLeft[i]);
        }

        for (int i = 0; i < _rightRow.Length; i++)
        {
            _rightRow[i].ColButton.SetId(i);
            _rightRow[i].ColButton.buttonIteractAction += HandleRightButtonInteract;
            SetCompassCoordValue(_rightRow[i], _currentRight[i]);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _leftRow.Length; i++)
        {
            _leftRow[i].ColButton.buttonIteractAction -= HandleLeftButtonInteract;
        }

        for (int i = 0; i < _rightRow.Length; i++)
        {
            _rightRow[i].ColButton.buttonIteractAction -= HandleRightButtonInteract;
        }
    }
    #endregion

    #region Public Methods
    public void InitialSetup(bool[] current, bool[] solution)
    {
        _currentLeft = current;
        _solutionLeft = solution;

        SetInitialValues();
    }

    public override void SetId(int id)
    {
        _id = id;
    }

    public override void Activate()
    {
        for (int i = 0; i < _leftRow.Length; i++)
        {
            _leftRow[i].ColButton.PullButton();
            _leftRow[i].ColButton.Activate();
        }

        for (int i = 0; i < _rightRow.Length; i++)
        {
            _rightRow[i].ColButton.PullButton();
            _rightRow[i].ColButton.Activate();
        }

        if (_signalVfx != null)
            _signalVfx.SetActive(true);
    }

    public override void Deactivate()
    {
        for (int i = 0; i < _leftRow.Length; i++)
        {
            _leftRow[i].ColButton.PressButton();
            _leftRow[i].ColButton.Deactivate();
        }

        for (int i = 0; i < _rightRow.Length; i++)
        {
            _rightRow[i].ColButton.PressButton();
            _rightRow[i].ColButton.Deactivate();
        }

        if (_signalVfx != null)
            _signalVfx.SetActive(false);
    }

    public override void Reset()
    {
        for (int i = 0; i < _leftRow.Length; i++)
        {
            _leftRow[i].MeshSwitcher.SetIsVisible(_originalLeftValues[i]);
        }

        for (int i = 0; i < _rightRow.Length; i++)
        {
            _rightRow[i].MeshSwitcher.SetIsVisible(_originalRightValues[i]);
        }
    }
    #endregion

    #region Private Methods
    private void SetInitialValues()
    {
        for (int i = 0; i < _leftRow.Length; i++)
        {
            SetSymbolsCoordValue(_leftRow[i], _solutionLeft[i]);
            SetCompassCoordValue(_leftRow[i], _currentLeft[i]);
        }

        for (int i = 0; i < _rightRow.Length; i++)
        {
            SetSymbolsCoordValue(_rightRow[i], _solutionRight[i]);
            SetCompassCoordValue(_rightRow[i], _currentRight[i]);
        }
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
        compassCoord.MeshSwitcher.Switch();
    }

    private void HandleLeftButtonInteract(int id)
    {
        SwitchCompassCoordValue(_leftRow[id]);
        CheckSolutionReached(_leftRow[id].ColButton);
        _sfxManager.PlayClip1();
    }

    private void HandleRightButtonInteract(int id)
    {
        SwitchCompassCoordValue(_rightRow[id]);
        CheckSolutionReached(_rightRow[id].ColButton);
        _sfxManager.PlayClip1();
    }

    private void CheckSolutionReached(ColButton colButton)
    {
        if (IsSolutionReached())
        {
            Debug.Log("SOLUTION REACHED");
            Deactivate();
            PuzzleSolvedAction?.Invoke(_id);
            _puzzleNotifier?.NotifyPuzzleSolved(_id);
        }
        else
        {
            Debug.Log("Try again");
            colButton.PressAndPull();
        }
    }

    private bool IsSolutionReached()
    {
        for (int i = 0; i < _leftRow.Length; i++)
        {
            bool current = _leftRow[i].MeshSwitcher.GetIsVisible();
            if (current != _solutionLeft[i])
                return false;
        }

        for (int i = 0; i < _rightRow.Length; i++)
        {
            bool current = _rightRow[i].MeshSwitcher.GetIsVisible();
            if (current != _solutionRight[i])
                return false;
        }

        return true;
    }
    #endregion
}