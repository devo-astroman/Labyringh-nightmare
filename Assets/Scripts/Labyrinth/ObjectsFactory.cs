using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsFactory : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _boxAllFacesPrefab;
    [SerializeField] private GameObject _box2EmptiesPrefab;

    [SerializeField] private GameObject _ammoOnBoxPrefab;
     
     private List<int> _allAmmoBoxIdsCreated = new List<int>();

    

    public GameObject GetBox2Empties(Vector3 position)
    {
        GameObject box = Instantiate(_box2EmptiesPrefab,position,Quaternion.identity);
        return box;
    }

    public GameObject GetAmmoOnBox(Vector3 position, int id)
    {
        if (!_allAmmoBoxIdsCreated.Contains(id))
        {
            GameObject ammoOnBox = Instantiate(_ammoOnBoxPrefab,position,Quaternion.identity);
            ammoOnBox.GetComponent<AmmoOnBox>().SetId(id);

            _allAmmoBoxIdsCreated.Add(id);
            return ammoOnBox;

        }
        
        return null;
    }
}
