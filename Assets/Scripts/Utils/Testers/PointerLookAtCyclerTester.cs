using UnityEngine;

public class PointerLookAtCyclerTester : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private PointerLookAtCycler _pointerLookAtCyclerTester;

    [SerializeField] private bool _testNext = false;
    [SerializeField] private bool _testPrev = false;

    void Start()
    {
        _pointerLookAtCyclerTester.OnRotationFinished += HandleRotationFinished;
    }

    void Update()
    {
        if (_testNext)
        {
            _pointerLookAtCyclerTester.Next();
            _testNext = false;
        }

        if (_testPrev)
        {
            _pointerLookAtCyclerTester.Prev();
            _testPrev = false;
        }
    }

    void OnDestroy()
    {
        _pointerLookAtCyclerTester.OnRotationFinished -= HandleRotationFinished;
    }

    private void HandleRotationFinished(int currentIndex, Transform target)
    {
        Debug.Log("currentIndex: " + currentIndex);
        Debug.Log("target: " + target.gameObject.name);
    }

}