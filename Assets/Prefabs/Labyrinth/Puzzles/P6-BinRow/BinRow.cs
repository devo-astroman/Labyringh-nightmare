using UnityEngine;


public class BinBinary : MonoBehaviour
{
    #region Fields
    [SerializeField] private bool[] _currentLeft;    
    [SerializeField] private bool[] _solutionLeft;

    [SerializeField] private bool[] _currentRight;    
    [SerializeField] private bool[] _solutionRight;
    #endregion

    #region public properties
    #endregion

    #region Private properties
    [SerializeField] private CompassCoord[] _leftRow;
    [SerializeField] private CompassCoord[] _rightRow;    

    #endregion
    #region Unity Callbacks
    void Start()
    {

        SetInitialValues();

        for(int i = 0; i < _leftRow.Length; i++)
        {
            _leftRow[i].ColButton.SetId(i);
            _leftRow[i].ColButton.buttonIteractAction += HandleLeftButtonInteract;
            SetCompassCoordValue(_leftRow[i], _currentLeft[i]);
        }

        for(int i = 0; i < _rightRow.Length; i++)
        {
            _rightRow[i].ColButton.SetId(i);
            _rightRow[i].ColButton.buttonIteractAction += HandleRightButtonInteract;
            SetCompassCoordValue(_rightRow[i], _currentLeft[i]);
        }

    }

    void OnDestroy()
    {
        for(int i = 0; i < _leftRow.Length; i++)
        {
            _leftRow[i].ColButton.buttonIteractAction -= HandleLeftButtonInteract;
        }

        for(int i = 0; i < _rightRow.Length; i++)
        {
            _rightRow[i].ColButton.buttonIteractAction -= HandleRightButtonInteract;
        }
    }
    #endregion

    #region Public methods
    public void InitialSetup(bool[] current, bool[] solution)
    {//Use this to define different solutions
        _currentLeft = current;
        _solutionLeft = solution;

        SetInitialValues(); 
    }
    
	#endregion
    
    #region Private methods
    private void SetInitialValues()
    {

        for(int i = 0; i < _leftRow.Length; i++)
        {
            SetSymbolsCoordValue(_leftRow[i], _solutionLeft[i]);
            SetCompassCoordValue(_leftRow[i], _currentLeft[i]);
        }

        for(int i = 0; i < _rightRow.Length; i++)
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
        //compassCoord.Sym.Switch();
        compassCoord.MeshSwitcher.Switch();
    }

    private void HandleLeftButtonInteract(int id)
    {
        SwitchCompassCoordValue(_leftRow[id]);
        CheckSolutionReached(_leftRow[id].ColButton);
    }

    private void HandleRightButtonInteract(int id)
    {
        SwitchCompassCoordValue(_rightRow[id]);
        CheckSolutionReached(_rightRow[id].ColButton);
    }


    private void CheckSolutionReached(ColButton colButton)
    {
        if (IsSolutionReached())
        {
            Debug.Log("SOLUTION REACHED");
            Deactivate();
        }
        else
        {
            Debug.Log("Try again");
            colButton.PressAndPull();
        }
    }

    private bool IsSolutionReached()
    {
        for(int i = 0; i < _leftRow.Length; i++)
        {
            bool current = _leftRow[i].MeshSwitcher.GetIsVisible();
            if(current != _solutionLeft[i])
            {
                return false;
            }
        }

        for(int i = 0; i < _rightRow.Length; i++)
        {
            bool current = _rightRow[i].MeshSwitcher.GetIsVisible();
            if(current != _solutionRight[i])
            {
                return false;
            }
        }

        return true;
    }

    private bool Deactivate()
    {
        for(int i = 0; i < _leftRow.Length; i++)
        {
            _leftRow[i].ColButton.PressButton();
            _leftRow[i].ColButton.Deactivate();
        }

        for(int i = 0; i < _rightRow.Length; i++)
        {
            _rightRow[i].ColButton.PressButton();
            _rightRow[i].ColButton.Deactivate();
        }

        return true;
    }
    
    #endregion
}
