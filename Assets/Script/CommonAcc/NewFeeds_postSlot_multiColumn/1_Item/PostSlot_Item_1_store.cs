using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_Item_1_store : PostSlot_Item_0_Base {

    [SerializeField] protected TextMeshProUGUI rate;
    [SerializeField] protected TextMeshProUGUI feedbackCount;
    [SerializeField] protected TextMeshProUGUI loction;
    [SerializeField] Image _image;
    
    public override void SetInfo(E_PostSlot_0_Base newInfo) {
        E_PostSlot_store info = newInfo as E_PostSlot_store;
        // Validate Information
        if (info is null) { Debug.Log($"Can't read {newInfo} info as store"); return; }

        // Set Information
        Title.text = info.name;
        Content.text = info.description;
        rate.text = info.rate;
        feedbackCount.text = $"(000 feedbacks)";

        // Load Image
        _image.sprite = info.Image? info.Image : null;
    }

    public override void Call2Server(int curentPage, int itemPerPage, Action<E_PostSlot_0_Base> callbackCreateItem, Action callbackEnd) {
        Debug.Log($"{gameObject.name} Call Store Info from Server");
        WorkWithServer.Instance.GetStoreInfo(
            curentPage, itemPerPage,
            callbackCreateItem, callbackEnd);
    }
}
