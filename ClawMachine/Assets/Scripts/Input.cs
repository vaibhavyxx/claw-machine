using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _pressAction;

    private Vector2 _moveInput;
    private bool _pressed = false;
    public Vector2 direction => _moveInput;
    public bool Pressed => _pressed;

    private void OnEnable()
    {
        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCanceled;
        _moveAction.action.Enable();

        _pressAction.action.performed += OnPressPerformed;
        _pressAction.action.canceled += OnPressCanceled;
        _pressAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCanceled;
        _moveAction.action.Disable();

        _pressAction.action.performed -= OnPressPerformed;
        _pressAction.action.canceled -= OnPressCanceled;
        _pressAction.action.Disable();
    }
    void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    void OnPressPerformed(InputAction.CallbackContext context)
    {
        _pressed = true;
    }
    void OnPressCanceled(InputAction.CallbackContext context)
    {
        _pressed = false;
    }
}

