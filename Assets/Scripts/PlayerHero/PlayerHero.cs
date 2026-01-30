using UnityEngine;
using System;


public class PlayerHero : MonoBehaviour
{

    [SerializeField] private HeroFSM _heroFSM;
    public Action<Vector3,Vector3> onFireAction;

    void Start()
    {
        _heroFSM.onFireAction += HandleOnFireAction;
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        _heroFSM.onFireAction -= HandleOnFireAction;        
    }

    private void HandleOnFireAction(Vector3 hitPoint, Vector3 hitNormal)
    {
        onFireAction?.Invoke(hitPoint,hitNormal);
    }

}
