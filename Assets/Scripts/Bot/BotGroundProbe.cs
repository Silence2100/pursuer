using UnityEngine;

public class BotGroundProbe : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask = 0;
    [SerializeField] private float _maximumSlopeAngle = 45f;
    [SerializeField] private float _groundCheckDistance = 0.5f;

    private CapsuleCollider _capsuleCollider;

    private float _groundRaycastSkinOffset = 0.05f;

    public LayerMask GroundMask => _groundMask;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public Vector3 GetGroundNormal()
    {
        float bottomOffset = GetBottomOffset();
        Vector3 origin = transform.position + Vector3.up * bottomOffset;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hitInfo, _groundCheckDistance + bottomOffset, _groundMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.normal;
        }

        return Vector3.up;
    }

    public bool IsSlopeWalkable(Vector3 groundNormal)
    {
        float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
        bool isSlopeWalkable = slopeAngle <= _maximumSlopeAngle;

        return isSlopeWalkable;
    }

    private float GetBottomOffset()
    {
        return _capsuleCollider.radius + _groundRaycastSkinOffset;
    }
}