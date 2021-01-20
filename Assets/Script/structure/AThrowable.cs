using UnityEngine;
using UniRx;

public abstract class AThrowable : MonoBehaviour
{
    public bool isClear { get; private set; }
    public ReactiveProperty<bool> isLanded { get; private set; }

    private bool isThrowed;

    private void Awake() {
        isLanded = new ReactiveProperty<bool>();

        Initialization();
    }

    //After collision any objects, throw not clear
    private void OnCollisionEnter(Collision collision) {
        if (isClear && collision.impulse.magnitude > 2f)
            isClear = false;
    }

    public void Land() {
        isLanded.Value = true;
    }

    public void Throw(Vector2 start, Vector2 end) {
        if (!isThrowed) {
            ThrowObject(start, end);
            isThrowed = true;
        }
    }

    public void Prepare(Transform startPosition) {
        isThrowed = false;
        isLanded.Value = false;
        isClear = true;

        Prepare();

        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;
    }

    protected abstract void Initialization();
    protected abstract void ThrowObject(Vector2 start, Vector2 end);
    protected abstract void Prepare();
}
