using System;
using System.Collections;
using UnityEngine;

public class PostSlot_Load_Achievement : MonoBehaviour
{
    public Action<E_PostSlot_achievement> GetAchievement_callback;

    private PostSlot_Managemnet postSlot_Manager;
    private Coroutine GetAchievement_Corotine;

    private void Start() {
        postSlot_Manager = GetComponent<PostSlot_Managemnet>();
        GetAchievement_callback = postSlot_Manager.SpawnPost;
        GetInfo();
    }

    private void GetInfo() {
        if (GetAchievement_Corotine != null) StopCoroutine(GetAchievement_Corotine);
        GetAchievement_Corotine = StartCoroutine(WorkWithServer.Instance.GetUserAchievement(GetAchievement_callback));
    }

    public void Refresh() {
        postSlot_Manager.DestroyAllPost();
        GetInfo();
    }
}
