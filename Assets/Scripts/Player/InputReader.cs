using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private GameInput _input;
    private GameInput.PlayerActions _player;

    private Vector2 _move;
    private Vector2 _lookMouse;
    private Vector2 _lookStick;

    private bool _hasEscapePressedThisFrame;
    private bool _hasLeftClickPressedThisFrame;

    public Vector2 ReadMove() => _move;
    public Vector2 ReadLookMouse() => _lookMouse;
    public Vector2 ReadLookStick() => _lookStick;

    private void Awake()
    {
        _input = new GameInput();
        _player = _input.Player;
    }

    private void OnEnable()
    {
        _player.Move.performed += OnMovePerformed;
        _player.Move.canceled += OnMoveCanceled;

        _player.LookMouse.performed += OnLookMousePerformed;
        _player.LookMouse.canceled += OnLookMouseCanceled;

        _player.LookStick.performed += OnLookStickPerformed;
        _player.LookStick.canceled += OnLookStickCanceled;

        _player.Escape.performed += OnEscapePerformed;
        _player.LeftClick.performed += OnLeftClickPerformed;

        _player.Enable();
    }

    private void OnDisable()
    {
        _player.Disable();

        _player.Move.performed -= OnMovePerformed;
        _player.Move.canceled -= OnMoveCanceled;

        _player.LookMouse.performed -= OnLookMousePerformed;
        _player.LookMouse.canceled -= OnLookMouseCanceled;

        _player.LookStick.performed -= OnLookStickPerformed;
        _player.LookStick.canceled -= OnLookStickCanceled;

        _player.Escape.performed -= OnEscapePerformed;
        _player.LeftClick.performed -= OnLeftClickPerformed;
    }

    private void OnDestroy()
    {
        _input?.Dispose();
    }

    public bool WasEscapePressedThisFrame()
    {
        if (_hasEscapePressedThisFrame)
        {
            _hasEscapePressedThisFrame = false;

            return true;
        }

        return false;
    }

    public bool WasLeftClickPressedThisFrame()
    {
        if (_hasLeftClickPressedThisFrame)
        {
            _hasLeftClickPressedThisFrame = false;

            return true;
        }

        return false;
    }

    private void OnEscapePerformed(InputAction.CallbackContext context) => _hasEscapePressedThisFrame = true;
    private void OnLeftClickPerformed(InputAction.CallbackContext context) => _hasLeftClickPressedThisFrame = true;

    private void OnMovePerformed(InputAction.CallbackContext context) => _move = context.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext context) => _move = Vector2.zero;
    private void OnLookMousePerformed(InputAction.CallbackContext context) => _lookMouse = context.ReadValue<Vector2>();
    private void OnLookMouseCanceled(InputAction.CallbackContext context) => _lookMouse = Vector2.zero;
    private void OnLookStickPerformed(InputAction.CallbackContext context) => _lookStick = context.ReadValue<Vector2>();
    private void OnLookStickCanceled(InputAction.CallbackContext context) => _lookStick = Vector2.zero;
}