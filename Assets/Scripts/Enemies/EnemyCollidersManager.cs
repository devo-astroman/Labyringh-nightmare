using UnityEngine;


public class EnemyCollidersManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _colliders;
    [SerializeField] private GameObject _baseCollider;

    void Start()
    {
        HideColliderVisibility();
    }


    public void HideColliderVisibility()
    {
        for(int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].GetComponent<MeshRenderer>().enabled = false;
        }

        _baseCollider.GetComponent<MeshRenderer>().enabled = false;
    }

    public void RemoveCollisions()
    {
        for(int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].GetComponent<CapsuleCollider>().enabled = false;
        }

    }


    public void DeactivatePainColliders()
    {
        for(int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].GetComponent<CapsuleCollider>().enabled = false;
        }
        _baseCollider.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void ActivatePainColliders()
    {
        for(int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].GetComponent<CapsuleCollider>().enabled = true;
        }
        _baseCollider.GetComponent<CapsuleCollider>().enabled = true;
    }





}
