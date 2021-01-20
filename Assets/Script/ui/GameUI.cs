using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text comboLabel;

    public Text scores;
    public Button difficultButton;

    private bool isCombo = true;

    public void SetScoreValue(int value) {
        scores.text = value.ToString();
    }

    public void SetCombo(bool value) {
        bool show = (!isCombo && value);
        bool hide = (isCombo && !value);
        ActivateCombo(show || !hide);
    }

    private void ActivateCombo(bool show) {
        comboLabel.gameObject.SetActive(show);

        isCombo = show;
    }
}
