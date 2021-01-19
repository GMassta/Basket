using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public abstract class ABucket : MonoBehaviour
{
    [SerializeField]
    private List<DifficultElement> difficultyList;
    public ReactiveProperty<int> goalScore;

    private void Awake() {
        goalScore = new ReactiveProperty<int>();
        goalScore.Select(v => v > 0).Subscribe(_ => goalScore.Value = 0);
    }

    //Get Goal Score
    public ReactiveProperty<int> GetGoalScore() {
        return goalScore;
    }

    //Change bucket object difficult
    public void SetDifficulty(int index) {
        if (difficultyList == null || difficultyList.Count <= 0) return;

        var element = difficultyList[index % difficultyList.Count];
        ChangeDifficult(element.value, element.bucketPosition);
    }

    [Serializable]
    private struct DifficultElement {
        public int value;
        public Vector3 bucketPosition;
    }

    //Set new goal score value
    protected void SetScore(int value) {
        goalScore.Value = value;
    }

    protected abstract void ChangeDifficult(int value, Vector3 pos);
}
