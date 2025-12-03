using UnityEngine;

public class PostSlot_Load_Event : PostSlot_Load_0_Base
{
    [SerializeField] private PostSlot_LoadRefreshCondition condition;
    private int PageIndex = 0;

    protected override void Start() {
        base.Start();
        condition.OnTopReach += Refresh;
        condition.OnEndReach += GetInfo;
    }
    protected override void GetInfo() {
        if (GetInfo_Corotine is not null) { StopCoroutine(GetInfo_Corotine); }
        GetInfo_Corotine = StartCoroutine(WorkWithServer.Instance.GetEvent(PageIndex, postSlot_Manager.SpawnPost));
        PageIndex++;
    }
    public override void Refresh() {
        PageIndex = 0;
        base.Refresh();
    }
}
