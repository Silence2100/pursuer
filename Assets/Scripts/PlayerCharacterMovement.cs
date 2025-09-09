using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputReader _playerInputReader;
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private bool _useCameraRelativeMovement = true;
    [SerializeField] private float _gravityAcceleration = -9.81f;
    [SerializeField] private float _groundedGravity = -2f;
    [SerializeField] private Transform _cameraTransform;

    private CharacterController characterControllerComponent;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        characterControllerComponent = GetComponent<CharacterController>();

        if (_playerInputReader == null)
        {
            _playerInputReader = GetComponentInParent<PlayerInputReader>();
        }
    }

    private void Update()
    {
        Vector2 moveInput = _playerInputReader != null ? _playerInputReader.ReadMove() : Vector2.zero;
        Vector3 inputVector = new Vector3(moveInput.x, 0f, moveInput.y);

        if (inputVector.sqrMagnitude > 1f)
        {
            inputVector.Normalize();
        }

        if (_cameraTransform == null && Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }

        Vector3 moveDirectionWorld = inputVector;

        if (_useCameraRelativeMovement && _cameraTransform != null)
        {
            Vector3 cameraForward = _cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = _cameraTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            moveDirectionWorld = cameraForward * inputVector.z + cameraRight * inputVector.x;
        }

        Vector3 horizontalVelocity = moveDirectionWorld * _walkSpeed;

        if (characterControllerComponent.isGrounded && verticalVelocity.y < 0f)
        {
            verticalVelocity.y = _groundedGravity;
        }
        else
        {
            verticalVelocity.y += _gravityAcceleration * Time.deltaTime;
        }

        Vector3 totalMotion = horizontalVelocity + Vector3.up * verticalVelocity.y;
        characterControllerComponent.Move(totalMotion * Time.deltaTime);
    }
}