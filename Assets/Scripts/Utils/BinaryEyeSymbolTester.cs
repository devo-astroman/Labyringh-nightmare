using UnityEngine;

public class BinaryEyeSymbolTester : MonoBehaviour
{
    #region Fields
    [SerializeField] private bool _openTest = false;
    [SerializeField] BinaryEyeSymbol _binaryEyeSymbol;
    #endregion

    #region public properties
    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks
    void Update()
    {
        if (_openTest)
        {
            _binaryEyeSymbol.Switch();
            Debug.Log("isOpen " + _binaryEyeSymbol.GetIsOpen());
            _openTest = false;
        }
    }
    #endregion

    #region Public methods    
    #endregion

    #region Private methods
    #endregion
}
