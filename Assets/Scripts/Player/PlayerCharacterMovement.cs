using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float _walkSpeed = 5f;

    [SerializeField] private bool _isCameraRelativeMovementEnabled = true;

    [SerializeField] private float _gravityAcceleration = -9.81f;
    [SerializeField] private float _groundedGravity = -2f;
    [SerializeField] private Transform _cameraTransform;

    private CharacterController _characterController;
    private Vector3 _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (_inputReader == null)
        {
            _inputReader = GetComponentInParent<InputReader>();
        }
    }

    private void Update()
    {
        Vector2 moveInput = _inputReader != null ? _inputReader.ReadMove() : Vector2.zero;
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

        if (_isCameraRelativeMovementEnabled && _cameraTransform != null)
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

        if (_characterController.isGrounded && _verticalVelocity.y < 0f)
        {
            _verticalVelocity.y = _groundedGravity;
        }
        else
        {
            _verticalVelocity.y += _gravityAcceleration * Time.deltaTime;
        }

        Vector3 totalMotion = horizontalVelocity + Vector3.up * _verticalVelocity.y;
        _characterController.Move(totalMotion * Time.deltaTime);
    }
}