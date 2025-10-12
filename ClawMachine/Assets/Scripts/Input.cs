using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    private Vector2 _moveInput;
    public Vector2 direction => _moveInput;

    private void OnEnable()
    {
        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCanceled;
        _moveAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCanceled;
        _moveAction.action.Disable();
    }
    void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {_moveInput}");
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }
}

