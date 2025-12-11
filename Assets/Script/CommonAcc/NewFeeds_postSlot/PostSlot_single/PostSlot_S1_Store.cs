using TMPro;
using UnityEngine;

public class PostSlot_S1_Store : PostSlot_S0_Base
{
    [SerializeField] private TextMeshProUGUI rate;
    [SerializeField] private TextMeshProUGUI numberOfFeedback;
    [SerializeField] private TextMeshProUGUI location;
    public override void SetInfo(E_PostSlot_Base newInfo) {
        E_PostSlot_store newEvent = newInfo as E_PostSlot_store;
        if (newEvent == null) {
            Debug.Log("None Info"); return;
        }
        Title.text = newEvent.title;
        Content.text = newEvent.description;
    }
}
