using UnityEngine;

public class PostSlot_Load_1_BaseRELoadable : PostSlot_Load_0_Base
{
    [SerializeField] private PostSlot_LoadRefreshCondition condition;
    protected int PageIndex = 0;

    protected override void Start() {
        base.Start();
        condition.OnTopReach += Refresh;
        condition.OnEndReach += GetInfo;
    }
    public override void Refresh() {
        PageIndex = 0;
        base.Refresh();
    }
}
