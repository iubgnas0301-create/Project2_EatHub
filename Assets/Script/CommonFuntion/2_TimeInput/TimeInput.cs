using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeInput : MonoBehaviour
{
    private TMP_InputField input;
    [SerializeField] private TextMeshProUGUI fakeInput;
    public DateTime? value { get; private set; }
    public string temp_Debug;


    private void Awake() {
        input = GetComponent<TMP_InputField>();
    }

    private void Start() {
        input.onValueChanged.AddListener(OnVaLueChange);
        input.onEndEdit.AddListener(OnEndEdit);
    }

    void OnVaLueChange(string input_val) {
        if (input_val.Length > 4) {
            input.text = input.text.RemoveLastChar();
            return;
        }
        if (string.IsNullOrEmpty(input_val)) {
            fakeInput.text = "-- : --";
            return;
        }
        if (!char.IsDigit(input_val.LastChar())) {
            input.text = input.text.RemoveLastChar();
            return;
        }
        string format = input.text.PadLeft(4,'-').Insert(2, " : ");
        fakeInput.text = format;
    }

    private void OnEndEdit(string input_val) {
        if (string.IsNullOrEmpty(input_val)) { value = null; temp_Debug = "time = null"; return; }

        int number = int.Parse(input_val);
        int hour = number / 100;
        int minute = number % 100;
        if (hour > 24 || minute > 59 || (hour * 60 + minute) > (24 * 60)) {
            fakeInput.text = "-- : --";
            input.text = "";
            Notifi_Action.instance.Notifi_Act("Sai định dạng thời gian");
            value = null;
            temp_Debug = $"false {hour} : {minute}";
            return;
        }
        value = new DateTime();
        value.Value.AddHours(hour);
        value.Value.AddMinutes(minute);
        fakeInput.text = $"{hour.ToString("D2")} : {minute.ToString("D2")}";
        temp_Debug = $"set {hour} : {minute}";
    }
}

public static class Extent_class {
    public static string RemoveLastChar(this string s) {
        if (!string.IsNullOrEmpty(s)) s = s.Remove(s.Length - 1);
        return s;
    }
    public static char LastChar(this string s) {
        return s[s.Length - 1];
    }
}
