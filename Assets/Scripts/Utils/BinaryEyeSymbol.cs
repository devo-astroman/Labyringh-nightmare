using UnityEngine;

public class BinaryEyeSymbol : MonoBehaviour
{
    #region Fields
    private bool _isOpen = true;
    [SerializeField] MeshSwitcher _meshSwitcherOpen;
    [SerializeField] MeshSwitcher _meshSwitcherClose;
    #endregion

    #region public properties
    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks    
    #endregion

    #region Public methods
    public void Open()
    {
        _isOpen = true;
        UpdateSymbols();
    }
    public void Close()
    {
        _isOpen = false;
        UpdateSymbols();
    }
    public void Switch()
    {
        _isOpen = !_isOpen;
        UpdateSymbols();
    }

    public bool GetIsOpen()
    {
        return _isOpen;
    }

    public void SetIsOpen(bool isOpen)
    {
        _isOpen = isOpen;
        UpdateSymbols();
    }
	#endregion
    
    #region Private methods
    private void UpdateSymbols()
    {
        if (_isOpen)
        {
            _meshSwitcherOpen.Show();
            _meshSwitcherClose.Hide();
        }
        else
        {
            _meshSwitcherOpen.Hide();
            _meshSwitcherClose.Show();
        }
    }
    #endregion
}
