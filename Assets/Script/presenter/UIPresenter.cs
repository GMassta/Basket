using UnityEngine;
using UniRx;
using System;

public class UIPresenter : MonoBehaviour, IUIPresenter
{
    private PropertyModel propertyModel;

    [SerializeField] private GameUI ui;
    [SerializeField] int comboTimerSeconds = 5;

    private IDisposable comboTimer;

    private void Awake() {
        propertyModel = new PropertyModel();
    }

    private void Start() {
        propertyModel.scores
            .Subscribe(v => ui.SetScoreValue(v))
            .AddTo(this);

        propertyModel.combo
            .Subscribe(v => ui.SetCombo(v > 0))
            .AddTo(this);

        propertyModel.combo.Where(v => v > 0)
            .Subscribe(_ => StartComboTimer())
            .AddTo(this);

        ui.difficultButton.OnClickAsObservable()
            .Subscribe(_ => propertyModel.NextDifficult())
            .AddTo(this);
    }

    private void StartComboTimer() {
        comboTimer = propertyModel.comboTimer
            .Where(v => v > 0).Delay(TimeSpan.FromSeconds(1))
            .Subscribe(_ => propertyModel.TimerTick());
    }

    public ReactiveProperty<int> GetDifficulty() {
        return propertyModel.difficulty;
    }

    public void AddScore(int value) {
        propertyModel.AddScore(value);
    }

    public void AddCombo() {
        if(comboTimer != null)
            comboTimer.Dispose();
        propertyModel.SetTimer(comboTimerSeconds);
    }

    public void ResetCombo() {
        propertyModel.ResetCombo();
    }
}
