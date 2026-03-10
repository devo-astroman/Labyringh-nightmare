using System;
using UnityEngine;

public class RotatorDisc2Puzzle : ActivatableBehaviour
{
    [SerializeField] PointerLookAtCycler _pointerLookAtCyclerUp;
    [SerializeField] PointerLookAtCycler _pointerLookAtCyclerMiddle;

    [SerializeField] private int _id;

    [SerializeField] private int _currentUp;
    [SerializeField] private int _currentMiddle;   

    [SerializeField] ColButton _colButtonUp;
    [SerializeField] ColButton _colButtonMiddle;

    [SerializeField] PuzzleNotifier _puzzleNotifier;

    public Action<int> PuzzleSolvedAction;


    void Start()
    {
        _pointerLookAtCyclerUp.RotateInstantSteps(_currentUp);
        _pointerLookAtCyclerMiddle.RotateInstantSteps(_currentMiddle);        

        _pointerLookAtCyclerUp.OnRotationFinished += HandleRotationFinishedUp;
        _pointerLookAtCyclerMiddle.OnRotationFinished += HandleRotationFinishedMiddle;        

        _colButtonUp.SetId(0);
        _colButtonUp.buttonIteractAction += HandleButtonIteractAction;

        _colButtonMiddle.SetId(1);
        _colButtonMiddle.buttonIteractAction += HandleButtonIteractAction;
    }

    void OnDestroy()
    {
        _pointerLookAtCyclerUp.OnRotationFinished -= HandleRotationFinishedUp;
        _pointerLookAtCyclerMiddle.OnRotationFinished -= HandleRotationFinishedMiddle;        

        _colButtonUp.buttonIteractAction -= HandleButtonIteractAction;
        _colButtonMiddle.buttonIteractAction -= HandleButtonIteractAction;
    }

    public override void Activate()
    {
        _colButtonUp.PullButton();
        _colButtonUp.Activate();
        _colButtonMiddle.PullButton();
        _colButtonMiddle.Activate();
    }

    public override void Deactivate()
    {
        Debug.Log("_*_Deactivate_*_");
        _colButtonUp.PressButton();
        _colButtonUp.Deactivate();
        _colButtonMiddle.PressButton();
        _colButtonMiddle.Deactivate();
    }

    private void HandleRotationFinishedUp(int currentIndex, Transform target)
    {
        _currentUp = currentIndex;
        CheckSolutionReached();
    }

    private void HandleRotationFinishedMiddle(int currentIndex, Transform target)
    {
        _currentMiddle = currentIndex;
        CheckSolutionReached();
    }

    private void HandleButtonIteractAction(int idColButton)
    {
        if(idColButton == 0)
        {
            _pointerLookAtCyclerUp.Next();
        }else if(idColButton == 1)
        {

            _pointerLookAtCyclerMiddle.Prev();

        }
    }

    

    private void CheckSolutionReached()
    {
        if(_currentMiddle == 0 && _currentUp == 0)
        {
            Debug.Log("Solution Reached!");
            PuzzleSolvedAction?.Invoke(_id);
            _puzzleNotifier.NotifyPuzzleSolved(_id);
            Deactivate();
        }
        else
        {
            Debug.Log("Try again!");
        }
    }



}
