using UnityEngine;
using UniRx;
using System;

public class Ball : MonoBehaviour
{
    private const string LAND = "land";

    private Rigidbody body;
    private bool isReady;

    public ReactiveProperty<bool> isLanded { get; private set; }

    private void Awake() {
        isLanded = new ReactiveProperty<bool>();
        body = GetComponent<Rigidbody>();
    }

    //Throw ball after slide
    public void ThrowBall(Vector3 direction) {
        if (!isReady) return;

        body.isKinematic = false;
        body.AddForce(direction);

        isReady = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == LAND)
            isLanded.Value = true;
    }

    public void Prepare(Transform pocket) {
        isReady = true;
        isLanded.Value = false;
        body.isKinematic = true;
        transform.position = pocket.position;
        transform.rotation = pocket.rotation;
    }
}
