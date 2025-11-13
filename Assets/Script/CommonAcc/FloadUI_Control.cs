using UnityEngine;
using UnityEngine.UI;

public class FloadUI_Control : MonoBehaviour
{
    public static FloadUI_Control Instance;

    private GameObject[] floadUI_s;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    void Start()
    {
        HideFloads();
    }

    public void HideFloads() {
        if (floadUI_s is not null)
            foreach (var theUI in floadUI_s) {
                theUI.SetActive(false);
            }
        floadUI_s = null;
        gameObject.SetActive(false);
    }

    public void ShowFloads(GameObject[] UI_needShow) {
        if( floadUI_s is not null)
            foreach (var theUI in floadUI_s) {
                theUI.SetActive(false);
            }
        floadUI_s = UI_needShow;

        gameObject.SetActive(true);
        foreach (var theUI in UI_needShow) {
            theUI.SetActive(true);
        }
    }
}
