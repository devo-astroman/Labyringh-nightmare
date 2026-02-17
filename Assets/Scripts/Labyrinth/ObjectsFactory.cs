using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsFactory : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _boxAllFacesPrefab;
    [SerializeField] private GameObject _box2EmptiesPrefab;

    public GameObject GetBox2Empties(Vector3 position)
    {
        GameObject box = Instantiate(_box2EmptiesPrefab,position,Quaternion.identity);
        return box;
    }
}
