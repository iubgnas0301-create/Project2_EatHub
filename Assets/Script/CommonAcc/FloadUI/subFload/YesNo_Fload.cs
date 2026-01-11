using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YesNo_Fload : MonoBehaviour
{
    public event Action<bool> OnAnswer;

    [SerializeField] private TextMeshProUGUI _QuestionText;
    [SerializeField] private Button _Yes;
    [SerializeField] private Button _No;
    [SerializeField] private TextMeshProUGUI _YesText;
    [SerializeField] private TextMeshProUGUI _NoText;

    private void Start()
    {
        _Yes.onClick.AddListener(OnTryAgaintClicked);
        _No.onClick.AddListener(OnSkipClicked);
    }
    public void SetQuestion(string question) {
        _QuestionText.text = question;
    }
    private void OnTryAgaintClicked() {
        OnAnswer?.Invoke(true);
        FloadUI_Control_v2.instance.Close_Fload();
    }

    private void OnSkipClicked() {
        OnAnswer?.Invoke(false);
        FloadUI_Control_v2.instance.Close_Fload();
    }
    public void SetNameOfYesNoBtn(string yesString, string noString) {
        _YesText.text = yesString;
        _NoText.text = noString;
    }   
}
