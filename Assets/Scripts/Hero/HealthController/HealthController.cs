using UnityEngine;

public class HealthController : MonoBehaviour
{
    #region Fields
    #endregion

    #region Private properties	
    private int _maxLife = 10;
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

    public int GetMaxLife()
    {
        return _maxLife;
    }

    public float GetCurrentLife()
    {
        if (_maxLife <= 0) return 0f;

        return _life;
    }

    #region Unity Callbacks
	void Start()
    {
        _life = _maxLife;
    }
	#endregion



}
