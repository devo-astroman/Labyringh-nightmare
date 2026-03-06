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
        SetCompassCoordValue(_compassCoordN, _current.North);
        SetCompassCoordValue(_compassCoordS, _current.South);
        SetCompassCoordValue(_compassCoordE, _current.East);
        SetCompassCoordValue(_compassCoordW, _current.West);

    }
    #endregion

    #region Public methods
    
	#endregion
    
    #region Private methods
    private void SetCompassCoordValue(CompassCoord compassCoord, bool value)
    {
        Debug.Log("Set compass values " + value);
        compassCoord.Sym.SetIsOpen(value);
        compassCoord.MeshSwitcher.SetIsVisible(value);
    }
    
    #endregion
}
