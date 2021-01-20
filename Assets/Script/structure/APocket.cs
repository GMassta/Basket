using UnityEngine;
using UniRx;

public abstract class APocket: MonoBehaviour
{
    public ReactiveProperty<AThrowable> selected { get; private set; }

    private void Awake() {
        selected = new ReactiveProperty<AThrowable>();
    }

    protected void SetSelected(AThrowable throwable) {
        selected.Value = throwable;
    }

    //Select next ball
    abstract public void NextObject();
}
