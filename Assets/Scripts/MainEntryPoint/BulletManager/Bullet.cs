using UnityEngine;
using System;
public class Bullet : MonoBehaviour
{
    #region Fields
    #endregion

    #region Private Fields
    private int _id = 0;
    private int _nBulletsByAmmoBox = 2;
    #endregion

    #region Public Fields
    public Action<int> BulletDeactivatedAction;
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks
    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        Debug.Log("OnDisable " + _id);
        BulletDeactivatedAction?.Invoke(_id);   
    }
    #endregion

    #region Public Methods
    public void SetId(int id)
    {
        _id = id;
    }
    public void SetNBulletsByAmmoBox(int nBulletsByAmmoBox)
    {
        _nBulletsByAmmoBox = nBulletsByAmmoBox;
    }

    public int GetNBulletsByAmmoBox()
    {
        return _nBulletsByAmmoBox;
    }


    #endregion

    #region Private Methods
    #endregion
}

