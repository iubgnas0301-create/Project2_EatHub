using TMPro;
using UnityEngine;

public class PostSlot_S1_Event : PostSlot_S0_Base
{
    [SerializeField] private TextMeshProUGUI post_time;
    public override void SetInfo(E_PostSlot_Base newInfo) {
        E_PostSlot_event newEvent = newInfo as E_PostSlot_event;
        if (newEvent == null) {
            Debug.Log("None Info"); return;
        }
        Title.text = newEvent.title;
        Content.text = newEvent.description;
        post_time.text = newEvent.post_time;
    }
}
