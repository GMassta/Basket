using UnityEngine;
using UniRx;

public class BallPocket : APocket
{
    [SerializeField] private Ball prefab;
    [SerializeField] private int count;

    public ReactiveProperty<Ball> selected 
    {
        get;
        private set;
    }

    private ObjectPool<Ball> pool;

    private void Awake() {
        selected = new ReactiveProperty<Ball>();
    }

    private void Start() {
        BuildPool();

        //Select next ball
        selected
            .Where(obj => obj == null)
            .Subscribe(obj => GetObject())
            .AddTo(this);
    }

    public override void ThrowItem(Vector3 direction) {
        if (selected.Value == null) return;
        selected.Value.ThrowBall(direction);
    }

    private void BuildPool() {
        pool = new ObjectPool<Ball>(count);

        for (int i = 0; i < count; i++) {
            var obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);

            pool.Add(obj);

            //Subscribe to selected ball
            obj.isLanded.Where(b => true)
                .Subscribe(_ => UnselectBall())
                .AddTo(this);
        }
    }

    private void GetObject() {
        var obj = pool.Get();
        obj.gameObject.SetActive(true);
        obj.Prepare(transform);

        selected.Value = obj;
    }

    private void UnselectBall() {
        selected.Value = null;
    }

    public override void DrawTrace(Vector3 direction) {
        
    }
}
