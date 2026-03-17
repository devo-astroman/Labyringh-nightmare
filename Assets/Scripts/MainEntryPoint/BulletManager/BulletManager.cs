using System.Collections.Generic;
using UnityEngine;

public struct BulletInfo
{
    public int Id;
    public bool IsActive;    
    public GameObject Go;
}
public class BulletManager : MonoBehaviour
{
    #region Fields
    #endregion

    #region Private Fields
    //private List<GameObject> _allAmmoBullets = new List<GameObject>();
    private List<BulletInfo> _allBulletsInfo = new List<BulletInfo>();
    [SerializeField] private int _nBulletsByAmmoBox = 6;
    [SerializeField] private int _nTotalBullets = 0;
    [SerializeField] private int _nTotalInactiveBullets = 0;
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks
    void OnDestroy()
    {
    }
    #endregion

    #region Public Methods
    public void RegisterBulletAmmo(GameObject bulletAmmo)
    {
        BulletInfo bInfo = new BulletInfo()
        {
            Id = _nTotalBullets,
            IsActive = true,
            Go = bulletAmmo
        };
        _allBulletsInfo.Add(bInfo);        
        bulletAmmo.GetComponent<Bullet>().SetId(_nTotalBullets);
        bulletAmmo.GetComponent<Bullet>().SetNBulletsByAmmoBox(_nBulletsByAmmoBox);

        bulletAmmo.GetComponent<Bullet>().BulletDeactivatedAction += HandleBulletDeactivated;

        _nTotalBullets++;
    }

    public void ActivateRandomBulletsAmmo(int nBulletsToActive)
    {

        for (int i = 0; i < _allBulletsInfo.Count && nBulletsToActive > 0; i++)
        {
            if (!_allBulletsInfo[i].IsActive)
            {
                BulletInfo bInfo = _allBulletsInfo[i];
                bInfo.IsActive = true;
                bInfo.Go.transform.parent.gameObject.SetActive(true);
                _allBulletsInfo[i] = bInfo;

                _nTotalInactiveBullets--;
                nBulletsToActive--;
            }
        }
        
    }

    public int GetNBulletsActive()
    {
        int nActive = 0;

        foreach (BulletInfo bInfo in _allBulletsInfo)
        {
            if (bInfo.IsActive)
            {
                nActive++;
            }
        }

        return nActive;
    }

    #endregion

    #region Private Methods
    private void HandleBulletDeactivated(int id)
    {
        Debug.Log("HandleBulletDeactivated " + id);
        for (int i = 0; i < _allBulletsInfo.Count; i++)
        {
            if (_allBulletsInfo[i].Id == id)
            {
                BulletInfo bInfo = _allBulletsInfo[i];
                bInfo.IsActive = false;
                _allBulletsInfo[i] = bInfo;

                _nTotalInactiveBullets++;
                break;
            }
        }
    }
    #endregion
}

