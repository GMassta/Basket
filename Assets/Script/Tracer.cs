using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int count;

    private ObjectPool<GameObject> pool;

    private void Start() {
        BuildPool();

        DrawTrace(new Vector3(0f, 5f, 10f), 1);
    }

    private void BuildPool() {
        if (prefab == null)
            throw new UnityException("Select trace elemet prefab");

        pool = new ObjectPool<GameObject>(count);
        for (int i = 0; i < count; i++) {
            var obj = Instantiate(prefab, transform);
            obj.SetActive(false);

            pool.Add(obj);
        }
    }

    public void DrawTrace(Vector3 direction, float gravity) {
        for (int i = 0; i < count; i++) {
            var obj = pool.Get();
            obj.SetActive(true);
            //obj.transform.position += (direction * (i / (count / 100f))) / 100;
            float y = (-.5f * Mathf.Abs(Physics.gravity.y) * (i * i)) + (transform.position.y * i);

            obj.transform.position = new Vector3(0, y, 0);
        }
    }
}
