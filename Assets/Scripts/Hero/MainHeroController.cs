using UnityEngine;

public class MainHeroController : MonoBehaviour
{
    [SerializeField] private InputHeroController _inputHeroController;


    void Start()
    {
        _inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
        _inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
        _inputHeroController.interactKeyPressed += HandleInteractKeyPressed;
        _inputHeroController.aimKeyPressed += HandleAimKeyPressed;
    }

    void OnDestroy()
    {
        _inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
        _inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
        _inputHeroController.interactKeyPressed -= HandleInteractKeyPressed;
        _inputHeroController.aimKeyPressed -= HandleAimKeyPressed;
    }

    private void HandleWalkKeyPressed(){

    }
    private void HandleCrouchKeyPressed(){
        
    }
    private void HandleInteractKeyPressed(){
        
    }
    private void HandleAimKeyPressed(){
        
    }


}
