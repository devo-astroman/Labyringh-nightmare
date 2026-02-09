using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXsManager : MonoBehaviour
{
    [SerializeField] private WallBulletContact _wallBulletContact;

    [SerializeField] private GameObject _bloodVFXs;
    

    public void ShowHitWallVFXs(Vector3 hitPoint, Vector3 hitNormal)
    {
        _wallBulletContact.OnHit(hitPoint,hitNormal);
    }

    public void ShowBloodVFXs(Vector3 hitPoint, Vector3 hitNormal)
    {
        //Should show the blood particle effect in that coords
        
        Vector3 spawnPos = hitPoint + hitNormal * 0.001f;

        // Rotation so decal faces outward
        Quaternion rotation = Quaternion.LookRotation(hitNormal);

        rotation *= Quaternion.Euler(90f, 0f, 0f); 

        Instantiate(_bloodVFXs, spawnPos, rotation);

    }
}
