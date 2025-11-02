using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static int _tagetScene { get; private set; }
    public static void LoadScene(SCENES scene)
    {
        _tagetScene = (int)scene;
        SceneManager.LoadScene(SCENES.LoadingScene.ToString());
    }

    public static void LoadScene(int sceneIndex)
    {
        _tagetScene = sceneIndex;
        SceneManager.LoadScene(SCENES.LoadingScene.ToString());
    }

}
