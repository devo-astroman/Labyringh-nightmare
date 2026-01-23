using UnityEngine;


[RequireComponent(typeof(SimpleCharacterController))]
public class CharacterAnimatorController : MonoBehaviour
{

    private SimpleCharacterController _simpleCharacterController;

    void Start()
    {
        _simpleCharacterController = GetComponent<SimpleCharacterController>();
    }

    void Update()
    {
        
    }

}
