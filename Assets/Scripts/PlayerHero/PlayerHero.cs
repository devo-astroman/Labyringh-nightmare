using UnityEngine;
using System;


public class PlayerHero : MonoBehaviour
{

    [SerializeField] private HeroFSM _heroFSM;
    public Action<Vector3,Vector3,RaycastHit> onFireAction;

    void Start()
    {
        _heroFSM.onFireAction += HandleOnFireAction;
    }

    void Update()
    {
    }

    public void HitToHero()
    {
        Debug.Log("HIT TO HERO!!!");
        _heroFSM.TriggerReceiveHitFromEnemy();
    }

    void OnDestroy()
    {
        _heroFSM.onFireAction -= HandleOnFireAction;        
    }

    private void HandleOnFireAction(Vector3 hitPoint, Vector3 hitNormal, RaycastHit hit)
    {
        onFireAction?.Invoke(hitPoint,hitNormal,hit);
    }

}
