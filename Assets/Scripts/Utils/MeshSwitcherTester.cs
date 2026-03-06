using UnityEngine;

public class MeshSwitcherTester : MonoBehaviour
{
    #region Fields
    [SerializeField] private bool _test;
    [SerializeField] MeshSwitcher _meshSwitcher;
    #endregion

    #region public properties
    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks
    void Update()
    {
        if (_test)
        {
            _meshSwitcher.Switch();
            Debug.Log("Is visible? " + _meshSwitcher.GetIsVisible());            
            _test = false;
        }
    }
    #endregion

    #region Public methods    
	#endregion
    
    #region Private methods    
    #endregion
}
