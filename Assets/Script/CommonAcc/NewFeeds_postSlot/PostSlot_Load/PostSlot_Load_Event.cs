using UnityEngine;

public class PostSlot_Load_Event : PostSlot_Load_1_BaseRELoadable
{
    protected override void GetInfo() {
        if (GetInfo_Corotine is not null) { StopCoroutine(GetInfo_Corotine); }
        GetInfo_Corotine = StartCoroutine(WorkWithServer.Instance.GetEvent(PageIndex, postSlot_Manager.SpawnPost));
        PageIndex++;
    }
}
