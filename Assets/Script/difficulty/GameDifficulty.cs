using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficulty : MonoBehaviour
{

    [SerializeField] private ATarget target;
    
    private List<DifficultPoint> points;
    private DifficultPoint currentPoint;

    private void Awake() {
        points = new List<DifficultPoint>();

        foreach (Transform child in transform) {
            var point = child.GetComponent<DifficultPoint>();
            if (point != null)
                points.Add(point);
        }
    }

    public void SetDifficulty(int index) {
        if (points.Count <= 0) return;
        currentPoint = points[index % points.Count];
        SetTargetDifficulty();
    }

    private void SetTargetDifficulty() {
        target.ChangeDifficult(currentPoint.GetScore(), currentPoint.transform.position);
    }
}
