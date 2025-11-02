using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static SCENES _tagetScene { get; private set; }
    public static void LoadScene(SCENES scene)
    {
        _tagetScene = scene;
        SceneManager.LoadScene(SCENES.LoadingScene.ToString());
    }
}
