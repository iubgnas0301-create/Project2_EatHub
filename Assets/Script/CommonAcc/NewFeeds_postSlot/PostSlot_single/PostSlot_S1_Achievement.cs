using TMPro;
using UnityEngine;

public class PostSlot_S1_Achievement : PostSlot_S0_Base
{
    [SerializeField] private TextMeshProUGUI a_dateTime;
    public override void SetInfo(E_PostSlot_0_Base newInfo) {
        E_PostSlot_achievement newAchiev = newInfo as E_PostSlot_achievement;
        if (newAchiev == null) {
            Debug.Log("None Info"); return;
        }
        Title.text = newAchiev.title;
        Content.text = newAchiev.description;
        a_dateTime.text = newAchiev.a_date;
    }
}
