using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireContact : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _bulletHolePrefab;

    public void OnHit(Vector3 hitPoint, Vector3 hitNormal)
    {
        // Offset to avoid z-fighting
        Vector3 spawnPos = hitPoint + hitNormal * 0.001f;

        // Rotation so decal faces outward
        Quaternion rotation = Quaternion.LookRotation(hitNormal);

        rotation *= Quaternion.Euler(90f, 0f, 0f); 

        Instantiate(_bulletHolePrefab, spawnPos, rotation);
    }

}
