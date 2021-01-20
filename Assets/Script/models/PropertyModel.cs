using System;
using UnityEngine;
using UniRx;

public class PropertyModel {
    public ReactiveProperty<int> combo { get; private set; }
    public ReactiveProperty<int> scores { get; private set; }
    public ReactiveProperty<int> difficulty { get; private set; }
    public ReactiveProperty<int> comboTimer { get; private set; }

    public PropertyModel() {
        combo = new ReactiveProperty<int>();
        scores = new ReactiveProperty<int>();
        difficulty = new ReactiveProperty<int>();
        comboTimer = new ReactiveProperty<int>();
    }

    public void AddScore(int value) {
        scores.Value += value;
    }

    public void NextDifficult() {
        difficulty.Value++;
    }

    public void SetTimer(int value) {
        comboTimer.Value = value;
        combo.Value++;
    }

    public void TimerTick() {
        comboTimer.Value--;

        if (comboTimer.Value <= 0) {
            if(combo.Value > 1)
                AddScore(combo.Value);

            ResetCombo();
        }
    }

    public void ResetCombo() {
        combo.Value = 0;
        comboTimer.Value = 0;
    }
}
