using UnityEngine;
using System;

public sealed class InputReader : MonoBehaviour
{
    GameActions input;
    public event Action OnJump;
    public event Action<float> OnMove;

    private void Awake()
    {
        input = new GameActions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Jump.performed += ctx => OnJump?.Invoke();
        input.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<float>());
        input.Player.Move.canceled += ctx => OnMove?.Invoke(0f);
    }

    private void OnDisable()
    {
        input.Disable();
    }
}