using UnityEngine;
using UniRx;

public class TouchModel
{
    public ReactiveCommand throwCommand { get; private set; }
    public ReactiveCommand traceCommand { get; private set; } 

    public Vector2 vectorStart { get; private set; } 
    public Vector2 vectorDrag { get; private set; }
    public Vector2 vectorEnd { get; private set; }

    private int screenWidth;
    private int screenHeight;

    private bool isTouch;

    public TouchModel() {
        throwCommand = new ReactiveCommand();
        traceCommand = new ReactiveCommand();

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    public void OnBegin(Vector2 position) {
        isTouch = true;
        vectorStart = ConvertToScreen(position);
    }

    public void OnDrag(Vector3 position) {
        vectorDrag = ConvertToScreen(position);

        if (!isTouch) return;
        if (vectorStart.y - vectorDrag.y > 0) return;
        traceCommand.Execute();
    }

    public void OnEnd(Vector3 position) {
        vectorEnd = ConvertToScreen(position);

        if (!isTouch) return;
        if (vectorStart.y - vectorEnd.y > 0) return;
        throwCommand.Execute();
        isTouch = false;
    }

    private Vector2 ConvertToScreen(Vector2 position) {
        position.x /= screenWidth;
        position.y /= screenHeight;

        return position;
    }
}
