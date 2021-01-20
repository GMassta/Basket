using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Bucket : ATarget {
    [SerializeField] private float bucketHight = 2f;
    [SerializeField] private float maxRadius = 1f;
    [Space(10)]
    [SerializeField] private List<BonusRing> rings;

    private int difficultScore;

    public override void ChangeDifficult(int value, Vector3 pos) {
        difficultScore = value;
        StartCoroutine(MoveBucket(pos));
    }

    //Check bucket goal
    public override CheckStatus CheckIn(Vector3 position, bool bonus) {
        if (position.y <= bucketHight) {
            Vector3 plane = position - transform.position;
            plane.y = 0;

            float distance = plane.magnitude;
            if (distance > maxRadius) return CheckStatus.MISS;

            int score = difficultScore;

            //Clear shot add bonus
            if (bonus) {
                foreach (BonusRing ring in rings) {
                    if ((maxRadius * ring.radius * .01f) >= distance) {
                        score += ring.score;
                        break;
                    }
                }
            }

            SetScore(score);
            return CheckStatus.GOAL;

        }
        return CheckStatus.FLY;
    }

    //Move bucket
    private IEnumerator MoveBucket(Vector3 pos) {
        float elapse = 0f;
        float speed = 2f;

        while (transform.position != pos) {
            elapse += speed * Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, pos, elapse);
            yield return new WaitForEndOfFrame();
        }
    }

#if UNITY_EDITOR
    //Gizmo
    private void OnDrawGizmos() {
        foreach(BonusRing ring in rings) {
            Vector3 position = transform.position;
            position.y = bucketHight;

            DrawWireDisk(position, (maxRadius * ring.radius / 100), ring.gizmoColor);
        }
    }

    private const float GIZMO_DISK_THICKNESS = 0.01f;
    public static void DrawWireDisk(Vector3 position, float radius, Color color) {
        Color oldColor = Gizmos.color;
        Gizmos.color = color;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.identity, new Vector3(1, GIZMO_DISK_THICKNESS, 1));
        Gizmos.DrawWireSphere(Vector3.zero, radius);
        Gizmos.matrix = oldMatrix;
        Gizmos.color = oldColor;
    }

#endif

    [Serializable]
    private struct BonusRing {
        public int score;
        [Range(0, 100)] public int radius;

        public Color gizmoColor;
    } 
}
