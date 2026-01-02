using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberPicker : MonoBehaviour {
    public Action<int> OnValueChanged;

    [Header("Input")]
    [SerializeField] private Button increaseBtn;
    [SerializeField] private Button decreaseBtn;
    [SerializeField] private TMP_InputField inputField;

    [Header("Settings")]
    [SerializeField] private int minValue = 1;
    [SerializeField] private int maxValue = 99;
    [SerializeField] private int currentValue = 1;

    private void Awake() {
        increaseBtn.onClick.AddListener(OnIncreaseClicked);
        decreaseBtn.onClick.AddListener(OnDecreaseClicked);
        inputField.onEndEdit.AddListener(OnInputFieldEdited);
    }

    private void OnInputFieldEdited(string arg0) {
        if (int.TryParse(arg0, out int newValue)) {
            SetValue(newValue);
        } else {
            // Revert to current value if parsing fails
            Debug.Log("Invalid input, reverting to current value");
            inputField.text = currentValue.ToString();
        }
    }

    private void OnDecreaseClicked() {
        SetValue(currentValue - 1);
    }

    private void OnIncreaseClicked() {
        SetValue(currentValue + 1);
    }

    private void SetValue(int newValue) {
        if (newValue < minValue || newValue > maxValue) {
            Debug.Log("Value out of range");
            return;
        }

        currentValue = newValue;
        CheckValue_n_SetAcviveForBtn();
        inputField.text = currentValue.ToString();
        OnValueChanged?.Invoke(currentValue);
        
    }
    private void CheckValue_n_SetAcviveForBtn() {
        decreaseBtn.interactable = !(currentValue <= minValue);
        increaseBtn.interactable = !(currentValue >= maxValue);
    }

    public int GetValue() { return currentValue; }
    public void ResetValue() {
        SetValue(1);
    }
}
