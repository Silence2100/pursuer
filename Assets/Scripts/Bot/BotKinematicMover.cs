using UnityEngine;

public class BotKinematicMover : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Move(Vector3 directionWorld, float speed, float fixedDeltaTime)
    {
        Vector3 move = directionWorld.normalized * speed * fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + move);
    }

    public void Face(Vector3 directionWorld, float rotationSpeedDegreesPerSecond, float fixedDeltaTime)
    {
        Quaternion target = Quaternion.LookRotation(directionWorld.normalized, Vector3.up);
        Quaternion nextRotation = Quaternion.RotateTowards(_rigidbody.rotation, target, rotationSpeedDegreesPerSecond * fixedDeltaTime);

        _rigidbody.MoveRotation(nextRotation);
    }
}