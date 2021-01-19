using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Collections;

public class Bucket : ABucket {
    private float maxRadius;
    private int difficultScore;

    private void OnTriggerEnter(Collider other) {
        SetScore(difficultScore);
    }

    protected override void ChangeDifficult(int value, Vector3 pos) {
        difficultScore = value;
        StartCoroutine(MoveBucket(pos));
    }

    private IEnumerator MoveBucket(Vector3 pos) {
        while (transform.position != pos) {
            transform.position = pos;
            
            //Vector3.Lerp(transform.position, pos, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
