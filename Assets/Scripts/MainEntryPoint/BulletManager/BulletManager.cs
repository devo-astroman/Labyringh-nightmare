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
    [SerializeField] private int _nBulletsByAmmoBox = 2;
    [SerializeField] private int _nTotalBullets = 0;
    [SerializeField] private int _nTotalInactiveBullets = 0;
    #endregion

    #region Properties
    #endregion

    #region Unity Callbacks
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

    public void ActivateRandomBulletsAmmo(int nBulletsToActive, bool showInMinimap)
    {
        List<int> inactiveIndexes = new List<int>();

        for (int i = 0; i < _allBulletsInfo.Count; i++)
        {
            if (!_allBulletsInfo[i].IsActive)
            {
                inactiveIndexes.Add(i);
            }
        }

        if (inactiveIndexes.Count == 0)
        {
            return;
        }

        int amountToActivate = Mathf.Min(nBulletsToActive, inactiveIndexes.Count);

        for (int i = 0; i < amountToActivate; i++)
        {
            int randomListIndex = Random.Range(0, inactiveIndexes.Count);
            int bulletIndex = inactiveIndexes[randomListIndex];

            BulletInfo bInfo = _allBulletsInfo[bulletIndex];
            bInfo.IsActive = true;
            bInfo.Go.SetActive(true);

            if (showInMinimap)
            {
                Bullet bullet = bInfo.Go.GetComponent<Bullet>();
                if (bullet != null)
                {
                    bullet.ShowInMinimap();
                }
            }

            _allBulletsInfo[bulletIndex] = bInfo;

            _nTotalInactiveBullets--;

            inactiveIndexes.RemoveAt(randomListIndex);
        }
    }

    public BulletInfo GetBulletInfo(int id)
    {
        BulletInfo bInfo = new BulletInfo();
        for (int i = 0; i < _allBulletsInfo.Count; i++)
        {
            if(_allBulletsInfo[i].Id == id)
            {
                bInfo = _allBulletsInfo[i];                
            }
        }

        return bInfo;
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
                //bInfo.Go.transform.parent.gameObject.SetActive(false);
                bInfo.Go.GetComponent<AmmoOnBox>().MakeGlowOff();
                bInfo.Go.SetActive(false);
                break;
            }
        }
    }
    #endregion
}

