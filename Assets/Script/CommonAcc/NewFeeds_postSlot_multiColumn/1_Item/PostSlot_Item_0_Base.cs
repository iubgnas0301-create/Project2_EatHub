using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_Item_0_Base : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Title;
    [SerializeField] protected TextMeshProUGUI Content;

    public virtual void SetInfo(E_PostSlot_0_Base newInfo)
    {
        Title.text = newInfo.title;
        Content.text = newInfo.description;
    }
}
