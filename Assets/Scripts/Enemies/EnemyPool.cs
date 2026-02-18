using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{   
    #region Fields
    #endregion

    #region public properties    
    #endregion

    #region Private properties
    private List<GameObject> _allEnemies = new List<GameObject>();
    #endregion
    #region Unity Callbacks
	#endregion

    #region Public methods
    public void AddEnemy(GameObject enemy)
    {
        _allEnemies.Add(enemy);
    }
    
	#endregion




}
