using UnityEngine;
using System;
public class Bullet : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _minimapIndicator;
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

    public void DeactivateBullet()
    {
        BulletDeactivatedAction?.Invoke(_id);
    }

    public void ShowInMinimap()
    {
        _minimapIndicator.SetActive(true);
    }

    public void HideInMinimap()
    {
        _minimapIndicator.SetActive(false);
    }

    #endregion

    #region Private Methods
    #endregion
}

