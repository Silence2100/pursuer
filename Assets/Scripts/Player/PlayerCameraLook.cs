using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{
    [SerializeField] private InputReader _playerInputReader;
    [SerializeField] private Transform _playerYawTransform;
    [SerializeField] private float _mouseDegreesPerPixel = 0.15f;
    [SerializeField] private float _stickDegreesPerSecond = 180f;

    [SerializeField] private bool _isYInverted = false;
    [SerializeField] private bool _isCursorLockedOnStart = true;

    [SerializeField] private float _minPitch = -70f;
    [SerializeField] private float _maxPitch = 80f;

    private float currentPitch;

    private void Awake()
    {
        if (_isCursorLockedOnStart)
        {
            LockCursor(true);
        }
    }

    private void Update()
    {
        if (_playerInputReader != null)
        {
            if (_playerInputReader.WasEscapePressedThisFrame())
            {
                LockCursor(false);
            }

            if (Cursor.lockState != CursorLockMode.Locked && _playerInputReader.WasLeftClickPressedThisFrame())
            {
                LockCursor(true);
            }
        }

        Vector2 mouseDelta = _playerInputReader != null ? _playerInputReader.ReadLookMouse() : Vector2.zero;
        Vector2 stickDelta = _playerInputReader != null ? _playerInputReader.ReadLookStick() : Vector2.zero;

        float yawDelta =
            mouseDelta.x * _mouseDegreesPerPixel +
            stickDelta.x * _stickDegreesPerSecond * Time.deltaTime;

        float pitchDelta =
            (mouseDelta.y * _mouseDegreesPerPixel +
             stickDelta.y * _stickDegreesPerSecond * Time.deltaTime) * (_isYInverted ? 1f : -1f);

        if (_playerYawTransform != null)
        {
            _playerYawTransform.Rotate(Vector3.up, yawDelta, Space.World);
        }

        currentPitch = Mathf.Clamp(currentPitch + pitchDelta, _minPitch, _maxPitch);
        transform.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
    }

    private void LockCursor(bool shouldLock)
    {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}