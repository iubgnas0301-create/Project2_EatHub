using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PostSlot_Item_1_event : PostSlot_Item_0_Base {
    [SerializeField] private TextMeshProUGUI rate;
    [SerializeField] private TextMeshProUGUI feedbackCount;
    [SerializeField] private TextMeshProUGUI DatetimepPost;
    [SerializeField] private Image _image;

    public override void SetInfo(E_PostSlot_0_Base newInfo) {
        E_PostSlot_event info = newInfo as E_PostSlot_event;
        // Validate Information
        if (info is null) { Debug.Log($"Can't read {newInfo} info as event"); return; }

        // Set Information
        Title.text = "brand name";
        Content.text = $"{info.title}\n<color=#FFFFFF88>{info.description}</color>";
        rate.text = "0.0";
        feedbackCount.text = $"(000 feedbacks)";
        DatetimepPost.text = info.post_time;

        // Load Image
        _image.sprite = info._image;
    }
    public override void Call2Server(int curentPage, int itemPerPage, Action<E_PostSlot_0_Base> callbackCreateItem, Action callbackEnd) {
        Debug.Log($"{gameObject.name} Call Event Info from Server");
        WorkWithServer.Instance.GetEventInfo(
            curentPage, itemPerPage,
            callbackCreateItem, callbackEnd);
    }
}
