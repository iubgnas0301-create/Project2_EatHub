using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_S0_Base : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Title;
    [SerializeField] protected TextMeshProUGUI Content;
    [SerializeField] protected Image _image;
    public virtual void SetInfo(E_PostSlot_0_Base newInfo) {
        Title.text = newInfo.title;
        Content.text = newInfo.description;
    }
}
