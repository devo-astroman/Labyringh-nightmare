using System;
using UnityEngine;

public class RotatorDisc1Puzzle : Puzzle
{
    [SerializeField] PointerLookAtCycler _pointerLookAtCyclerUp;
    

    [SerializeField] private int _id;

    [SerializeField] private int _currentUp;
    private int _originalUp;
    

    [SerializeField] ColButton _colButtonUp;    

    [SerializeField] PuzzleNotifier _puzzleNotifier;

    [SerializeField] GameObject _signalVfx;

    public Action<int> PuzzleSolvedAction;


    void Start()
    {
        _originalUp = _currentUp;
        _pointerLookAtCyclerUp.RotateInstantSteps(_currentUp);

        _pointerLookAtCyclerUp.OnRotationFinished += HandleRotationFinishedUp;        

        _colButtonUp.SetId(0);
        _colButtonUp.buttonIteractAction += HandleButtonIteractAction;
                
    }

    void OnDestroy()
    {
        _pointerLookAtCyclerUp.OnRotationFinished -= HandleRotationFinishedUp;        

        _colButtonUp.buttonIteractAction -= HandleButtonIteractAction;        
    }

    public override void Activate()
    {
        _colButtonUp.PullButton();
        _colButtonUp.Activate();
        _signalVfx.SetActive(true);
    }

    public override void Deactivate()
    {
        _colButtonUp.PressButton();
        _colButtonUp.Deactivate();
        _signalVfx.SetActive(false);
    }

    public override void Reset()
    {
        _pointerLookAtCyclerUp.RotateInstantSteps(_originalUp);        
    }

    private void HandleRotationFinishedUp(int currentIndex, Transform target)
    {
        _currentUp = currentIndex;
        CheckSolutionReached();
    }

    private void HandleButtonIteractAction(int idColButton)
    {
        _pointerLookAtCyclerUp.Next();

    }

    

    private void CheckSolutionReached()
    {
        if( _currentUp == 0)
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
