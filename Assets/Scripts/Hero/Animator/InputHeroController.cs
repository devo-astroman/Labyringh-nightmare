using UnityEngine;
using System;

public class InputHeroController : MonoBehaviour
{

    [Header("Walk - Run Switch key")]
    public KeyCode walkKey = KeyCode.LeftShift;
    [Header("Crouch - Stand Switch key")]
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Jump key")]
    public KeyCode spaceKey = KeyCode.Space;

    [Header("Interact key")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Aim key")]
    public KeyCode aimKey = KeyCode.Q;


    public Action walkKeyPressed;
    public Action crouchKeyPressed;
    public Action jumpKeyPressed;
    public Action interactKeyPressed;
    public Action aimKeyPressed;
    public Action fireKeyPressed;

    private bool _blockInputs = false;
    

    void Start()
    {
    }

    void Update()
    {
        if (!_blockInputs)
        {
            if (Input.GetKeyDown(walkKey))
            {
                walkKeyPressed?.Invoke();
            }

            if (Input.GetKeyDown(crouchKey))
            {
                crouchKeyPressed?.Invoke();
            }

            if (Input.GetKeyDown(interactKey))
            {                interactKeyPressed?.Invoke();
            }

            if (Input.GetKeyDown(aimKey))
            {
                aimKeyPressed?.Invoke();
            }

            if (Input.GetKeyDown(spaceKey))
            {
                jumpKeyPressed?.Invoke();
            }

            if (Input.GetMouseButtonDown(0))
            {
                fireKeyPressed?.Invoke();
            }

        }
    }

    public void BlockInputs()
    {
        _blockInputs = true;
    }

    public void AllowInputs()
    {
        _blockInputs = false;
    }
}
