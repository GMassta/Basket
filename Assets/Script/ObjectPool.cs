using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T: Object
{
    private Queue<T> pool;

    public ObjectPool(int poolSize) {
        pool = new Queue<T>(poolSize);
    }

    public void Add(T element) {
        pool.Enqueue(element);
    }

    public T Get() {
        T temp = pool.Dequeue();
        Add(temp);

        return temp;
    }

    public void Clear() {
        pool.Clear();
    }

    public int Size() {
        return pool.Count;
    }
}
