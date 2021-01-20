using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UniRx;
using static ATarget.CheckStatus;

[RequireComponent(typeof(IUIPresenter))]
public class GamePresenter : MonoBehaviour
{
    private IUIPresenter uiPresenter;
    private TouchModel touchModel;

    [SerializeField] private ATarget bucket;
    [SerializeField] private APocket pocket;
    [Space(10)]
    [SerializeField] private GameDifficulty difficulty;

    private IDisposable ballSubscribe;

    private void Awake() {
        touchModel = new TouchModel();
    }

    void Start()
    {
        uiPresenter = GetComponent<IUIPresenter>();

        TouchActionSubscribe();
        UiControlSubscribe();
        ThrowPocketSubscribe();
        PocketSubscribe();
        BucketGoalSubscribe();
    }

    //Get next throwable object in pocket
    private void PocketSubscribe() {
        pocket.selected
            .Where(v => v != null)
            .Subscribe(v => SubscribeOnThrowable(v))
            .AddTo(this);
    }

    //Throwable change position Subscribe
    private void SubscribeOnThrowable(AThrowable selected) {
        if (ballSubscribe != null)
            ballSubscribe.Dispose();

        ballSubscribe = selected.transform
            .ObserveEveryValueChanged(v => v.position)
            .Subscribe(_ => BucketCheckIn(selected));
    }

    //Check ingress in bucket
    private void BucketCheckIn(AThrowable throwable) {
        var status = bucket.CheckIn(throwable.transform.position, throwable.isClear);
        switch (status) {
            case MISS:
                uiPresenter.ResetCombo();
                pocket.NextObject();
                break;
            case GOAL:
                uiPresenter.AddCombo();
                pocket.NextObject();
                break;
        }
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
            .Subscribe(v => difficulty.SetDifficulty(v))
            .AddTo(this);
    }

    //Subscribe in changed throw vector
    private void ThrowPocketSubscribe() {
        touchModel.throwCommand
            .Select(_ => touchModel)
            .Subscribe(v => 
                pocket.selected.Value.Throw(v.vectorStart, v.vectorEnd))
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
