using UnityEngine;

public class IntValue : MonoBehaviour
{

    #region Fields
    private int _intValue;
    #endregion

    #region public properties
    #endregion

    #region Private properties    
    #endregion
    #region Unity Callbacks    
    #endregion

    #region Public methods
    public int GetValue()
    {
        return _intValue;
    }
    public void SetValue(int value)
    {
        _intValue = value;
    }

    public void IncreaseValue(int amount, int downLimit, int upLimit)
    {
        int newValue = _intValue += amount;
        if(newValue >= upLimit)
            newValue = downLimit;

        _intValue = newValue;
    }

    public void DecreaseValue(int amount, int downLimit, int upLimit)
    {
        int newValue = _intValue -= amount;
        if(newValue < downLimit)
            newValue = upLimit-1;

        _intValue = newValue;
    }
	#endregion
    
    #region Private methods
    #endregion
}
