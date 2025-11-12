using UnityEngine;
using UnityEngine.UI;

public class FloadUI_Call : MonoBehaviour
{
    [SerializeField] private GameObject[] floatUI_toShow;

    private Button Btn_OpenFloadUI;
    void Start()
    {
        Btn_OpenFloadUI = GetComponent<Button>();
        Btn_OpenFloadUI.onClick.AddListener(ShowFloads);
    }

    public void ShowFloads() {
        if (floatUI_toShow is null) {
            Debug.Log("Havent set Float UI to show yet");
            return;
        }
        FloadUI_Control.Instance.ShowFloads(floatUI_toShow);
    }
}
