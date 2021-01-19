using UnityEngine;
using UniRx;

public class TouchModel
{
    public ReactiveProperty<Vector3> throwVector { get; private set; } 
    public ReactiveProperty<Vector3> traceVector { get; private set; }

    private int maxDistance = 1500;
    private int minDistance = 100;

    private float maxHeight;
    private bool isTouch;
    private Vector3 positionStart;

    private Vector3 direction = new Vector3(0f, .5f, 1f);

    public TouchModel(int min, int max) {
        maxDistance = max;
        minDistance = min;

        throwVector = new ReactiveProperty<Vector3>();
        traceVector = new ReactiveProperty<Vector3>();

        maxHeight = Screen.height;
    }

    public void OnBegin(Vector3 position) {
        isTouch = true;
        positionStart = position;
    }

    public void OnDrag(Vector3 position) {
        if (!isTouch) return;
        traceVector.Value = GetDirection(position);
    }

    public void OnEnd(Vector3 position) {
        if (!isTouch) return;
        throwVector.Value = GetDirection(position);
        isTouch = false;
    }

    private Vector3 GetDirection(Vector3 positionEnd) {
        Vector3 direction = new Vector3(0f, .5f, 1f);

        float distance = Vector2.Distance(positionStart, positionEnd);
        distance = distance / (maxHeight / 100);
        distance = (distance * (maxDistance - minDistance)) / 100;
        distance += minDistance;

        bool forward = (positionStart.y - positionEnd.y > 0);

        direction.x = positionStart.x - positionEnd.x;
        direction.y *= distance;
        direction.z *= distance;

        return direction;
    }
}
