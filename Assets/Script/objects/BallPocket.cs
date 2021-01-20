using UnityEngine;
using UniRx;
using System;

public class BallPocket : APocket
{
    [SerializeField] private Ball prefab;
    [SerializeField] private int count;
    [Space(10)]
    [Tooltip("seconds")]
    [SerializeField] private float nextSpawnTime;

    private ObjectPool<Ball> pool;
    private ReactiveCommand prepare; 

    private void Start() {
        BuildPool();

        prepare = new ReactiveCommand();
        prepare.Delay(TimeSpan.FromSeconds(nextSpawnTime))
            .Subscribe(_ => PrepareNextObject())
            .AddTo(this);

        NextObject();
    }

    private void BuildPool() {
        pool = new ObjectPool<Ball>(count);

        for (int i = 0; i < count; i++) {
            var obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);

            pool.Add(obj);
        }
    }

    public override void NextObject() {
        var obj = pool.Get();
        obj.Prepare(transform);
        obj.gameObject.SetActive(false);
        SetSelected(obj);

        prepare.Execute();
    }

    private void PrepareNextObject() {
        selected.Value.gameObject.SetActive(true);
    }
}
