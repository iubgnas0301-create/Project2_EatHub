using TMPro;
using UnityEngine;

public class PostSlot_S0_Base : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Title;
    [SerializeField] protected TextMeshProUGUI Content;
    public virtual void SetInfo(E_PostSlot_Base newInfo) {
        Title.text = newInfo.title;
        Content.text = newInfo.description;
    }
}
