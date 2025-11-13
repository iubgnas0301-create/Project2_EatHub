using System;
using UnityEngine;

public class PostSlot_Load_Achievement : MonoBehaviour
{
    public Action<E_PostSlot_achievement> GetAchievement_callback;

    private PostSlot_manager postSlot_Manager;
    private Coroutine GetAchievement_Corotine;

    private void Start() {
        postSlot_Manager = GetComponent<PostSlot_manager>();
        GetAchievement_callback = postSlot_Manager.SpawnPost;
    }

    private void OnEnable() {
        if (GetAchievement_Corotine != null) StopCoroutine(GetAchievement_Corotine);
        GetAchievement_Corotine = StartCoroutine(WorkWithServer.Instance.GetUserAchievement(GetAchievement_callback));
    }
    private void OnDisable() {
        postSlot_Manager.DestroyAllPost();
    }

}
