using UnityEngine;

public class AppManager : MonoBehaviour
{
    private static AppManager _instance;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            if (_instance != this) Destroy(gameObject);
        }
    }
}
