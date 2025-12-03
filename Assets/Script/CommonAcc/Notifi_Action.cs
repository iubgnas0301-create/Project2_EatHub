using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notifi_Action : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _imageBG;

    private float _maxAlpha = 0.98f;

    private void Start() {
        SetActiveTo(false);
    }
    public void Notifi_Act(string message = "Funtion Under Development")
    {
        StopAllCoroutines();
        SetActiveTo(true);
        _text.text = message;
        StartCoroutine(NotifiCoroutine(message));
    }

    IEnumerator NotifiCoroutine(string message)
    {
        SetAlpha(_maxAlpha);
        yield return new WaitForSeconds(2f);

        float tempAlpha = _maxAlpha;
        while (tempAlpha > 0f)
        {
            SetAlpha(tempAlpha);
            tempAlpha -= Time.deltaTime;
            yield return null;
        }

        SetActiveTo(false);
    }

    private void SetAlpha(float value) {
        _imageBG.color = new Color(_imageBG.color.r, _imageBG.color.g, _imageBG.color.b, value);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, value);
    }

    private void SetActiveTo(bool active) {
        _imageBG.gameObject.SetActive(active);
        _text.gameObject.SetActive(active);
    }
}
