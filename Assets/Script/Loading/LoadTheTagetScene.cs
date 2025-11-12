using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadTheTagetScene : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _progressText;
    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return null;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Loader._tagetScene);
        while (!asyncLoad.isDone)
        {
            float progress = asyncLoad.progress / 0.9f;
            _progressBar.fillAmount = progress;
            //Debug.Log("Load Progress" + progress);
            _progressText.text = "Loading..." + (progress * 100f).ToString("F0") + "%";
            yield return null;
        }
    }
}
