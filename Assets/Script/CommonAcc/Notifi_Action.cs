using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notifi_Action : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private Image _image;

    private float _maxAlpha = 0.98f;

    private void Start() {
        _image = GetComponent<Image>();
        gameObject.SetActive(false);
    }
    public void Notifi_Act(string message = "Funtion Under Development")
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
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

        gameObject.SetActive(false);
    }

    private void SetAlpha(float value) {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, value);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, value);
    }
}
