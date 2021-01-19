using UnityEngine;
using UniRx;
using System;

public class UIPresenter : MonoBehaviour, IUIPresenter
{
    private PropertyModel propertyModel;

    [SerializeField] private GameUI ui;
    [SerializeField] int comboTimerSeconds = 5;

    private void Awake() {
        propertyModel = new PropertyModel();
    }

    private void Start() {
        propertyModel.scores.Subscribe(v => ui.SetScoreValue(v));
        propertyModel.combo.Subscribe(v => ui.SetCombo(v));

        propertyModel.comboTimer
            .Where(v => v > 0)
            .Delay(TimeSpan.FromSeconds(1))
            .Subscribe(_ => propertyModel.TimerTick());

        var diff = ui.difficultButton.OnClickAsObservable();
        diff.Subscribe(_ => propertyModel.NextDifficult());
    }

    public ReactiveProperty<int> GetDifficulty() {
        return propertyModel.difficulty;
    }

    public void AddScore(int value) {
        propertyModel.AddScore(value);
        propertyModel.SetTimer(comboTimerSeconds);
    }
}
