using System;
using UnityEngine;

public sealed class InputReader : MonoBehaviour
{
    GameActions input;
    public event Action OnJump;
    public event Action<float> OnMove;

    void Awake()
    {
        input = new GameActions();
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Jump.performed += ctx => OnJump?.Invoke();
        input.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<float>());
        input.Player.Move.canceled += ctx => OnMove?.Invoke(0f);
    }

    void OnDisable()
    {
        input.Disable();
    }
}