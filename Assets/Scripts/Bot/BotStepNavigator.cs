using UnityEngine;

public class BotStepNavigator : MonoBehaviour
{
    [SerializeField] private float _maximumStepHeight = 0.25f;
    [SerializeField] private float _stepCheckDistance = 0.35f;
    [SerializeField] private float _stepClimbSmoothing = 4f;

    private float _lowerStepRaycastVerticalOffset = 0.02f;
    private float _groundRaycastSkinOffset = 0.05f;

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private BotGroundProbe _groundProbe;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _groundProbe = GetComponent<BotGroundProbe>();
    }

    public void TryClimbStep(Vector3 moveDirection, float fixedDeltaTime)
    {
        moveDirection.Normalize();

        float capsuleBottom = _capsuleCollider.radius + _groundRaycastSkinOffset;

        Vector3 lowerRayOrigin = transform.position + Vector3.up * (capsuleBottom + _lowerStepRaycastVerticalOffset);
        Vector3 upperRayOrigin = lowerRayOrigin + Vector3.up * _maximumStepHeight;

        bool hasLowerHit = Physics.Raycast(lowerRayOrigin, moveDirection, out RaycastHit lowerHitInfo, _stepCheckDistance, _groundProbe.GroundMask, QueryTriggerInteraction.Ignore);
        bool hasUpperHit = Physics.Raycast(upperRayOrigin, moveDirection, _stepCheckDistance, _groundProbe.GroundMask, QueryTriggerInteraction.Ignore);

        if (hasLowerHit && hasUpperHit == false)
        {
            Vector3 stepUp = Vector3.up * (_stepClimbSmoothing * fixedDeltaTime);
            _rigidbody.MovePosition(_rigidbody.position + stepUp);
        }
    }
}