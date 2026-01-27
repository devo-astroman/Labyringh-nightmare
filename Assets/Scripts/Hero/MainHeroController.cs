using UnityEngine;

public class MainHeroController : MonoBehaviour
{
    [SerializeField] private InputHeroController _inputHeroController;
    [SerializeField] private SimpleCharacterController _simpleCharacterController;

    [SerializeField] private HeroFSM _heroFSM;

    private string heroCurrentState = "Run";

    void Start()
    {
        //_heroFSM.Setup(_inputHeroController,_animatorHeroController);
        _inputHeroController.walkKeyPressed += HandleWalkKeyPressed;
        _inputHeroController.crouchKeyPressed += HandleCrouchKeyPressed;
        _inputHeroController.spaceKeyPressed += HandleSpaceKeyPressed;
        _inputHeroController.interactKeyPressed += HandleInteractKeyPressed;
        _inputHeroController.aimKeyPressed += HandleAimKeyPressed;
        //heroFSM.SetInputController(_inputHeroController);
    }

    void OnDestroy()
    {
         _inputHeroController.walkKeyPressed -= HandleWalkKeyPressed;
        _inputHeroController.crouchKeyPressed -= HandleCrouchKeyPressed;
        _inputHeroController.spaceKeyPressed -= HandleSpaceKeyPressed;        
        _inputHeroController.interactKeyPressed -= HandleInteractKeyPressed;
        _inputHeroController.aimKeyPressed -= HandleAimKeyPressed; 
    }

    private void HandleWalkKeyPressed(){
        //_heroFSM.WalkPressed();
        Debug.Log("HandleWalkKeyPressed");
        if(heroCurrentState == "Walk")
        {
            _heroFSM.GoRun();
            heroCurrentState = "Run";
        }
        else
        {
            _heroFSM.GoWalk();
            heroCurrentState = "Walk";            
        }
    }
    private void HandleCrouchKeyPressed(){
        //_heroFSM.CrouchPressed();
        Debug.Log("HandleCrouchKeyPressed");

        if(heroCurrentState == "Crouch")
        {
            if (_heroFSM.CanStandUp())
            {
                _heroFSM.GoRun();
                heroCurrentState = "Run";
            }
            else
            {
                Debug.Log("Player can't standup");
            }

        }
        else
        {
            _heroFSM.GoCrouch();
            heroCurrentState = "Crouch";
        }

    }

    private void HandleSpaceKeyPressed(){
        //_heroFSM.CrouchPressed();
        Debug.Log("HandleCrouchKeyPressed");

        if(heroCurrentState != "Crouch")
        {
            _simpleCharacterController.ApplyJump();
        }
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
