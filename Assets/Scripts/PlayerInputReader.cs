using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _lookMouseAction;
    private InputAction _lookStickAction;
    private InputAction _escapeAction;
    private InputAction _leftClickAction;

    private void Awake()
    {
        _moveAction = new InputAction("Move", InputActionType.Value, expectedControlType: "Vector2");

        var wasd = _moveAction.AddCompositeBinding("2DVector");
        wasd.With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");

        var arrows = _moveAction.AddCompositeBinding("2DVector");
        arrows.With("Up", "<Keyboard>/upArrow").With("Down", "<Keyboard>/downArrow").With("Left", "<Keyboard>/leftArrow").With("Right", "<Keyboard>/rightArrow");
        _moveAction.AddBinding("<Gamepad>/leftStick");

        _lookMouseAction = new InputAction("LookMouse", InputActionType.Value, binding: "<Mouse>/delta");
        _lookStickAction = new InputAction("LookStick", InputActionType.Value, binding: "<Gamepad>/rightStick");

        _escapeAction = new InputAction("Escape", InputActionType.Button, "<Keyboard>/escape");
        _leftClickAction = new InputAction("LeftClick", InputActionType.Button, "<Mouse>/leftButton");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _lookMouseAction.Enable();
        _lookStickAction.Enable();
        _escapeAction.Enable();
        _leftClickAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookMouseAction.Disable();
        _lookStickAction.Disable();
        _escapeAction.Disable();
        _leftClickAction.Disable();
    }

    public Vector2 ReadMove()
    {
        return _moveAction.ReadValue<Vector2>();
    }

    public Vector2 ReadLookMouse()
    {
        return _lookMouseAction.ReadValue<Vector2>();
    }

    public Vector2 ReadLookStick()
    {
        return _lookStickAction.ReadValue<Vector2>();
    }

    public bool WasEscapePressedThisFrame()
    {
        return _escapeAction.WasPressedThisFrame();
    }

    public bool WasLeftClickPressedThisFrame()
    {
        return _leftClickAction.WasPressedThisFrame();
    }
}
