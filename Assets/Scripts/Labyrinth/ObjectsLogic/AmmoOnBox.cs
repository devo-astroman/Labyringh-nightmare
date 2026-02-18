using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoOnBox : MonoBehaviour
{

    #region Fields
    [SerializeField] private GameObject[] _ammoBoxesGO;
    #endregion

    #region public properties    
    #endregion

    #region Private properties
    private int _id = 0;
    #endregion
    #region Unity Callbacks
    void Start()
    {
        ShowOneRandomAmmoBox();
    }
    #endregion

    #region Public methods
    public void SetId(int id)
    {
        _id = id;
    }

    public int GetId()
    {
        return _id;
    }
    public void ShowOneRandomAmmoBox()
    {
        if (_ammoBoxesGO == null || _ammoBoxesGO.Length == 0)
        {
            Debug.LogWarning("No ammo boxes assigned.");
            return;
        }

        // Deactivate all first
        for (int i = 0; i < _ammoBoxesGO.Length; i++)
        {
            if (_ammoBoxesGO[i] != null)
                _ammoBoxesGO[i].SetActive(false);
        }

        // Pick random index
        int randomIndex = Random.Range(0, _ammoBoxesGO.Length);

        // Activate selected one
        if (_ammoBoxesGO[randomIndex] != null)
            _ammoBoxesGO[randomIndex].SetActive(true);
    }

    
	#endregion
    // Start is called before the first frame update

}
