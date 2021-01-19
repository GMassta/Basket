using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(IUIPresenter))]
public class GamePresenter : MonoBehaviour
{
    private IUIPresenter uiPresenter;
    private TouchModel touchModel;

    [SerializeField] private int minArea = 100;
    [SerializeField] private int maxArea = 1500;

    [Space(10)]
    [SerializeField] private ABucket bucket;
    [SerializeField] private APocket pocket;

    private void Awake() {
        touchModel = new TouchModel(minArea, maxArea);
    }

    void Start()
    {
        uiPresenter = GetComponent<IUIPresenter>();

        TouchActionSubscribe();
        ThrowPocketSubscribe();
        UiControlSubscribe();
        BucketGoalSubscribe();
    }

    //Subscribe in goal
    private void BucketGoalSubscribe() {
        bucket.GetGoalScore()
            .Where(v => v > 0)
            .Subscribe(v => uiPresenter.AddScore(v))
            .AddTo(this);
    }

    //Subscribe in difficulty change
    private void UiControlSubscribe() {
        var dif = uiPresenter.GetDifficulty()
            .Subscribe(v => bucket.SetDifficulty(v))
            .AddTo(this);
    }

    //Subscribe in changed throw vector
    private void ThrowPocketSubscribe() {
        touchModel.throwVector
            .Where(v => v != Vector3.zero)
            .Subscribe(v => pocket.ThrowItem(v))
            .AddTo(this);
    }

    private void TouchActionSubscribe() {
        var touchBegin = TouchSubscribe(TouchPhase.Began);
        var touchDrag = TouchSubscribe(TouchPhase.Moved);
        var touchEnd = TouchSubscribe(TouchPhase.Ended);

        touchBegin.Subscribe(v => touchModel.OnBegin(v)).AddTo(this);
        touchDrag.Subscribe(v => touchModel.OnDrag(v)).AddTo(this);
        touchEnd.Subscribe(v => touchModel.OnEnd(v)).AddTo(this);
    }

    private IObservable<Vector3> TouchSubscribe(TouchPhase phase) {
        return Observable.EveryUpdate()
            .Where(_ => GetTouchPhase(phase))
            .Select(_ => GetTouchPosition());
    }

    //Check touch phase
    private bool GetTouchPhase(TouchPhase phase) {
        if (Input.touchCount < 1)
            return false;
        Touch touch = Input.touches[0];
        //Ignore UI touch
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return false;

        return touch.phase == phase;
    }

    //Get touch position
    private Vector3 GetTouchPosition() {
        return Input.touches[0].position;
    }
}
