using UnityEngine;

public class HealthController : MonoBehaviour
{
    #region Fields
    #endregion

    #region Private properties	
    private int _life = 2;
    #endregion
    public void DecreaseLife(int amount)
    {
            _life -= amount;

            if(_life < 0) _life = 0;
            
    }

    public bool IsDead()
    {
        return _life <= 0;
    }

    #region Unity Callbacks
	
	#endregion



}
