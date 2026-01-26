using UnityEngine;

public class MainHeroController : MonoBehaviour
{
    [SerializeField] private InputHeroController _inputHeroController;
    [SerializeField] private Animator _animator;

    [SerializeField] private HeroFSM _heroFSM;

    void Start()
    {
        _heroFSM.Setup(_inputHeroController,_animator);

         _inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
        _inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
        _inputHeroController.interactKeyPressed += HandleInteractKeyPressed;
        _inputHeroController.aimKeyPressed += HandleAimKeyPressed; 
        //heroFSM.SetInputController(_inputHeroController);
    }

    void OnDestroy()
    {
         _inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
        _inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
        _inputHeroController.interactKeyPressed -= HandleInteractKeyPressed;
        _inputHeroController.aimKeyPressed -= HandleAimKeyPressed; 
    }

    private void HandleWalkKeyPressed(){
        //_heroFSM.WalkPressed();
        Debug.Log("HandleWalkKeyPressed");
        _heroFSM.GoWalk();
    }
    private void HandleCrouchKeyPressed(){
        //_heroFSM.CrouchPressed();
        Debug.Log("HandleCrouchKeyPressed");
        _heroFSM.GoCrouch();
    }
    private void HandleInteractKeyPressed(){
        //_heroFSM.InteractPressed();
        Debug.Log("HandleInteractKeyPressed");
    }
    private void HandleAimKeyPressed(){
        //_heroFSM.AimPressed();
        Debug.Log("HandleAimKeyPressed");
    }


}
