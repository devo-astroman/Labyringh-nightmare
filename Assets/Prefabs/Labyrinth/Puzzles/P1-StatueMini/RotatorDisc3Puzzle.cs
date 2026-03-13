using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorDisc3Puzzle  : Puzzle
{
    [SerializeField] PointerLookAtCycler _pointerLookAtCyclerUp;
    [SerializeField] PointerLookAtCycler _pointerLookAtCyclerMiddle;
    [SerializeField] PointerLookAtCycler _pointerLookAtCyclerBottom;
    [SerializeField] private int _currentUp;
    [SerializeField] private int _currentMiddle;
    [SerializeField] private int _currentBottom;

    [SerializeField] ColButton _colButtonUp;
    [SerializeField] ColButton _colButtonMiddle;
    [SerializeField] ColButton _colButtonBottom;
    [SerializeField] GameObject _signalVfx;

    private int _originalUp;
    private int _originalMiddle;
    private int _originalBottom;

    void Start()
    {
        _originalUp = _currentUp;
        _originalMiddle = _currentMiddle;
        _originalBottom = _currentBottom;
        _pointerLookAtCyclerUp.RotateInstantSteps(_currentUp);
        _pointerLookAtCyclerMiddle.RotateInstantSteps(_currentMiddle);
        _pointerLookAtCyclerBottom.RotateInstantSteps(_currentBottom);

        _pointerLookAtCyclerUp.OnRotationFinished += HandleRotationFinishedUp;
        _pointerLookAtCyclerMiddle.OnRotationFinished += HandleRotationFinishedMiddle;
        _pointerLookAtCyclerBottom.OnRotationFinished += HandleRotationFinishedBottom;

        _colButtonUp.SetId(0);
        _colButtonUp.buttonIteractAction += HandleButtonIteractAction;

        _colButtonMiddle.SetId(1);
        _colButtonMiddle.buttonIteractAction += HandleButtonIteractAction;

        _colButtonBottom.SetId(2);
        _colButtonBottom.buttonIteractAction += HandleButtonIteractAction;
    }

    void OnDestroy()
    {
        _pointerLookAtCyclerUp.OnRotationFinished -= HandleRotationFinishedUp;
        _pointerLookAtCyclerMiddle.OnRotationFinished -= HandleRotationFinishedMiddle;
        _pointerLookAtCyclerBottom.OnRotationFinished -= HandleRotationFinishedBottom;

        _colButtonUp.buttonIteractAction -= HandleButtonIteractAction;
        _colButtonMiddle.buttonIteractAction -= HandleButtonIteractAction;
        _colButtonBottom.buttonIteractAction -= HandleButtonIteractAction;
    }
    public override void Activate()
    {
        _colButtonUp.PullButton();
        _colButtonUp.Activate();
        _colButtonMiddle.PullButton();
        _colButtonMiddle.Activate();
        _colButtonBottom.PullButton();
        _colButtonBottom.Activate();
        _signalVfx.SetActive(true);
    }

    public override void Deactivate()
    {
        Debug.Log("_*_Deactivate_*_");
        _colButtonUp.PressButton();
        _colButtonUp.Deactivate();
        _colButtonBottom.PressButton();
        _colButtonBottom.Deactivate();
        _signalVfx.SetActive(false);
    }

    public override void Reset()
    {
        _pointerLookAtCyclerUp.RotateInstantSteps(_originalUp);
        _pointerLookAtCyclerMiddle.RotateInstantSteps(_originalMiddle);
        _pointerLookAtCyclerBottom.RotateInstantSteps(_originalBottom);
        Activate();
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

    private void HandleRotationFinishedBottom(int currentIndex, Transform target)
    {
        _currentBottom = currentIndex;
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

        }else if(idColButton == 2)
        {
            _pointerLookAtCyclerBottom.Next();
        }
    }

    

    private void CheckSolutionReached()
    {
        if(_currentBottom == 0 && _currentMiddle == 0 && _currentUp == 0)
        {
            Debug.Log("Solution Reached!");
        }
        else
        {
            Debug.Log("Try again!");
        }
    }

}
