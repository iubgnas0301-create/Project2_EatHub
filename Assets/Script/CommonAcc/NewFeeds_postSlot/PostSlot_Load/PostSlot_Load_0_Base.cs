using System;
using UnityEngine;

public class PostSlot_Load_0_Base : MonoBehaviour
{
    protected PostSlot_Managemnet postSlot_Manager;
    protected Coroutine GetInfo_Corotine;

    protected virtual void Start() {
        postSlot_Manager = GetComponent<PostSlot_Managemnet>();
        GetInfo();
    }

    protected virtual void GetInfo() {}

    public virtual void Refresh() {
        postSlot_Manager.DestroyAllPost();
        GetInfo();
    }
}
