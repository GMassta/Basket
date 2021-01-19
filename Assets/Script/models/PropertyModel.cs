using System;
using UnityEngine;
using UniRx;

public class PropertyModel {
    public ReactiveProperty<bool> combo { get; private set; }
    public ReactiveProperty<int> scores { get; private set; }
    public ReactiveProperty<int> difficulty { get; private set; }
    public ReactiveProperty<int> comboTimer { get; private set; }

    public PropertyModel() {
        combo = new ReactiveProperty<bool>();
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
        if (value > 0) {
            combo.Value = true;
        } else {
            combo.Value = false;
        }
    }

    public void TimerTick() {
        SetTimer(comboTimer.Value - 1);
        Debug.Log("Tick "+ comboTimer.Value);
    }
}
