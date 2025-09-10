using UnityEngine;

public class BotPursuitMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _stopDistance = 1.5f;
    [SerializeField] private float _rotationSpeedDegreesPerSecond = 720f;

    [SerializeField] private LayerMask _groundMask = 0;
    [SerializeField] private float _maximumSlopeAngle = 45f;
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [SerializeField] private float _maximumStepHeight = 0.25f;
    [SerializeField] private float _stepCheckDistance = 0.35f;
    [SerializeField] private float _stepClimbSmoothing = 4f;

    private float _groundRaycastSkinOffset = 0.05f;
    private float _lowerStepRaycastVerticalOffset = 0.02f;

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void FixedUpdate()
    {
        if (_playerTransform == null)
        {
            return;
        }

        Vector3 toTarget = _playerTransform.position - transform.position;
        float planarDistance = new Vector2(toTarget.x, toTarget.z).magnitude;

        if (planarDistance <= _stopDistance)
        {
            return;
        }

        Vector3 desiredDirectionWorld = new Vector3(toTarget.x, 0f, toTarget.z).normalized;

        Vector3 groundNormal = GetGroundNormal();

        Vector3 projectedDirection = Vector3.ProjectOnPlane(desiredDirectionWorld, groundNormal).normalized;

        float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
        bool slopeIsWalkable = slopeAngle <= _maximumSlopeAngle;

        if (slopeIsWalkable)
        {
            TryClimbStep(projectedDirection);
        }

        if (slopeIsWalkable)
        {
            Vector3 move = projectedDirection * _movementSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + move);

            Quaternion target = Quaternion.LookRotation(projectedDirection, Vector3.up);
            Quaternion nextRotation = Quaternion.RotateTowards(_rigidbody.rotation, target, _rotationSpeedDegreesPerSecond * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(nextRotation);

        }
    }

    private Vector3 GetGroundNormal()
    {
        float bottomOffset = _capsuleCollider.radius + _groundRaycastSkinOffset;
        Vector3 origin = transform.position + Vector3.up * bottomOffset;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo, _groundCheckDistance + bottomOffset, _groundMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.normal;
        }

        return Vector3.up;
    }

    private void TryClimbStep(Vector3 moveDirection)
    {
        float capsuleBottom = _capsuleCollider.radius + _groundRaycastSkinOffset;

        Vector3 lowerRayOrigin = transform.position + Vector3.up * (capsuleBottom + _lowerStepRaycastVerticalOffset);
        Vector3 upperRayOrigin = lowerRayOrigin + Vector3.up * _maximumStepHeight;

        bool lowerHit = Physics.Raycast(lowerRayOrigin, moveDirection, out RaycastHit lowerHitInfo, _stepCheckDistance, _groundMask, QueryTriggerInteraction.Ignore);
        bool upperHit = Physics.Raycast(upperRayOrigin, moveDirection, _stepCheckDistance, _groundMask, QueryTriggerInteraction.Ignore);

        if (lowerHit && upperHit == false)
        {
            Vector3 stepUp = Vector3.up * (_stepClimbSmoothing * Time.fixedDeltaTime);
            _rigidbody.MovePosition(_rigidbody.position + stepUp);
        }
    }
}