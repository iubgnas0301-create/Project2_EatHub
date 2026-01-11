using UnityEngine;

public class CloseApplication : MonoBehaviour
{
    public void CloseApp()
    {
        FloadUI_Control_v2.instance.Open_YesNoFload("Bạn có chắc chắn muốn thoát không?", "Thoát", "Hủy").OnAnswer += CloseApplication_OnAnswer; ;
        Application.Quit();
    }

    private void CloseApplication_OnAnswer(bool obj) {
        if (obj) Application.Quit();
    }
}
