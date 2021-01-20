using UnityEngine;
using UniRx;

public class DifficultPoint : MonoBehaviour
{
    [SerializeField, Range(1,20)]
    private int score = 1;

    public int GetScore() {
        return score;
    }
}
