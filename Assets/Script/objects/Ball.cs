using UnityEngine;

public class Ball : AThrowable
{
    private const float X_LIMIT = .4f;
    private const float Y_LIMIT = .5f;

    public float throwPower = 30f;

    private Rigidbody body;
    private Vector3 direction;

    private bool isReady;

    protected override void Initialization() {
        body = GetComponent<Rigidbody>();
        direction = new Vector3(X_LIMIT, X_LIMIT, 1f);
    }

    protected override void ThrowObject(Vector2 start, Vector2 end) {
        body.isKinematic = false;

        Vector2 dir = (end - start);

        float vx = dir.x * direction.x;
        float vy = dir.y * direction.y;
        float vz = dir.y * direction.z;

        var velocity = new Vector3(vx, vy, vz) * throwPower;
        body.AddForce(velocity, ForceMode.VelocityChange);
    }

    protected override void Prepare() {
        body.isKinematic = true;
    }
}
