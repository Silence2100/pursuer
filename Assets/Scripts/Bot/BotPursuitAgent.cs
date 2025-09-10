using System;
using UnityEngine;

public class BotPursuitAgent : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _stopDistance = 1.5f;
    [SerializeField] private float _rotationSpeedDegreesPerSecond = 720f;

    private BotGroundProbe _groundProbe;
    private BotStepNavigator _stepNavigator;
    private BotKinematicMover _kinematicMover;

    private bool _isWithinStopRange;

    public event Action<bool> StopRangeStateChanged;
    public event Action<Vector3> DesiredDirectionChanged;

    private void Awake()
    {
        _groundProbe = GetComponent<BotGroundProbe>();
        _stepNavigator = GetComponent<BotStepNavigator>();
        _kinematicMover = GetComponent<BotKinematicMover>();
    }

    private void FixedUpdate()
    {
        if (_playerTransform == null)
        {
            SetStopRangeState(true);

            return;
        }

        Vector3 toTarget = _playerTransform.position - transform.position;
        float planarDistance = new Vector2(toTarget.x, toTarget.z).magnitude;

        bool isCloseEnough = planarDistance <= _stopDistance;
        SetStopRangeState(isCloseEnough);

        if (isCloseEnough)
        {
            return;
        }

        Vector3 desiredDirectionWorld = new Vector3(toTarget.x, 0f, toTarget.z).normalized;

        Vector3 groundNormal = _groundProbe.GetGroundNormal();
        Vector3 projectedDirection = Vector3.ProjectOnPlane(desiredDirectionWorld, groundNormal).normalized;

        DesiredDirectionChanged?.Invoke(projectedDirection);

        if (_groundProbe.IsSlopeWalkable(groundNormal) == false)
        {
            return;
        }

        _stepNavigator.TryClimbStep(projectedDirection, Time.fixedDeltaTime);

        _kinematicMover.Move(projectedDirection, _movementSpeed, Time.fixedDeltaTime);
        _kinematicMover.Face(projectedDirection, _rotationSpeedDegreesPerSecond, Time.fixedDeltaTime);
    }

    private void SetStopRangeState(bool isWithinRange)
    {
        if (_isWithinStopRange == isWithinRange)
        {
            return;
        }

        _isWithinStopRange = isWithinRange;
        StopRangeStateChanged?.Invoke(_isWithinStopRange);
    }
}