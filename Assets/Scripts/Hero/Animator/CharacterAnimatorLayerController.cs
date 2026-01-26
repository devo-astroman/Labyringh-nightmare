using UnityEngine;

public class CharacterAnimatorLayerController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [Header("Layer Names")]
    [SerializeField] private string _baseLayerName = "Base Layer";
    [SerializeField] private string _walkLayerName = "Walk";
    [SerializeField] private string _runLayerName = "Run";

    private int _baseLayerIndex;
    private int _walkLayerIndex;
    private int _runLayerIndex;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _baseLayerIndex = _animator.GetLayerIndex(_baseLayerName);
        _walkLayerIndex = _animator.GetLayerIndex(_walkLayerName);
        _runLayerIndex = _animator.GetLayerIndex(_runLayerName);

        // Safety check
        ValidateLayer(_baseLayerIndex, _baseLayerName);
        ValidateLayer(_walkLayerIndex, _walkLayerName);
        ValidateLayer(_runLayerIndex, _runLayerName);

        // Default state
        SetBase();
    }

    private void ValidateLayer(int index, string name)
    {
        if (index == -1)
            Debug.LogError($"Animator Layer '{name}' not found!");
    }

    // -------------------------
    // PUBLIC API
    // -------------------------

    public void SetBase()
    {
        SetExclusiveLayer(_baseLayerIndex);
    }

    public void SetWalk()
    {
        SetExclusiveLayer(_walkLayerIndex);
    }

    public void SetRun()
    {
        SetExclusiveLayer(_runLayerIndex);
    }

    // -------------------------
    // INTERNAL
    // -------------------------

    private void SetExclusiveLayer(int activeLayer)
    {
        if (activeLayer == -1) return;

        _animator.SetLayerWeight(_baseLayerIndex, 0f);
        _animator.SetLayerWeight(_walkLayerIndex, 0f);
        _animator.SetLayerWeight(_runLayerIndex, 0f);

        _animator.SetLayerWeight(activeLayer, 1f);
    }
}
