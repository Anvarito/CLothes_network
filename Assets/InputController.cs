using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Vector2 move { get; private set; }
    public Vector2 look { get; private set; }

    private PlayerInput inputs;
    private void OnEnable()
    {
        inputs.Player.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    private void Awake()
    {
        inputs = new PlayerInput();

        inputs.Player.Move.performed += context => move = context.ReadValue<Vector2>();
        inputs.Player.Move.canceled += context => move = Vector2.zero;

        inputs.Player.Look.performed += context => look = context.ReadValue<Vector2>();
        inputs.Player.Look.canceled += context => look = Vector2.zero;
    }

}
