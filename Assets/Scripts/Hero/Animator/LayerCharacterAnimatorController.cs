using UnityEngine;

public class LayerHeroAnimatorController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator _animator;

    [Header("Layer Names")]
    [SerializeField] private string _baseLayerName = "Base Layer";
    private int _baseLayerIndex;

    [SerializeField] private string _runLayerName = "Run";
    private int _runLayerIndex;

    [SerializeField] private string _walkLayerName = "Walk";
    private int _walkLayerIndex;

    [SerializeField] private string _crouchLayerName = "Crouch";
    private int _crouchLayerIndex;


    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _baseLayerIndex = _animator.GetLayerIndex(_baseLayerName);
        _runLayerIndex = _animator.GetLayerIndex(_runLayerName);
        _walkLayerIndex = _animator.GetLayerIndex(_walkLayerName);
        _crouchLayerIndex = _animator.GetLayerIndex(_crouchLayerName);

        // Safety check
        ValidateLayer(_baseLayerIndex, _baseLayerName);
        ValidateLayer(_runLayerIndex, _runLayerName);
        ValidateLayer(_walkLayerIndex, _walkLayerName);
        ValidateLayer(_crouchLayerIndex, _crouchLayerName);

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

    public void SetRun()
    {
        SetExclusiveLayer(_runLayerIndex);
    }

    public void SetWalk()
    {
        SetExclusiveLayer(_walkLayerIndex);
    }

    public void SetCrouch()
    {
        Debug.Log("SetExclusiveLayer - Crouch");
        SetExclusiveLayer(_crouchLayerIndex);
    }

    // -------------------------
    // INTERNAL
    // -------------------------

    private void SetExclusiveLayer(int activeLayer)
    {
        if (activeLayer == -1) return;

        _animator.SetLayerWeight(_baseLayerIndex, 0f);
        _animator.SetLayerWeight(_runLayerIndex, 0f);
        _animator.SetLayerWeight(_walkLayerIndex, 0f);
        _animator.SetLayerWeight(_crouchLayerIndex, 0f);

        _animator.SetLayerWeight(activeLayer, 1f);
    }
}
