using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloadUI_Avata_Init : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI UserNameText;
    [SerializeField] private TextMeshProUGUI UserRank;
    [SerializeField] private TextMeshProUGUI UserExp;
    [SerializeField] private Slider UserExpSlider;

    private void OnEnable() {
        UpdateUserInfo();
    }

    public void UpdateUserInfo() {
        E_UserInfo? presentInfo = Static_Info.UserInfo;
        presentInfo ??= new E_UserInfo
        {
            id = 0,
            username = "Guest",
            score = 0
        };
        UserNameText.text = presentInfo.username;
        UserRank.text = "RANK " +  presentInfo.rank.ToString();
        UserExp.text = presentInfo.exp.ToString("000") + "/" + presentInfo.expPerRank.ToString("000");
        UserExpSlider.value = (float)presentInfo.exp / presentInfo.expPerRank;
    }
}
