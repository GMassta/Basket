using UnityEngine;
using UniRx;

public abstract class ATarget : MonoBehaviour
{
    public enum CheckStatus {
        FLY, GOAL, MISS
    }

    [HideInInspector]
    public ReactiveProperty<int> goalScore;

    private void Awake() {
        goalScore = new ReactiveProperty<int>();
        goalScore.Select(v => v > 0).Subscribe(_ => goalScore.Value = 0);
    }

    //Get Goal Score
    public ReactiveProperty<int> GetGoalScore() {
        return goalScore;
    }

    //Set new goal score value
    protected void SetScore(int value) {
        goalScore.Value = value;
    }

    //Change target difficult
    public abstract void ChangeDifficult(int value, Vector3 pos);
    //Return false if vector hight more a target hight
    public abstract CheckStatus CheckIn(Vector3 position, bool bonus);
}
