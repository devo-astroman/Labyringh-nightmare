using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorDisc3Puzzle : MonoBehaviour
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

    private int _solutionUp = 0;
    private int _solutionMiddle = 0;
    private int _solutionBottom = 0;

    void Start()
    {
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
