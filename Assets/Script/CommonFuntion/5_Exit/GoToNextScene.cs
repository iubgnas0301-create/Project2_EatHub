using UnityEngine;

public class GoToNextScene : MonoBehaviour
{
    [SerializeField] private SCENES nextScene;

    public void GoToScene()
    {
        FloadUI_Control_v2.instance.Open_YesNoFload(
            "Bạn có chắc chắn muốn đăng xuất không?",
            "Đăng xuất", "Hủy"
        ).OnAnswer += LogOut;

        void LogOut(bool ans) {
            if (ans) Loader.LoadScene(nextScene);
        }
    }
}
