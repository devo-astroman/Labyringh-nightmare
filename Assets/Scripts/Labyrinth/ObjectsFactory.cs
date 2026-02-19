using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsFactory : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _boxAllFacesPrefab;
    [SerializeField] private GameObject _box2EmptiesPrefab;
    [SerializeField] private GameObject _ammoOnBoxPrefab;
    [SerializeField] private GameObject _spikesPrefab;
     
     private List<int> _allAmmoBoxIdsCreated = new List<int>();
     private List<int> _allSpikesIdsCreated = new List<int>();

    

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

    public GameObject GetSpikes(Vector3 position, int id)
    {
        if (!_allSpikesIdsCreated.Contains(id))
        {
            GameObject spikes = Instantiate(_spikesPrefab,position,Quaternion.identity);
            spikes.GetComponent<Spikes>().SetId(id);

            _allSpikesIdsCreated.Add(id);
            return spikes;

        }
        
        return null;
    }
}
