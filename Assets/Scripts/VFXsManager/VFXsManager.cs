using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXsManager : MonoBehaviour
{
    [SerializeField] private WallBulletContact _wallBulletContact;
    

    public void ShowHitWallVFXs(Vector3 hitPoint, Vector3 hitNormal)
    {
        _wallBulletContact.OnHit(hitPoint,hitNormal);
    }
}
